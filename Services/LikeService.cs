using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LikeService
    {
        private List<Like> dbLikes;
        private readonly UserService _userService;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly NotificationService _notificationService;

        public LikeService(UserService userService, SocialMediaPostService socialMediaPostService, NotificationService notificationService)
        {
            _userService = userService;
            _socialMediaPostService = socialMediaPostService;
            _notificationService = notificationService;
        }

        public void AddLike(Like like)
        {
            if(ValidateLike(like) && !Exists(like))
            {
                _notificationService.AddNotification(new DTO.NotificationAddRequest()
                {
                    NotificationType = NotificationType.LIKE,
                    ReceiverID = _socialMediaPostService.PostOwner(like.PostID),
                    UserID = like.UserID,
                    ContentID = like.PostID
                });
                dbLikes.Add(like);
                _socialMediaPostService.AddLikeToPost(like);
            }
        }

        public void RemoveLike(Like like)
        {
            if (ValidateLike(like) && Exists(like))
            {
                _notificationService.RemoveLikeNotification(like);
                dbLikes.Remove(like);
                _socialMediaPostService.RemoveLikeFromPost(like);
            }
        }

        public bool ValidateLike(Like like) 
        {
            if(!_userService.ValidateID(like.UserID))
            {
                throw new ArgumentException("Given UserID doesn't exist.");
            } else if(!_socialMediaPostService.ValidateID(like.PostID))
            {
                throw new ArgumentException("Given PostID doesn't exist.");
            } 
            return true;
        }

        public bool Exists(Like like)
        {
            return dbLikes.Contains(like);
        }

    }
}
