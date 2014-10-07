using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FlickrApp
{
    public partial class ImageUpload : PhoneApplicationPage
    {
        public ImageUpload()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Stream photoStream = (Stream) PhoneApplicationService.Current.State["upload_photo"];
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(photoStream);
            MainImage.Source = bitmapImage;
        }
    }
}