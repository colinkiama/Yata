using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Core;
using YATA.Core.Syncing;
using YATA.Enums;
using YATA.External;
using YATA.Model;
using YATA.Services;

namespace YATA
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public static CloudSyncService syncService = new CloudSyncService();
        public static Navigation NavService { get; set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.EnteredBackground += App_EnteredBackground;
            ApplicationData.Current.DataChanged += Current_DataChanged;
            ToDoTask.ListOfTasksChanged += ToDoTask_ListOfTasksChanged;
        }


        private void ToDoTask_ListOfTasksChanged(object sender, EventArgs e)
        {
            TileService.UpdateLiveTile(sender as ObservableCollection<ToDoTask>);
        }

        private void Current_DataChanged(ApplicationData sender, object args)
        {
            RoamingSync.UpdateDataChange();
        }

        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await new FileIOService().saveData();
            deferral.Complete();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {
            await Engagement.RegisterEngagementService();
            var result = Mango.App.appVersionChecker.getAppVersionStatus();
            switch (result)
            {
                case Mango.Enums.appVersionStatus.FirstTime:
                    await new FileIOService().saveData();
                    TileService.UpdateLiveTile(ToDoTask.listOfTasks);
                    break;
                case Mango.Enums.appVersionStatus.Old:
                case Mango.Enums.appVersionStatus.Current:
                    await new FileIOService().loadData();
                    break;
            }




            RoamingSync.RestoreScore();

            var TopBarColor = (Color)Application.Current.Resources["SystemAccentColor"];
            if (DeviceDetection.DetectDeviceType() == DeviceType.Phone)
            {
                var statusBar = StatusBar.GetForCurrentView();
                statusBar.BackgroundColor = TopBarColor;
                statusBar.BackgroundOpacity = 1;
                statusBar.ForegroundColor = Colors.White;
            }

            else
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.BackgroundColor = TopBarColor;
                titleBar.ForegroundColor = Colors.White;
                titleBar.ButtonBackgroundColor = TopBarColor;
                titleBar.InactiveBackgroundColor = TopBarColor;
                titleBar.ButtonInactiveBackgroundColor = TopBarColor;

            }

            var appView = ApplicationView.GetForCurrentView();
            appView.SetPreferredMinSize(new Size(256, 256));


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    PageStuff.navigating = true;
                    NavService = new Navigation(ref rootFrame);
                    //if (Settings.GetWhetherOnBoardingPageHasBeenViewed() == null)
                    //{
                    //    rootFrame.Navigate(typeof(OnboardingPage), e.Arguments);
                    //}
                    //else
                    //{
                    //    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                    //}
                    NavService.Navigate(typeof(MainPage), e.Arguments);


                }
                // Ensure the current window is active
                Window.Current.Activate();
            }


            rootFrame.Navigated += RootFrame_Navigated;
            // After  Window.Current.Content = rootFrame; 
            // Register a handler for BackRequested events and set the  
            // visibility of the Back button  
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                 rootFrame.CanGoBack ?
                 AppViewBackButtonVisibility.Visible :
                 AppViewBackButtonVisibility.Collapsed;
        }



        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }

        private void RootFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var navigatedFrame = (Frame)sender;
            if (navigatedFrame.CurrentSourcePageType == typeof(MainPage))
            {
                navigatedFrame.BackStack.Clear();
            }

            // Each time a navigation event occurs, update the Back button's visibility  
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;

        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }

    }
}
