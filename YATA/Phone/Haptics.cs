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

        public static void ApplyCompletedStampHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(10));
        }


        public static void ApplyEraseCompletedStampHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(5));
        }

        public static void ApplyAddTaskButtonPressHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(20));
        }

        public async static void ApplyCreateTaskButtonPressHaptics()
        {
            VibrateDevice(TimeSpan.FromMilliseconds(20));
            await Task.Delay(20);
            VibrateDevice(TimeSpan.FromMilliseconds(20));
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

