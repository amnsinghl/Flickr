using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using FlickrNet;

namespace FlickrApp.FlickrApi
{
    class FlickrPhotoFavourite
    {
        private string url = "https://api.flickr.com/services/rest/?method=flickr.photos.getFavorites&format=rest";

        public void fetch(string photoid, int perpage, Action<FlickrResult<PhotoFavouriteCollection>> callback)
        {
            WebClient client = new WebClient();
            client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

            client.DownloadStringAsync(new Uri(url + "&api_key=" + FlickrManager.ApiKey + "&per_page=" + perpage + "&photo_id=" + photoid, UriKind.Absolute));
            client.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
            {
                var result = new FlickrResult<PhotoFavouriteCollection>();
                if (e.Error != null)
                {
                    result.HasError = true;
                }
                else
                {
                    PhotoFavouriteCollection cpfc = new PhotoFavouriteCollection();
                    System.Diagnostics.Debug.WriteLine(e.Result);
                    XmlReader xmlReader = XmlReader.Create(new StringReader(e.Result));
                    xmlReader.ReadToFollowing("photo");
                    ((IFlickrParsable)cpfc).Load(xmlReader);
                    result.Result = cpfc;
                }
                
                callback(result);
            };

        }

    }
}
