using Entities;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services
{
    public class CommentService
    {
        private List<Comment> dbComments;
        private readonly UserService _userService;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly NotificationService _notificationService;

        public CommentService(UserService userService, SocialMediaPostService socialMediaPostService, NotificationService notificationService)
        {
            _userService = userService;
            _socialMediaPostService = socialMediaPostService;
            _notificationService = notificationService;
        }

        public Guid AddComment(CommentAddRequest commentAddRequest)
        {
            Comment comment = commentAddRequest.ToComment();
            if(ValidateComment(comment))
            {
                _notificationService.AddNotification(new NotificationAddRequest()
                {
                    NotificationType = NotificationType.COMMENT,
                    ReceiverID = _socialMediaPostService.PostOwner(comment.PostID),
                    UserID = comment.AuthorID,
                    ContentID = comment.CommentID
                });
                dbComments.Add(comment);
            }
            return comment.CommentID;
        }

        public void EditComment(CommentEditRequest comment)
        {
            var index = dbComments.FindIndex(a => a.CommentID == comment.CommentID);
            if (index != -1)
            {
                dbComments[index].Content = comment.Content;
            }
        }

        public void RemoveComment(Guid commentID)
        {
            var index = dbComments.FindIndex(a => a.CommentID == commentID);
            if (index != -1)
            {
                _notificationService.RemoveCommentNotification(commentID);
                dbComments.RemoveAt(index);
            }
        }

        public List<Comment> GetCommentByPost(Guid postID)
        {
            return dbComments.Where(a => a.PostID == postID).ToList();
        }

        public Comment? GetCommentByID(Guid commentID)
        {
            return dbComments.FirstOrDefault(a => a.CommentID == commentID);
        }

        public bool ValidateComment(Comment comment) {
            if (!_userService.ValidateID(comment.AuthorID))
            {
                throw new ArgumentException("Given UserID doesn't exist.");
            }
            else if (!_socialMediaPostService.ValidateID(comment.PostID))
            {
                throw new ArgumentException("Given PostID doesn't exist.");
            }
            return true;
            }
        }
}
