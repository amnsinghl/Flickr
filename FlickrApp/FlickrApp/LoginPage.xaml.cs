using System;
using System.Linq;
using System.Windows;
using FlickrNet;
using Microsoft.Phone.Controls;

namespace FlickrApp
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private OAuthRequestToken requestToken = null;
        private const string callbackURL = "http://localhost/dummyurl";

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Flickr f = FlickrManager.GetAuthInstance();

            // obtain the request token from Flickr
            f.OAuthGetRequestTokenAsync(callbackURL, r =>
            {
                // Check if an error was returned
                if (r.Error != null)
                {
                    Dispatcher.BeginInvoke(() => { MessageBox.Show("An error occurred getting the request token: " + r.Error.Message); });
                    return;
                }

                // Get the request token
                requestToken = r.Result;

                // get Authorization url
                string url = f.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);
                // Replace www.flickr.com with m.flickr.com for mobile version
                // url = url.Replace("https://www.flickr.com", "http://www.flickr.com");

                // Navigate to url
                Dispatcher.BeginInvoke(() => { LoginWebBrowser.Navigate(new Uri(url)); });

            });
        }

        private void LoginWebBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Url: " + e.Uri.AbsoluteUri);
            Console.Out.WriteLine("Url: " + e.Uri.AbsoluteUri);
            // if we are not navigating to the callback url then authentication is not complete.
            if (!e.Uri.AbsoluteUri.StartsWith(callbackURL)) return;

            // Get "oauth_verifier" part of the query string.
            var oauthVerifier = e.Uri.Query.Split('&')
                .Where(s => s.Split('=')[0] == "oauth_verifier")
                .Select(s => s.Split('=')[1])
                .FirstOrDefault();

            if (String.IsNullOrEmpty(oauthVerifier))
            {
                MessageBox.Show("Unable to find Verifier code in uri: " + e.Uri.AbsoluteUri);
                return;
            }

            // Found verifier, so cancel navigation
            e.Cancel = true;
            LoginWebBrowser.Visibility = Visibility.Collapsed;

            // Obtain the access token from Flickr
            Flickr f = FlickrManager.GetAuthInstance();

            f.OAuthGetAccessTokenAsync(requestToken, oauthVerifier, r =>
            {
                // Check if an error was returned
                if (r.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show("An error occurred getting the access token: " + r.Error.Message));
                    return;
                }

                OAuthAccessToken accessToken = r.Result;

                // Save the oauth token for later use
                FlickrManager.OAuthToken = accessToken.Token;
                FlickrManager.OAuthTokenSecret = accessToken.TokenSecret;

                Dispatcher.BeginInvoke(() => MessageBox.Show("Authentication completed for user " + accessToken.FullName + ", with token " + accessToken.Token));

                this.NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            });
        }
    }
}