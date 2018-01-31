using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Core;
using YATA.Core.Audio;
using YATA.Model;
using YATA.Phone;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YATA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTaskPage : Page
    {
        private string title = "Creating a New Task...";
        InputPane onScreenInput = InputPane.GetForCurrentView();

        public CreateTaskPage()
        {
            this.InitializeComponent();
            onScreenInput.Showing += CreateTaskPage_Showing;
            onScreenInput.Hiding += CreateTaskPage_Hiding;
        }

        private void CreateTaskPage_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            KeyboardCommandBar.Visibility = Visibility.Collapsed;  
        }

        private void CreateTaskPage_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            KeyboardCommandBar.Visibility = Visibility.Visible;
        }

        private void myGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            taskDetailsTextBox.Focus(FocusState.Pointer);
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            SoundFX.PlayFinishCreatingTaskSound();
            Haptics.ApplyCreateTaskButtonPressHaptics();
           ToDoTask.CreateNote(taskDetailsTextBox.Text);
            PageStuff.navigating = true;
            Frame.Navigate(typeof(MainPage));
        }

        private void taskDetailsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var changedTextBlock = sender as TextBox;
            if (changedTextBlock != null)
            {
                if (changedTextBlock.Text.TrimStart() == String.Empty)
                {
                    CreateTaskButton.IsEnabled = false;
                }

                else
                {
                    CreateTaskButton.IsEnabled = true;
                }
            }
            
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Haptics.ApplyEraseCompletedStampHaptics();
            onScreenInput.TryHide();
        }
    }
}
