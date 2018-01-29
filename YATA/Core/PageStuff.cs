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

        public static void OnPageSizeChanged(Double pageWidth)
        {
            pageSizeChanged?.Invoke(pageWidth, EventArgs.Empty);
        }

    }
}
