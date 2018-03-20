using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace YATA.Services
{
    public class FluentService
    {
        public bool tryMakingTitleBarFluent()
        {
            bool isFluentTitleBar = false;
            if (isAtLeastOnFallCreatorsUpdate())
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                titleBar.ButtonBackgroundColor = Colors.Transparent;
                titleBar.ButtonForegroundColor = Colors.White;
                titleBar.ButtonHoverBackgroundColor = Colors.LightSlateGray;
                titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                titleBar.ButtonInactiveForegroundColor = Colors.LightGray;
                titleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                isFluentTitleBar = true;
            }
            return isFluentTitleBar;
        }

        public bool isAtLeastOnFallCreatorsUpdate()
        {
            return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5);
        }
    }
}
