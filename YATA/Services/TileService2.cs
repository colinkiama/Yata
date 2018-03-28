using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using YATA.Model;

namespace YATA.Services
{
    public partial class TileService
    {
        const string serviceSettingsKey = "tileServiceEnabled";
        public void ChangeTileServiceAvailability()
        {
            
        }


        internal void ChangeTileServiceAvailability(bool newState)
        {

            setServiceAvailability(newState);
            UpdateLiveTile(ToDoTask.listOfTasks);
        }

        public static bool getServiceAvailablilty()
        {
            var localSettings = ApplicationData.Current.LocalSettings.Values;
            return (bool)localSettings[serviceSettingsKey];
        }


        private void setServiceAvailability(bool availability)
        {
            var localSettings = ApplicationData.Current.LocalSettings.Values;
            localSettings[serviceSettingsKey] = availability;
        }

        private bool CheckIfServiceAvailable()
        {
            var localSettings = ApplicationData.Current.LocalSettings.Values;
            bool isTileServiceEnabled = (bool)localSettings[serviceSettingsKey];
            return isTileServiceEnabled;
        }

        public static void turnOnTileServiceForFirstTime()
        {
            var localSettings = ApplicationData.Current.LocalSettings.Values;
            if (localSettings[serviceSettingsKey] == null)
            {
                localSettings[serviceSettingsKey] = true;
            } 
        }
    }
}
