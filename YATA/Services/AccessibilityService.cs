using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YATA.Services
{
    public class AccessibilityService
    {
        public delegate void LeftHandedEventHandler(object sender, LeftHandedEventArgs args);
        public static event LeftHandedEventHandler LeftHandedEvent;

        public static void toggleLeftHandedEvent(bool toggleState)
        {
            setLeftHandedSettings(toggleState);
           

        }

        protected virtual void OnRaiseLeftHandedEvent(LeftHandedEventArgs e)
        {
      
        }

        private static void setLeftHandedSettings(bool toggleState)
        {
            throw new NotImplementedException();
        }
    }

        public class LeftHandedEventArgs : EventArgs
        {
            public LeftHandedEventArgs(bool isOn)
            {
                isLeftHandedEventEnabled = isOn;
            }
            private bool isLeftHandedEventEnabled;
            public bool isEnabled
            {
                get { return isLeftHandedEventEnabled; }
            }
        }
    
}
