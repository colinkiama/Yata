using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Services.OneDrive;

namespace YATA.Services
{
   public class CloudSyncService
    {
        public void Begin()
        {
            OneDriveService.Instance.Initialize(OneDriveScopes.AppFolder);
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
