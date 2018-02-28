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
                StorageFile exportedTasks = await ToDoTask.ExportTasks();
            }
           
           return canLogin;
        }

        public async Task<bool> Sync()
        {
            bool isSynced = false;
            
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
