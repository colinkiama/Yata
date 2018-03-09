using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace YATA.Core
{
    public static class Settings
    {
        public static bool? GetWhetherOnBoardingPageHasBeenViewed()
        {
            return (bool?)GetLocalSettings().Values["onboardingPageViewed"];
        }

        public static void SetOnBoardingPageAsViewed()
        {
            GetLocalSettings().Values["onboardingPageViewed"] = true;
        }

        private static ApplicationDataContainer GetLocalSettings()
        {
            return ApplicationData.Current.LocalSettings;
        }

    }
}
