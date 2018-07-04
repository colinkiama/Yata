using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using YATA.Core.Audio;
using YATA.Core.Syncing;
using YATA.Model;
using YATA.Phone;
using YATA.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace YATA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary> 
    public sealed partial class MainPage : Page
    {
        public  ObservableCollection<ToDoTask> localListOfTasks = ToDoTask.listOfTasks;
        private string title = "Tasks";
        public MainPage()
        {
            this.InitializeComponent();
            ScoreTextBlock.Text = ToDoTask.CompletedTasks.ToString();
            ToDoTask.CompletedTasksCountChanged += ToDoTask_CompletedTasksCountChanged;
            ToDoTask.listOfTasks.CollectionChanged += ListOfTasks_CollectionChanged;
            enableLiveTileToggle.IsOn = TileService.getServiceAvailablilty();
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
            PageStuff.navigating = false;
            this.Focus(FocusState.Pointer);
        }



        private void enableLiveTileToggle_Toggled(object sender, RoutedEventArgs e)
        {
            var toggledSwitch = (ToggleSwitch)sender;
            bool newState = toggledSwitch.IsOn;
            new TileService().ChangeTileServiceAvailability(newState);
        }
    }
}
