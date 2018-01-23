using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using YATA.Enums;

namespace YATA.Core
{
    public static class DeviceDetection
    {
        public static DeviceType DetectDeviceType()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusbar = StatusBar.GetForCurrentView();
                return DeviceType.Phone;

            } 

            else
            {
                return DeviceType.Desktop;
            }
        }
    }
}
