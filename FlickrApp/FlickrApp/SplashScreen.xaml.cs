using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FlickrApp.FlickrApi;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FlickrApp
{
    public partial class SplashScreen : PhoneApplicationPage
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            String uri = FlickrManager.IsLoggedIn() ? "/HomePage.xaml" : "/MainPage.xaml";
            Thread.Sleep(3000);
            this.NavigationService.Navigate(new Uri(uri, UriKind.Relative));
        }
    }
}