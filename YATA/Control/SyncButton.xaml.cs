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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YATA.Enums;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace YATA.Control
{
    public sealed partial class SyncButton : UserControl
    {
        const string _syncCompleteGlyph = "";
        const string _syncFailedGlyph = "";
        readonly SolidColorBrush GreenBrush = new SolidColorBrush(Colors.Green);
        readonly SolidColorBrush RedBrush = new SolidColorBrush(Colors.Red);

        public event EventHandler SyncButtonClicked;
        public bool nowSyncing = false;
        public bool dialogOpened = false;

        public SyncButton()
        {
            this.InitializeComponent();
            SyncDialog.CloseDialogButtonClicked += SyncDialog_CloseDialogButtonClicked;
        }

        private void SyncDialog_CloseDialogButtonClicked(object sender, EventArgs e)
        {
            dialogOpened = false;
        }

        private void theSyncButton_Click(object sender, RoutedEventArgs e)
        {
            // Show synicng menu
            if (!dialogOpened)
            {
                dialogOpened = true;
                SyncButtonClicked?.Invoke(this, EventArgs.Empty);
            }

        }

        public async Task changeStatusIcon(SyncStatus syncStatus)
        {
            if (syncStatus == SyncStatus.Syncing)
            {
                await animateSyncingIcon();
            }
            else
            {
                string glyphToUse = syncStatus == SyncStatus.Failed ? _syncFailedGlyph : _syncCompleteGlyph;
                statusIcon.Foreground = glyphToUse.Equals(_syncFailedGlyph) ? RedBrush : GreenBrush;
                await PlayIconChangeAnimation();

            }

        }

        private async Task PlayIconChangeAnimation()
        {
            var centreX = (float)(statusIcon.ActualWidth / 2);
            var centreY = (float)(statusIcon.ActualHeight / 2);
            await statusIcon.Scale(0.2f, 0.2f, centreX, centreY, 0).StartAsync();
            await statusIcon.Fade(1, 200).Scale(1, 1, centreX, centreY, 500).StartAsync();
        }

        private async Task animateSyncingIcon()
        {
            nowSyncing = true;
            var centerX = (float)syncIcon.ActualWidth / 2;
            var centerY = (float)syncIcon.ActualHeight / 2;
            const int maxRotation = 360;
            double rotationsToPerform = 10;
            double rotationsPerSecond = (double)(1 / rotationsToPerform);
            var duration = (float)(rotationsPerSecond / maxRotation);
            int rotation = 1;
            while (nowSyncing)
            {
                if (rotation == maxRotation)
                {
                    await syncIcon.Rotate(0, centerX, centerY, 0).StartAsync();
                    rotation = 0;
                }
                await syncIcon.Rotate(rotation, centerX, centerY, duration).StartAsync();
                rotation += (int)rotationsToPerform;

            }
        }
    }
}
