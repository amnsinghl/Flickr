using System.IO.IsolatedStorage;
using FlickrNet;

namespace FlickrApp
{
    public class FlickrManager
    {
        public const string ApiKey = "3a68f22971d8d66b521b362c312c175c";
        public const string SharedSecret = "b2acf0fb7910be24";

        public const string OauthTokenString = "OAuthToken";
        public const string OauthSecretString = "OAuthTokenSecret";

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(ApiKey, SharedSecret)
            {
                OAuthAccessToken = OAuthToken,
                OAuthAccessTokenSecret = OAuthTokenSecret
            };
            return f;
        }

        public static void Logout()
        {
            IsolatedStorageSettings.ApplicationSettings.Remove(OauthTokenString);
            IsolatedStorageSettings.ApplicationSettings.Remove(OauthSecretString);
        }

        public static bool IsLoggedIn()
        {
            return OAuthToken != null && OAuthTokenSecret != null;
        }

        public static string OAuthToken
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(OauthTokenString))
                    return IsolatedStorageSettings.ApplicationSettings[OauthTokenString] as string;
                else
                    return null;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[OauthTokenString] = value;
            }
        }

        public static string OAuthTokenSecret
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(OauthSecretString))
                    return IsolatedStorageSettings.ApplicationSettings[OauthSecretString] as string;
                else
                    return null;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[OauthSecretString] = value;
            }
        }
    }
}
