using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.OneDrive;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using YATA.Core.Syncing;
using YATA.Model;

namespace YATA.Services
{
    public class CloudSyncService
    {

        OneDriveStorageFolder rootFolder;
        public static bool serviceStarted = false;
        public static event EventHandler syncStarted;
        public static event EventHandler syncFailed;
        public static event EventHandler syncCompleted;
        public async Task<bool> Begin()
        {
            bool canLogin = OneDriveService.Instance.Initialize(OneDriveScopes.AppFolder);
            if (!canLogin)
            {
                canLogin = OneDriveService.Instance.Initialize("00000000482119BC", OneDriveEnums.AccountProviderType.Msa, OneDriveScopes.AppFolder);
            }

            if (!canLogin)
            {
                Debug.WriteLine("Login Failed");
            }

            if (canLogin)
            {
                rootFolder = await OneDriveService.Instance.AppRootFolderAsync();
                serviceStarted = true;

            }

            return canLogin;
        }

        public async Task<bool> Sync()
        {
            syncStarted?.Invoke(this, EventArgs.Empty);
            bool isSynced = false;

            if (syncedOnDevice())
            {
                if (await CheckIfFileIsOnCloud())
                {

                    long lastDateModified = await GetDateModifiedDate();

                    try
                    {
                        bool isDataSaved = await new FileIOService().saveData();
                        Debug.WriteLine("Local Data Saved = " + isDataSaved);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                        syncFailed?.Invoke(this, EventArgs.Empty);
                        return false;
                    }
                    isSynced = await decideToUploadOrDownloadFile(lastDateModified);
                }

            }
            else
            {
                if (await CheckIfFileIsOnCloud())
                {
                    // Cloud Clash!!!
                    var clashDialog = new ContentDialog
                    {
                        Title = "Cloud Clash",
                        Content = "Do you want to continue using the data on your device (Local data) or do you want to use data from the cloud instead?",
                        PrimaryButtonText = "Local",
                        SecondaryButtonText = "Cloud"
                    };

                    clashDialog.PrimaryButtonClick += ClashDialog_PrimaryButtonClick;
                    clashDialog.SecondaryButtonClick += ClashDialog_SecondaryButtonClick;
                    await clashDialog.ShowAsync();
                }
            }
            return isSynced;
        }

        private async void ClashDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //Use cloud data
            await Download();
        }

        private async void ClashDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //Use local data
            await Upload();
        }

        private async Task<bool> decideToUploadOrDownloadFile(long lastDateModified)
        {
            bool isSynced = false;

            bool shouldUpload = CheckIfLocalDateIsNewer(lastDateModified);

            isSynced = shouldUpload ? await Upload() : await Download();

            return isSynced;
        }

        private bool CheckIfLocalDateIsNewer(long lastDateModified)
        {
            bool isLocalDataNewer = false;
            long localSyncDate = (long)OneDriveSync.GetLastSyncDate();
            if (localSyncDate > lastDateModified)
            {
                isLocalDataNewer = true;
            }
            return isLocalDataNewer;
        }

        private bool syncedOnDevice()
        {
            bool hasBeenSyncedBefore = false;
            var lastSyncDate = (long?)OneDriveSync.GetLastSyncDate();
            if (lastSyncDate != null)
            {
                hasBeenSyncedBefore = true;
            }

            return hasBeenSyncedBefore;
        }

        private async Task<long> GetDateModifiedDate()
        {
            OneDriveStorageFile fileToReturn = await rootFolder.GetFileAsync(FileIOService.saveFileName);
            return fileToReturn.DateModified.Value.DateTime.Ticks;
        }

        private async Task<bool> CheckIfFileIsOnCloud()
        {
            bool fileExists = false;
            try
            {
                OneDriveStorageFile fileToReturn = await rootFolder.GetFileAsync(FileIOService.saveFileName);
                fileExists = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Debug.WriteLine("File does not exist in the cloud");
            }

            return fileExists;
        }



        public async Task<bool> Download()
        {
            bool isLoaded = false;
            var taskOnCloud = await rootFolder.GetFileAsync(FileIOService.saveFileName);
           
            

            StorageFile tempLoadedTask = await FileIOService.tempFolder.CreateFileAsync(FileIOService.saveFileName, CreationCollisionOption.ReplaceExisting);
            try
            {


                using (var downloadStream = await taskOnCloud.OpenAsync())
                {
                    byte[] buffer = new byte[downloadStream.Size];
                    var localBuffer = await downloadStream.ReadAsync(buffer.AsBuffer(), (uint)downloadStream.Size, InputStreamOptions.ReadAhead);
                    using (var localStream = await tempLoadedTask.OpenAsync(FileAccessMode.ReadWrite))
                    {
                        await localStream.WriteAsync(localBuffer);
                        await localStream.FlushAsync();

                    }



                }
                isLoaded = await FileIOService.replaceOldTasksWithNewTasks(tempLoadedTask);

                UpdateLocalSyncDate();
                syncCompleted?.Invoke(this, EventArgs.Empty);
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                syncFailed?.Invoke(this, EventArgs.Empty);
            }
            return isLoaded;
        }

        private void UpdateLocalSyncDate()
        {
            var currentDate = DateTime.Now;
            long currentDateInTicks = currentDate.Ticks;
            OneDriveSync.SetLastSyncDate(currentDateInTicks);
        }

        public async Task<bool> Upload()
        {
            bool isSaved = false;
            StorageFile exportedTasks = await new FileIOService().GiveBackToDoTaskFile();
            using (var stream = await exportedTasks.OpenReadAsync())
            {
                try
                {
                    await rootFolder.UploadFileAsync(FileIOService.saveFileName, stream, CreationCollisionOption.ReplaceExisting, 320 * 2048);
                    //await stream.FlushAsync();
                    syncCompleted?.Invoke(this, EventArgs.Empty);
                    Debug.WriteLine("Sync Complete!");
                }
                catch (Exception ex)
                {
                    syncFailed?.Invoke(this, EventArgs.Empty);
                    Debug.WriteLine(ex);

                }

            }
            UpdateLocalSyncDate();
            return isSaved;
        }
    }
}
