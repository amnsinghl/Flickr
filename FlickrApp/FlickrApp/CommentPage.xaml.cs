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
            f.PhotosCommentsGetListAsync(photoid, r =>
            {
                if (r.Result == null)
                {
                    MessageBox.Show("Error Occured in fetching Comments");
                    return;
                }

                PhotoCommentCollection photoCommentCollection = r.Result;
                PhotoComment psd;
                Dispatcher.BeginInvoke(() =>
                {
                    CommentList.ItemsSource = photoCommentCollection;
                    SystemTray.ProgressIndicator.IsVisible = false;
                });


            });
        }
    }
}