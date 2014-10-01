using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FlickrApp.FlickrApi;
using FlickrNet;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FlickrApp
{
    public partial class FavouritesPage : PhoneApplicationPage
    {
        public FavouritesPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string photoid = "";

            if (!NavigationContext.QueryString.TryGetValue("photoid", out photoid))
            {
                MessageBox.Show("Error");
            }

            Flickr f = FlickrManager.GetAuthInstance();
            SystemTray.ProgressIndicator = new ProgressIndicator()
            {
                IsIndeterminate = true,
                IsVisible = true
            };
            f.PhotosGetFavoritesAsync(photoid,100000,1, r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Error Occured in fetching Comments");
                    return;
                }
                PhotoFavoriteCollection photoFavoriteCollection = r.Result;
                PhotoFavorite ff;
                
                Dispatcher.BeginInvoke(() =>
                {
                    FavouritesList.ItemsSource = photoFavoriteCollection;
                    SystemTray.ProgressIndicator.IsVisible = false;
                });


            });
        }
    }
}