using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Telerik.Windows.Controls;

namespace FlickrApp
{
    public partial class HomePage : PhoneApplicationPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void PivotLoaded(object sender, RoutedEventArgs e)
        {
            PivotSelectionChanged(null, null);
        }

        private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (MainPivot.SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = false;
                    break;
                case 1:
                    ApplicationBar.IsVisible = true;
                    break;
                default:
                    ApplicationBar.IsVisible = false;
                    break;
            }
        }

        private void CameraButtonClicked(object sender, EventArgs e)
        {
//            Task<MessageBoxClosedEventArgs> t = RadMessageBox.ShowAsync(new string[] {"Camera", "Gallery"}, "Choose From");
//            t.ContinueWith(task =>
//            {
//                if (task.Result.ButtonIndex == 0)
//                {
//                    CameraCapture();
//                }
//                else
//                {
//                    GalleryCapture();
//                }
//            });
            
            RadMessageBox.Show(new string[] { "Camera", "Gallery" }, "Choose from", closedHandler: (args) =>
            {
                DialogResult result = args.Result;
                int buttonIndex = args.ButtonIndex;
                Button clickedButton = args.ClickedButton;

                if (buttonIndex == 0)
                {
                    CameraCapture();
                }
                else
                {
                    GalleryCapture();
                }
            });
        }

        private void CameraCapture()
        {
            CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += (sender, result) =>
            {
                if (result.TaskResult == TaskResult.OK)
                {
//                    ShareMediaTask shareMediaTask = new ShareMediaTask();
//                    shareMediaTask.FilePath = result.OriginalFileName;
//                    shareMediaTask.Show();
                    PhoneApplicationService.Current.State["upload_photo"] = result.ChosenPhoto;
                    this.NavigationService.Navigate(new Uri("/ImageUpload.xaml", UriKind.Relative));
                }
            };
            cameraCaptureTask.Show();
        }

        private void GalleryCapture()
        {
            PhotoChooserTask photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += (sender, result) =>
            {
                if (result.TaskResult == TaskResult.OK)
                {
//                    ShareMediaTask shareMediaTask = new ShareMediaTask();
//                    shareMediaTask.FilePath = result.OriginalFileName;
//                    shareMediaTask.Show();
                    PhoneApplicationService.Current.State["upload_photo"] = result.ChosenPhoto; 
                    this.NavigationService.Navigate(new Uri("/ImageUpload.xaml", UriKind.Relative));
                }
            };

            photoChooserTask.Show();
        }
    }
}