using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Controls;
using YATA.Fluent;

namespace YATA.Services
{
    public class Navigation
    {
        public static Navigation Instance { get; protected set; }
        private Frame frame;

        public Navigation(ref Frame frame)
        {
            if (Instance != null)
            {
                throw new Exception("Only one navigation service can exist.");
            }

            //assigns the instance of the class and frame
            Instance = this;
            this.frame = frame;
        }

        public void Navigate(Type pageType, object parameter = null)
        {
            switch (pageType.Name)
            {
                case "MainPage":
                    NavigateToMainPage(pageType, parameter);
                    break;
                case "CreateTaskPage":
                    NavigateToCreateTask(pageType, parameter);
                    break;
            }
        }

        private void NavigateToCreateTask(Type pageType, object parameter)
        {
            if (isAtLeastOnFallCreatorsUpdate())
            {
                frame.Navigate(typeof(FluentCreateTaskPage),parameter);
            }

            else
            {
                frame.Navigate(typeof(CreateTaskPage), parameter);
            }
        }

        private void NavigateToMainPage(Type pageType, object parameter)
        {
            if (isAtLeastOnFallCreatorsUpdate())
            {
                frame.Navigate(typeof(FluentMainPage),parameter);
            }
            else
            {
                frame.Navigate(typeof(MainPage),parameter);
            }
        }

        private bool isAtLeastOnFallCreatorsUpdate()
        {
            return ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5);
        }
    }
}
