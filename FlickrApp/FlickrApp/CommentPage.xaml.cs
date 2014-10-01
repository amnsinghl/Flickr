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
    public partial class CommentPage : PhoneApplicationPage
    {
        public CommentPage()
        {
            InitializeComponent();
        }

        string photoid = "";

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            

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
            f.PhotosCommentsGetListAsync(photoid, r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Error Occured in fetching Comments");
                    return;
                }

                PhotoCommentCollection photoCommentCollection = r.Result;
                Dispatcher.BeginInvoke(() =>
                {
                    CommentList.ItemsSource = photoCommentCollection;
                    SystemTray.ProgressIndicator.IsVisible = false;
                });


            });
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

        private void PostButton_Clicked(object sender, RoutedEventArgs e)
        {
            string comment = CommentTextBox.Text;
            if (comment.Length == 0)
            {
                MessageBox.Show("Comment can not be empty");
                return;
            }
            if (!FlickrManager.IsLoggedIn())
            {
                MessageBox.Show("You need to login before you can comment");
                this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
                return;
            }

            ShowProgressIndicator();
            Flickr flickr = FlickrManager.GetAuthInstance();
            flickr.PhotosCommentsAddCommentAsync(photoid,comment, r =>
            {
                HideProgressIndicator();
                if (r.Result == null)
                {
                    MessageBox.Show("Error occured in creating a comment");
                }
                else
                {
                    MessageBox.Show("Comment Added");
                    CommentTextBox.Text = "";
                }
            });
        }
    }
}