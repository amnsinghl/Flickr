using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FlickrApp.FlickrApi;
using FlickrNet;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FlickrApp
{
    public partial class ImageDisplay : PhoneApplicationPage
    {
        private string photoid;
        public ImageDisplay()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Photo p = (Photo)PhoneApplicationService.Current.State["photo"];
            photoid = p.PhotoId;
            Uri uri = new Uri(p.LargeUrl, UriKind.Absolute);
            ImageSource imgSource = new BitmapImage(uri);
            MainImage.Source = imgSource;

            ImageTitle.Text = p.Title;
            FavAndCommentCount.Text = p.CountFaves + " Faves, " + p.CountComments + " Comments";
        }

        private void CommentButton_Clicked(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/CommentPage.xaml?photoid=" + photoid, UriKind.Relative));
        }

        private void FavouriteButton_Clicked(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/FavouritesPage.xaml?photoid=" + photoid, UriKind.Relative));
        }

        private void ShowProgressIndicator()
        {
            Dispatcher.BeginInvoke(() =>
            {
                SystemTray.ProgressIndicator = new ProgressIndicator { IsIndeterminate = true, IsVisible = true };
            });
        }

        private void HideProgressIndicator()
        {
            Dispatcher.BeginInvoke(() =>
            {
                SystemTray.ProgressIndicator.IsVisible = false;
            });
        }

        private void LikeButton_Clicked(object sender, EventArgs e)
        {
            if (!FlickrManager.IsLoggedIn())
            {
                MessageBox.Show("You need to login to like the image");
                this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                return;
            }

            ShowProgressIndicator();
            
            Flickr flickr = FlickrManager.GetAuthInstance();
            flickr.FavoritesGetContextAsync(photoid, FlickrManager.UserId, r =>
            {
                if (r.Result == null)
                {
                    if (r.ErrorMessage.Contains("not a favorite"))
                    {
                        flickr.FavoritesAddAsync(photoid, result =>
                        {
                            HideProgressIndicator();;
                            if (result.HasError)
                            {
                                MessageBox.Show("Error occured");
                            }
                            else
                            {
                                MessageBox.Show("Image liked");
                            }
                        });
                    }
                    else
                    {
                        SystemTray.ProgressIndicator.IsVisible = false;
                        MessageBox.Show("Error Occured " + r.ErrorMessage + "  " + r.HasError);
                        return;
                    }
                }
                else
                {
                    flickr.FavoritesRemoveAsync(photoid, result =>
                    {
                        SystemTray.ProgressIndicator.IsVisible = false;
                        if (result.HasError)
                        {
                            MessageBox.Show("Error occured");
                        }
                        else
                        {
                            MessageBox.Show("Image Unliked");
                        }
                    });
                }

            });
        }
    }
}