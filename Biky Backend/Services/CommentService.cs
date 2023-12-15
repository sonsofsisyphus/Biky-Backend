using System;
using System.Linq;
using System.Collections.Generic;
using Entities;
using Services.DTO;
using Biky_Backend.Services.DTO;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class CommentService
    {
        private readonly DBConnector _dbContext;
        private readonly UserService _userService;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly NotificationService _notificationService;

        public CommentService(DBConnector dbContext, UserService userService, SocialMediaPostService socialMediaPostService, NotificationService notificationService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _socialMediaPostService = socialMediaPostService;
            _notificationService = notificationService;
        }

        public Guid? AddComment(CommentAddRequest commentAddRequest)
        {
            Comment comment = commentAddRequest.ToComment();
            if (ValidateComment(comment))
            {
                _notificationService.AddNotification(new NotificationAddRequest()
                {
                    ReceiverID = _socialMediaPostService.PostOwner(comment.PostID),
                    Content = $"{_userService.GetUserByID(comment.AuthorID).Nickname} has made a comment on your post"
                });
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();
                return comment.CommentID;
            }
            return null;
        }

        public void EditComment(CommentEditRequest comment)
        {
            var existingComment = _dbContext.Comments.Find(comment.CommentID);
            if (existingComment != null)
            {
                existingComment.Content = comment.Content;
                _dbContext.SaveChanges();
            }
        }

        public void RemoveComment(Guid commentID)
        {
            var existingComment = _dbContext.Comments.Find(commentID);
            if (existingComment != null)
            {
                _dbContext.Comments.Remove(existingComment);
                _dbContext.SaveChanges();
            }
        }

        public List<CommentSendRequest> GetCommentByPost(Guid postID)
        {
            return _dbContext.Comments.Where(a => a.PostID == postID).Include(c => c.Author).Select(CommentSendRequest.ToCommentSendRequest).ToList();
        }

        public Comment? GetCommentByID(Guid commentID)
        {
            return _dbContext.Comments.FirstOrDefault(a => a.CommentID == commentID);
        }

        public bool ValidateComment(Comment comment)
        {
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
