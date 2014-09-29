using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FlickrNet;
using Microsoft.Phone.Controls;

namespace FlickrApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String uri = FlickrManager.IsLoggedIn() ? "/SearchPage.xaml" : "/LoginPage.xaml";
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}
