﻿using System.IO.IsolatedStorage;
using FlickrNet;

namespace FlickrApp.FlickrApi
{
    public class FlickrManager
    {
        public const string ApiKey = "3a68f22971d8d66b521b362c312c175c";
        public const string SharedSecret = "b2acf0fb7910be24";

        public const string OauthTokenString = "OAuthToken";
        public const string OauthSecretString = "OAuthTokenSecret";
        public const string UserIdString = "UserId";
        public const string UserNameString = "UserName";

        public static Flickr GetAuthInstance()
        {
            var f = new Flickr(ApiKey, SharedSecret)
            {
                OAuthAccessToken = OAuthToken,
                OAuthAccessTokenSecret = OAuthTokenSecret
            };
            return f;
        }

        public static Flickr GetInstance()
        {
            return new Flickr(ApiKey, SharedSecret);
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

        public static string UserId
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(UserIdString))
                    return IsolatedStorageSettings.ApplicationSettings[UserIdString] as string;
                else
                    return null;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[UserIdString] = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains(UserNameString))
                    return IsolatedStorageSettings.ApplicationSettings[UserNameString] as string;
                else
                    return null;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[UserNameString] = value;
            }
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
