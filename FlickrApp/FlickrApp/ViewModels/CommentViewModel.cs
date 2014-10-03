using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlickrNet;

namespace FlickrApp.ViewModels
{
    class CommentViewModel
    {
        private PhotoComment _photoComment;

        public string UserIcon
        {
            get { return _photoComment.AuthorBuddyIcon; }
        }

        public string UserName
        {
            get 
            {
                return _photoComment.AuthorRealName.Length == 0 ? _photoComment.AuthorRealName : _photoComment.AuthorUserName;
            }
        }

        public string Comment
        {
            get { return _photoComment.CommentHtml; }
        }

        public string Date
        {
            get { return _photoComment.DateCreated.Date.ToShortDateString(); }
        }

        public CommentViewModel(PhotoComment photoComment)
        {
            _photoComment = photoComment;
        }

        public static ObservableCollection<CommentViewModel> PhotoCommentCollectionToObservableCommentViewModel(PhotoCommentCollection photoCommentCollection)
        {
            var commentViewCollection = new ObservableCollection<CommentViewModel>();

            foreach (var photoComment in photoCommentCollection)
            {
                commentViewCollection.Add(new CommentViewModel(photoComment));
            }
            return commentViewCollection;
        } 
    }
}
