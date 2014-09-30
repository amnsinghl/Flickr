using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            Photo p = (Photo) PhoneApplicationService.Current.State["photo"];
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
    }
}