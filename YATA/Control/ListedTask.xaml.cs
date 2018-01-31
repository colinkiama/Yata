﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Core;
using YATA.Model;
using YATA.Phone;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YATA.Control
{
    public sealed partial class ListedTask : UserControl
    {
        public ToDoTask TaskItem { get { return DataContext as ToDoTask; } }

        private bool isDataContextNull = true;
        public ListedTask()
        {
            this.InitializeComponent();
            this.DataContextChanged += ListedTask_DataContextChanged;
            PageStuff.pageSizeChanged += PageStuff_pageSizeChanged;
        }

        private void ListedTask_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Bindings.Update();
            if (isDataContextNull == true)
            {
                if (TaskItem != null)
                {
                    if (TaskItem.isCompleted)
                    {
                        this.TaskTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxDisabledForegroundThemeBrush"];
                        TaskCompleteTag.Visibility = Visibility.Visible;
                        CompletedStampToggleButton.IsChecked = true;
                    }
                    TaskItem.isCompletedChanged += TaskItem_isCompletedChanged;
                    isDataContextNull = false;
                }
            }

        }

        private void PageStuff_pageSizeChanged(object sender, EventArgs e)
        {

            mainPanel.Width = PageStuff.currentWidth - (mainPanel.Padding.Left + mainPanel.Padding.Right);
        }

        private void TaskItem_isCompletedChanged(object sender, EventArgs e)
        {
            if (TaskItem.isCompleted)
            {
                this.TaskTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxDisabledForegroundThemeBrush"];
                TaskCompleteTag.Visibility = Visibility.Visible;
            }
            else
            {
                this.TaskTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources["DefaultTextForegroundThemeBrush"];
                TaskCompleteTag.Visibility = Visibility.Collapsed;
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (isDataContextNull == true)
            {
                if (TaskItem != null)
                {
                    if (TaskItem.isCompleted)
                    {
                        this.TaskTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxDisabledForegroundThemeBrush"];
                        TaskCompleteTag.Visibility = Visibility.Visible;
                        CompletedStampToggleButton.IsChecked = true;
                    }
                    TaskItem.isCompletedChanged += TaskItem_isCompletedChanged;

                }

            }
            mainPanel.Width = PageStuff.currentWidth - (mainPanel.Padding.Left + mainPanel.Padding.Right);
        }

        private async void CompletedStampToggleButton_Click(object sender, RoutedEventArgs e)
        {
           await this.TaskItem.changeIsCompletedState();
        }

        private void CompletedStampToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Haptics.ApplyCompletedStampHaptics();
        }

        private void CompletedStampToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Haptics.ApplyEraseCompletedStampHaptics();
        }
    }
}