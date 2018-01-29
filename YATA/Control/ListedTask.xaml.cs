using System;
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
using YATA.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YATA.Control
{
    public sealed partial class ListedTask : UserControl
    {
        public ToDoTask TaskItem { get { return DataContext as ToDoTask; } }
        public ListedTask()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
            
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
            if (TaskItem.isCompleted)
            {
                this.TaskTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources["TextBoxDisabledForegroundThemeBrush"];
                TaskCompleteTag.Visibility = Visibility.Visible;
                CompletedStampToggleButton.IsChecked = true;
            }
            TaskItem.isCompletedChanged += TaskItem_isCompletedChanged;
        }

        private void CompletedStampToggleButton_Click(object sender, RoutedEventArgs e)
        {
            this.TaskItem.changeIsCompletedState();
        }
    }
}
