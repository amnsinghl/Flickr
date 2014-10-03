using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Xna.Framework.Media;

namespace FlickrApp
{
    public partial class ImageDisplay : PhoneApplicationPage
    {
        private string photoid;
        private Uri photoUri;
        private BitmapImage imgSource;
        public ImageDisplay()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Photo p = (Photo)PhoneApplicationService.Current.State["photo"];
            photoid = p.PhotoId;
            photoUri = new Uri(p.LargeUrl, UriKind.Absolute);
            imgSource = new BitmapImage(photoUri);
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

        private bool SaveImageToPhotoHub(WriteableBitmap bmp)
        {

            using (var mediaLibrary = new MediaLibrary())
            {
                using (var stream = new MemoryStream())
                {
                    var fileName = string.Format("Gs{0}.jpg", Guid.NewGuid());
                    bmp.SaveJpeg(stream, bmp.PixelWidth , bmp.PixelHeight , 0, 100);
                    stream.Seek(0, SeekOrigin.Begin);
                    var picture = mediaLibrary.SavePicture(fileName, stream);
                    if (picture.Name.Contains(fileName)) return true;
                }
            }
            return false;
        }

        private void DownloadImage_Clicked(object sender, EventArgs e)
        {
            ShowProgressIndicator();
            WriteableBitmap bmp = new WriteableBitmap(imgSource);
            if (SaveImageToPhotoHub(bmp))
            {
                MessageBox.Show("Image Saved", "Information", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Error : Image Not Saved", "Information", MessageBoxButton.OK);
            }
            HideProgressIndicator();
        }
    }
}