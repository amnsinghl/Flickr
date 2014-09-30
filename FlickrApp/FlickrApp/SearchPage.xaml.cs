using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FlickrNet;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FlickrApp
{
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void SearchButtonTapped(object sender, System.Windows.Input.GestureEventArgs e)
        {
            String searchTerm = SearchTextBox.Text;
            if (searchTerm.Length == 0)
            {
                MessageBox.Show("Enter Something to search");
                return;
            }

            Flickr flickr = FlickrManager.GetInstance();
            var options = new PhotoSearchOptions();
           
            options.Tags = searchTerm;
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

                PhotoCollection photos = r.Result;

                Dispatcher.BeginInvoke(() =>
                {
                    ResultsListBox.ItemsSource = photos;
                });

                ResultsListBox.SelectionChanged += (o, args) =>
                {
                    if (ResultsListBox.SelectedIndex == -1)
                    {
                        return;
                    }

                    Photo p = photos.ElementAt(ResultsListBox.SelectedIndex);
                    MessageBox.Show(p.LargeUrl + "Yay bitch");
                    PhoneApplicationService.Current.State["photo"] = p;
                    ResultsListBox.SelectedIndex = -1;
                    this.NavigationService.Navigate(new Uri("/ImageDisplay.xaml", UriKind.Relative));
                };
            });
        }
    }
}