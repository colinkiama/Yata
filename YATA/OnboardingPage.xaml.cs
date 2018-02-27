using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace YATA
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OnboardingPage : Page
    {
        public OnboardingPage()
        {
            this.InitializeComponent();
        }

        private async void NoButton_Click(object sender, RoutedEventArgs e)
        {
            float yToAnimateBy = (float)(this.ActualHeight / 1.5);
            var animation = this.Offset(0, yToAnimateBy).Fade(0);
            animation.SetDurationForAll(400);
            await animation.StartAsync();
            Frame.Navigate(typeof(MainPage), "No");
        }

        private async void YesButton_Click(object sender, RoutedEventArgs e)
        {
            // Do the Synchronisation!
            bool canSync = NetworkInterface.GetIsNetworkAvailable();

            if (canSync)
            {
                await PrepareUIForSyncing();
                await StartSyncing();
            }
        }

        private async Task StartSyncing()
        {
            syncRing.IsActive = true;
            var syncService = new CloudSyncService();
            
            if (await syncService.Begin())
            {
                Debug.WriteLine("Yes We Can");
                ChangePageTitleText();

                
               
            }
            else
            {
                Debug.WriteLine("No we can't!");
            }
        }

        private void ChangePageTitleText()
        {
            PageTitleTextBlock.Inlines.Clear();
            PageTitleTextBlock.Inlines.Add(new Run { Text = "Syncing " });
            Bold yourInBold = new Bold();
            Bold secondYourInBold = new Bold();
            yourInBold.Inlines.Add(new Run { Text = "your " });
            secondYourInBold.Inlines.Add(new Run { Text = "your " });
            PageTitleTextBlock.Inlines.Add(yourInBold);
            PageTitleTextBlock.Inlines.Add(new Run { Text = "tasks between " });
            PageTitleTextBlock.Inlines.Add(secondYourInBold);
            PageTitleTextBlock.Inlines.Add(new Run { Text = "devices" });

        }

        private async Task PrepareUIForSyncing()
        {
            await decisionStackPanel.Fade(0).StartAsync();
            decisionStackPanel.Visibility = Visibility.Collapsed;
            syncStatusTextBlock.Text = "Starting...";
            await syncStatusStackPanel.Fade(0, 0).StartAsync();
            syncStatusStackPanel.Visibility = Visibility.Visible;
            await syncStatusStackPanel.Fade(1).StartAsync();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await this.Fade(0, duration: 0).StartAsync();
            await this.Fade(1, duration: 400, delay: 200).StartAsync();

        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
