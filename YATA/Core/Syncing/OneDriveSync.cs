using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YATA.Core.Syncing
{
    public static class OneDriveSync
    { 

        public static int? GetLastSyncDate()
        {
           return (int?)GetLocalSettings().Values["lastSyncDate"];
        }

        public static void SetLastSyncDate(int dateToSave)
        {
            GetLocalSettings().Values["lastSyncDate"] = dateToSave;
        }

        private static ApplicationDataContainer GetLocalSettings()
        {
            return ApplicationData.Current.LocalSettings; 
        }
    }
}
