using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace FlickrApp
{
    class AppManager
    {
        public static void ClearBackStack(NavigationService navigationService)
        {
            while (navigationService.CanGoBack)
            {
                navigationService.RemoveBackEntry();
            }
        }
    }
}
