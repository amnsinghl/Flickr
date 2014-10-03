using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrApp.FlickrApi;

namespace FlickrApp.ViewModels
{
    class FavouriteViewModel
    {
        private PhotoFavourite _photoFavourite;

        public string UserIcon
        {
            get { return _photoFavourite.AuthorBuddyIcon; }
        }

        public string UserName
        {
            get 
            {
                return _photoFavourite.RealName.Length == 0 ? _photoFavourite.RealName : _photoFavourite.UserName;
            }
        }


        public string Date
        {
            get { return _photoFavourite.FavoriteDate.Date.ToShortDateString(); }
        }

        public FavouriteViewModel(PhotoFavourite photoFavourite)
        {
            _photoFavourite = photoFavourite;
        }

        public static ObservableCollection<FavouriteViewModel> PhotoFavouriteCollectionToObservableFavouriteViewModel(PhotoFavouriteCollection photoCommentCollection)
        {
            var viewCollection = new ObservableCollection<FavouriteViewModel>();

            foreach (var photoFavourite in photoCommentCollection)
            {
                viewCollection.Add(new FavouriteViewModel(photoFavourite));
            }
            return viewCollection;
        } 
    }
}
