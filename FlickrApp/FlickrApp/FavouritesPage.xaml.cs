using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FlickrApp.FlickrApi;
using FlickrApp.ViewModels;
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

            FlickrPhotoFavourite photoFavourite = new FlickrPhotoFavourite();;
            SystemTray.ProgressIndicator = new ProgressIndicator()
            {
                IsIndeterminate = true,
                IsVisible = true
            };
            photoFavourite.fetch(photoid,100000, r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Error Occured in fetching Comments");
                    return;
                }
                PhotoFavouriteCollection photoFavoriteCollection = r.Result;
                
                Dispatcher.BeginInvoke(() =>
                {
                    FavouritesList.ItemsSource = FavouriteViewModel.PhotoFavouriteCollectionToObservableFavouriteViewModel(photoFavoriteCollection);
                    SystemTray.ProgressIndicator.IsVisible = false;
                });


            });
        }
    }
}