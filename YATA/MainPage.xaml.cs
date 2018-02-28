using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public ObservableCollection<ToDoTask> localListOfTasks;
        private string title = "Tasks";
        public MainPage()
        {
            this.InitializeComponent();
            localListOfTasks = ToDoTask.listOfTasks;
            ScoreTextBlock.Text = ToDoTask.CompletedTasks.ToString();
            ToDoTask.CompletedTasksCountChanged += ToDoTask_CompletedTasksCountChanged;
            SyncButton.SyncButtonClicked += SyncButton_SyncButtonClicked;
            SyncDialog.CloseDialogButtonClicked += SyncDialog_CloseDialogButtonClicked;

        }

        private async void SyncDialog_CloseDialogButtonClicked(object sender, EventArgs e)
        {
            SyncDialog dialogToRemove = (SyncDialog)sender;

            if (dialogToRemove != null)
            {
                await MaskGrid.Fade(0).StartAsync();
                listGrid.Children.Remove(dialogToRemove);
                MaskGrid.Visibility = Visibility.Collapsed;
            }
            
        }

        private async void SyncButton_SyncButtonClicked(object sender, EventArgs e)
        {
            var dialogToShow = new SyncDialog
            {
                Width = 300,
                Height = 400,
                Opacity = 0,
                
            };

            

            var mainGrid = (Grid)this.Content;
            listGrid.Children.Add(dialogToShow);
            Canvas.SetZIndex(dialogToShow, 30);
            MaskGrid.Visibility = Visibility.Visible;

            await Task.WhenAll(dialogToShow.Fade(1).StartAsync(), MaskGrid.Fade(0.5f).StartAsync());
            


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
            Frame.Navigate(typeof(CreateTaskPage));

        }

        private void tasksListView_ItemClick(object sender, ItemClickEventArgs e)
        {

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

        private void AskForSyncButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OnboardingPage));
        }
    }
}
