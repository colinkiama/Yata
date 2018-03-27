using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YATA.Services
{
    public partial class TileService
    {
        const string serviceSettingsKey = "tileServiceEnabled";
        public void ChangeTileServiceAvailability()
        {
            bool tileServiceEnabled = CheckIfServiceAvailable();
            if (tileServiceEnabled)
            {
                setServiceAvailability(!tileServiceEnabled);
            }
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
    }
}
