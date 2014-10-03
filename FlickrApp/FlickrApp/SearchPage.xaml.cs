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
    public partial class SearchPage : PhoneApplicationPage
    {
        PhotoCollection photos;

        public SearchPage()
        {
            InitializeComponent();
        }

        private void SearchButtonTapped(object sender, EventArgs eventArgs)
        {
            this.Focus();
            String searchTerm = SearchTextBox.Text;
            if (searchTerm.Length == 0)
            {
                MessageBox.Show("Enter Something to search");
                return;
            }

            SystemTray.IsVisible = true;
            SystemTray.ProgressIndicator = new ProgressIndicator();
            SystemTray.ProgressIndicator.IsIndeterminate = true;
            SystemTray.ProgressIndicator.IsVisible = true;
//            Flickr flickr = FlickrManager.GetInstance();
            Flickr flickr = FlickrManager.GetAuthInstance();
            var options = new PhotoSearchOptions();
           
            options.Tags = searchTerm;
            options.SortOrder = PhotoSearchSortOrder.InterestingnessDescending;
            options.Extras = PhotoSearchExtras.CountFaves | PhotoSearchExtras.CountComments;
            
            flickr.PhotosSearchAsync(options, r =>
            {
                if (r.Error != null)
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        MessageBox.Show("An error occurred talking to Flickr: " + r.Error.Message);
                    });
                    return;
                }

                photos = r.Result;


                Dispatcher.BeginInvoke(() =>
                {
                    ResultsListBox.ItemsSource = photos;
                    SystemTray.ProgressIndicator.IsVisible = false;
                });
            });
        }

        private void ResultsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (photos == null || ResultsListBox.SelectedIndex == -1)
            {
                return;
            }

            Photo p = photos.ElementAt(ResultsListBox.SelectedIndex);
            PhoneApplicationService.Current.State["photo"] = p;
            ResultsListBox.SelectedIndex = -1;
            this.NavigationService.Navigate(new Uri("/ImageDisplay.xaml", UriKind.Relative));
        }
    }
}