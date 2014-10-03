using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FlickrNet;

namespace FlickrApp.FlickrApi
{
    class PhotoFavourite: IFlickrParsable
    {
        /// <summary>
        /// The Flickr User ID of the user who favourited the photo.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The user name of the user who favourited the photo.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The user name of the user who favourited the photo.
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// The date the hoto was favourited.
        /// </summary>
        public DateTime FavoriteDate { get; set; }

        /// <summary>
        /// The server for the commenting users buddy icon.
        /// </summary>
        public string IconServer { get; set; }

        /// <summary>
        /// The farm for the commenting users buddy icon.
        /// </summary>
        public string IconFarm { get; set; }

        /// <summary>
        /// The comment authors buddy icon.
        /// </summary>
        public string AuthorBuddyIcon
        {
            get
            {
                return UtilityMethods.BuddyIcon(IconServer, IconFarm, UserId);
            }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public PhotoFavourite()
        {
        }

        void IFlickrParsable.Load(XmlReader reader)
        {
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "nsid":
                        UserId = reader.Value;
                        break;
                    case "realname":
                        RealName = reader.Value;
                        break;
                    case "username":
                        UserName = reader.Value;
                        break;
                    case "favedate":
                        FavoriteDate = UtilityMethods.UnixTimestampToDate(reader.Value);
                        break;
                    case "iconserver":
                        IconServer = reader.Value;
                        break;
                    case "iconfarm":
                        IconFarm = reader.Value;
                        break;
                    default:
                        break;
                }
            }

//            reader.Skip();
        }

    }
}
