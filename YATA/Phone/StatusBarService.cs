using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace YATA.Phone
{
    public class StatusBarService
    {
        
        StatusBar sBar;
        public StatusBarService()
        {
           
            sBar = StatusBar.GetForCurrentView();
            sBar.BackgroundColor =  getAccentColor();
            sBar.BackgroundOpacity = 1;
            sBar.ForegroundColor = Colors.White;
        }

       public void setAccentColor()
        {
            sBar.BackgroundColor = getAccentColor();
        }

        private Color getAccentColor()
        {
            return (Color)Application.Current.Resources["SystemAccentColor"];
        }

        
    }
}
