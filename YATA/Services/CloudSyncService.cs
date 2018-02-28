using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.OneDrive;
using Windows.Storage;
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
            try
            {
                bool isDataSaved = await new FileIOService().saveData();
                Debug.WriteLine("Local Data Saved = " + isDataSaved);
            }
            catch (Exception ex)
            {
                syncFailed?.Invoke(this, EventArgs.Empty);
                return false;
            }
            
            StorageFile exportedTasks = await new FileIOService().GiveBackToDoTaskFile();
            using (var stream = await exportedTasks.OpenReadAsync())
            {
                try
                {

                    await rootFolder.UploadFileAsync(FileIOService.saveFileName, stream, CreationCollisionOption.ReplaceExisting, 320 * 2048);
                    //await stream.FlushAsync();
                    isSynced = true;
                    syncCompleted?.Invoke(this, EventArgs.Empty);
                    Debug.WriteLine("Sync Complete!");
                }
                catch (Exception ex)
                {
                    syncFailed?.Invoke(this, EventArgs.Empty);
                    Debug.WriteLine(ex);

                }

            }

            return isSynced;
        }

        public async Task<bool> Load()
        {
            bool isLoaded = false;
            return isLoaded;
        }

        public async Task<bool> Save()
        {
            bool isSaved = false;
            return isSaved;
        }
    }
}
