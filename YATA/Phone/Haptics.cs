using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YATA.Core;
using YATA.Enums;
using Windows.Phone.Devices.Notification;
using System.Diagnostics;

namespace YATA.Phone
{
    public class Haptics
    {

        public static void CompletedStampHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(150));
        }


        public static void CancelCompletedStampHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(80));
        }

        static void VibrateDevice(TimeSpan VibrationTime)
        {
            if (DeviceDetection.DetectDeviceType() == DeviceType.Phone)
            {
                var vibrator = VibrationDevice.GetDefault();
                vibrator.Vibrate(VibrationTime);
            }
        }
    }
}

