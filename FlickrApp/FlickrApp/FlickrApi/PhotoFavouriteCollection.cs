using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FlickrNet;

namespace FlickrApp.FlickrApi
{
    class PhotoFavouriteCollection: System.Collections.ObjectModel.Collection<PhotoFavourite>, IFlickrParsable
    {
        /// <summary>
        /// The ID of the photo.
        /// </summary>
        public string PhotoId { get; set; }

        /// <summary>
        /// The page of favourites that has been returned.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// The number of favourites returned per page.
        /// </summary>
        public int PerPage { get; set; }

        /// <summary>
        /// The total number of favourites for this photo.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// The number of pages of favourites that are available.
        /// </summary>
        public int Pages { get; set; }

        void IFlickrParsable.Load(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "id":
                        PhotoId = reader.Value;
                        break;
                    case "page":
                        Page = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "pages":
                        Pages = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "perpage":
                        PerPage = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    case "total":
                        Total = int.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                        break;
                    default:
                        break;
                }
            }

            reader.ReadToFollowing("person");

            while (reader.LocalName == "person")
            {
                System.Diagnostics.Debug.WriteLine(reader.LocalName);
                PhotoFavourite favorite = new PhotoFavourite();
                ((IFlickrParsable)favorite).Load(reader);
                Add(favorite);
                reader.ReadToFollowing("person");
            }

            // Skip to next element (if any)

        }

    }
}

