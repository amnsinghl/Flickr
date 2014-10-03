using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using FlickrNet;
using MemberTypes = FlickrNet.MemberTypes;

namespace FlickrApp.FlickrApi
{
    class UtilityMethods
    {
        private static readonly DateTime UnixStartDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converts a <see cref="DateTime"/> object into a unix timestamp number.
        /// </summary>
        /// <param name="date">The date to convert.</param>
        /// <returns>A long for the number of seconds since 1st January 1970, as per unix specification.</returns>
        public static string DateToUnixTimestamp(DateTime date)
        {
            TimeSpan ts = date - UnixStartDate;
            return ts.TotalSeconds.ToString("0", System.Globalization.NumberFormatInfo.InvariantInfo);
        }

        /// <summary>
        /// Converts a string, representing a unix timestamp number into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="timestamp">The timestamp, as a string.</param>
        /// <returns>The <see cref="DateTime"/> object the time represents.</returns>
        public static DateTime UnixTimestampToDate(string timestamp)
        {
            if (String.IsNullOrEmpty(timestamp)) return DateTime.MinValue;
            try
            {
                return UnixTimestampToDate(Int64.Parse(timestamp, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo));
            }
            catch (FormatException)
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Converts a <see cref="long"/>, representing a unix timestamp number into a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="timestamp">The unix timestamp.</param>
        /// <returns>The <see cref="DateTime"/> object the time represents.</returns>
        public static DateTime UnixTimestampToDate(long timestamp)
        {
            return UnixStartDate.AddSeconds(timestamp);
        }


        
        
        /// <summary>
        /// Returns the buddy icon for a given set of server, farm and userid. If no server is present then returns the standard buddy icon.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="farm"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string BuddyIcon(string server, string farm, string userId)
        {
            if (String.IsNullOrEmpty(server) || server == "0")
                return "https://www.flickr.com/images/buddyicon.jpg";
            else
                return String.Format(System.Globalization.CultureInfo.InvariantCulture, "https://farm{0}.staticflickr.com/{1}/buddyicons/{2}.jpg", farm, server, userId);
        }

    }

}

