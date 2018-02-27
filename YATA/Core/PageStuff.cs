using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATA.Core
{
   public class PageStuff
    {
        public static event EventHandler pageSizeChanged;
        public static double currentWidth = 0;
        public static bool navigating = false;
        public static void OnPageSizeChanged(double pageWidth)
        {
            currentWidth = pageWidth;
            pageSizeChanged?.Invoke(null, EventArgs.Empty);
        }



    }
}
