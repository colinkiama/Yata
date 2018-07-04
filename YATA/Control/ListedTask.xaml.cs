using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Core;
using YATA.Core.Audio;
using YATA.Model;
using YATA.Phone;
using YATA.Services;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YATA.Control
{
    public sealed partial class ListedTask : UserControl
    {
        public ToDoTask TaskItem { get { return DataContext as ToDoTask; } }
        private SolidColorBrush transparentColor = new SolidColorBrush(Colors.Transparent);
        public ListedTask()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            PageStuff.pageSizeChanged += PageStuff_pageSizeChanged;
        }

        private void PageStuff_pageSizeChanged(object sender, EventArgs e)
        {
            mainPanel.Width = PageStuff.currentWidth - (mainPanel.Padding.Left + mainPanel.Padding.Right);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainPanel.Width = PageStuff.currentWidth - (mainPanel.Padding.Left + mainPanel.Padding.Right);
        }

        private void CompletedStampToggleButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateUIAutomation();
        }


        public void UpdateUIAutomation()
        {
            string taskItemValue = TaskItem.IsCompleted ? $"{ TaskItem.Content} is completed" : TaskItem.Content;
            string buttonNameValue = TaskItem.Content + " " + "completed toggle checked" + "is" + TaskItem.IsCompleted + " ";
            buttonNameValue += "Click here to toggle task completion";

            AutomationProperties.SetName(this, taskItemValue);
            AutomationProperties.SetName(CompletedStampToggleButton, buttonNameValue);
        }

        private async void CompletedStampToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            await updateListedTaskFromCompletionResult();
            if (PageStuff.navigating == false)
            {
                Haptics.ApplyCompletedStampHaptics();
                SoundFX.PlayCompletedSound();
            }

        }

        private async void CompletedStampToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            await updateListedTaskFromCompletionResult();
            Haptics.ApplyEraseCompletedStampHaptics();
        }

        private void FlyoutDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            this.TaskItem.DeleteNote();
        }

        private void updateUIOnLoadOrCompletion()
        {
            bool taskCompletionResult = TaskItem.IsCompleted;
            if (taskCompletionResult)
            {
                TaskCompleteTag.Opacity = 1;
                TaskCompleteTag.Visibility = Visibility.Visible;
                CompletedStampToggleButton.IsChecked = true;
            }
        }

        private async Task updateListedTaskFromCompletionResult()
        {
            if (CompletedStampToggleButton.IsChecked == true)
            {
                TaskCompleteTag.Visibility = Visibility.Visible;
                TaskCompleteTag.Opacity = 0;

                await TaskCompleteTag.Offset(0, -10, 0).StartAsync();
                var fadeTaskTitleAnim = this.TaskTextBlock.Fade(0.6f, 200).StartAsync();
                var showTagAnim = TaskCompleteTag.Fade(1, 200).Offset(duration: 200).StartAsync();
                await Task.WhenAll(fadeTaskTitleAnim, showTagAnim);
            }
            else
            {
                var showTitleAnim = this.TaskTextBlock.Fade(1, 200).StartAsync();
                var fadeTagAnim = TaskCompleteTag.Fade(0, 200).Offset(offsetY: -10, duration: 200).StartAsync();
                await Task.WhenAll(showTitleAnim, fadeTagAnim);
                TaskCompleteTag.Visibility = Visibility.Collapsed;
            }
        }

        private void UserControl_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
