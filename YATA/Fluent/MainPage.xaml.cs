using Microsoft.Advertising.WinRT.UI;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using YATA.Control;
using YATA.Core;
using YATA.Core.Syncing;
using YATA.External;
using YATA.Model;
using YATA.Phone;
using YATA.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YATA.Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FluentMainPage : Page
    {
        public ObservableCollection<ToDoTask> localListOfTasks = ToDoTask.listOfTasks;
        private string title = "Tasks";
        public FluentMainPage()
        {
            this.InitializeComponent();
            decideToShowAds();
            ScoreTextBlock.Text = ToDoTask.CompletedTasks.ToString();
            ToDoTask.listOfTasks.CollectionChanged += ListOfTasks_CollectionChanged;
            ToDoTask.CompletedTasksCountChanged += ToDoTask_CompletedTasksCountChanged;
            enableLiveTileToggle.IsOn = TileService.getServiceAvailablilty();

        }

        

        private void decideToShowAds()
        {
            bool userRemovedAds = adverts.checkIfUserRemovedAds();
            if (userRemovedAds == false)
            {
                showAds();
            }
            else
            {
                removeAds();
            }
        }

        private void removeAds()
        {
           
            mainGrid.Children.Remove(adStackPanel);
        }

        private void showAds()
        {

#if DEBUG
            advertControl.ErrorOccurred += advertControl_ErrorOccurred;
            advertControl.ApplicationId = "3f83fe91-d6be-434d-a0ae-7351c5a997f1";
            advertControl.AdUnitId = "test";



#endif

#if TRACE && !DEBUG
            advertControl.ApplicationId = adverts.appID;
            advertControl.AdUnitId = adverts.adUnitID;
#endif

        }

        private void advertControl_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            Debug.WriteLine(e.ErrorCode + ": " + e.ErrorMessage);
        }

        private void ListOfTasks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TileService.UpdateLiveTile(ToDoTask.listOfTasks);
        }

        private async void ToDoTask_CompletedTasksCountChanged(object sender, EventArgs e)
        {
            int numOfCompletedTasks = ToDoTask.CompletedTasks;
            ScoreTextBlock.Text = numOfCompletedTasks.ToString();
            RoamingSync.UpdateScore(numOfCompletedTasks);




            if (numOfCompletedTasks % 10 == 0 && ToDoTask.CompletedTasks != 0)
            {
                await animateScoreTextBlock();

            }

            TileService.UpdateLiveTile(localListOfTasks);
        }



        private async Task animateScoreTextBlock()
        {
            await ScoreTextBlock.Scale(1.5f, 1.5f, (float)ScoreTextBlock.ActualWidth / 2, (float)ScoreTextBlock.ActualHeight / 2, 300).StartAsync();
            await ScoreTextBlock.Scale(1, 1, (float)ScoreTextBlock.ActualWidth / 2, (float)ScoreTextBlock.ActualHeight / 2, 300).StartAsync();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Haptics.ApplyAddTaskButtonPressHaptics();
            Frame.ContentTransitions = new TransitionCollection();
            var navThemeTransition = new NavigationThemeTransition
            {
                DefaultNavigationTransitionInfo = new EntranceNavigationTransitionInfo()
            };
            Frame.ContentTransitions.Add(navThemeTransition);
            App.NavService.Navigate(typeof(CreateTaskPage));

        }



        private void CurrentPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != null)
            {
                var widthToSend = e.NewSize.Width;
                PageStuff.OnPageSizeChanged(widthToSend);
            }
        }

        private void CurrentPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus(FocusState.Pointer);
        }



        private async void requestStartupButton_Click(object sender, RoutedEventArgs e)
        {
            StartupTask startupTask = await StartupTask.GetAsync("yataStartup");
            MessageDialog dialog = new MessageDialog("Yata! is already running at startup.", "Startup task has been enabled already.");
            switch (startupTask.State)
            {
                case StartupTaskState.Disabled:
                    // Task is disabled but can be enabled.
                    StartupTaskState newState = await startupTask.RequestEnableAsync();
                    Debug.WriteLine("Request to enable startup, result = {0}", newState);
                    break;
                case StartupTaskState.DisabledByUser:
                    // Task is disabled and user must enable it manually.
                    dialog.Content = "In order for this app to run at start up, you need to run task manager" +
                        ", select the \"Start-up\" tab " +
                        "then enable \"Yata!\" from there!";
                    dialog.Title = "You need to this yourself 😞";
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.DisabledByPolicy:
                    dialog.Content =
                       "Please contact your system administrator for more information";
                    dialog.Title = "Disabled by group policy";
                    await dialog.ShowAsync();
                    break;
                case StartupTaskState.Enabled:
                    await dialog.ShowAsync();
                    break;
            }
        }

        private void enableLiveTileToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggledSwitch = (ToggleSwitch)sender;
            bool newState = toggledSwitch.IsOn;
            new TileService().ChangeTileServiceAvailability(newState);
        }

        private async void RemoveAdsButton_Click(object sender, RoutedEventArgs e)
        {
            bool shouldRemoveAds = await adverts.tryRemovingAds();
            if (shouldRemoveAds)
            {
                adverts.setRemoveAdsSettingToTrue();
                removeAds();
                
            }
        }
    }
}
