using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Model;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YATA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTaskPage : Page
    {
        private string title = "Creating a New Task...";
        

        public CreateTaskPage()
        {
            this.InitializeComponent();
        }

        private void myGrid_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            taskDetailsTextBox.Focus(FocusState.Pointer);
        }

        private void CreateTaskButton_Click(object sender, RoutedEventArgs e)
        {
           Note.CreateNote(taskDetailsTextBox.Text);
            Frame.Navigate(typeof(MainPage));
        }
    }
}
