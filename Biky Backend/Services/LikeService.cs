using Entities;
using Services.DTO;

namespace Services
{
    public class LikeService
    {
        private readonly DBConnector _dbConnector;
        private readonly UserService _userService;
        private readonly SocialMediaPostService _socialMediaPostService;
        private readonly NotificationService _notificationService;

        public LikeService(DBConnector dbConnector, UserService userService, SocialMediaPostService socialMediaPostService, NotificationService notificationService)
        {
            _dbConnector = dbConnector;
            _userService = userService;
            _socialMediaPostService = socialMediaPostService;
            _notificationService = notificationService;
        }

        public void AddLike(Like like)
        {
            try
            {
                if (ValidateLike(like) && !Exists(like))
                {
                    _notificationService.AddNotification(new NotificationAddRequest()
                    {
                        ReceiverID = _socialMediaPostService.PostOwner(like.PostID),
                        Content = $"{like.User.Nickname} has liked your post"
                    });

                    _dbConnector.Likes.Add(like);
                    _dbConnector.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding like.", ex);
            }
        }

        public void RemoveLike(Like like)
        {
            try
            {
                if (ValidateLike(like) && Exists(like))
                {
                    _dbConnector.Likes.Remove(like);
                    _dbConnector.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error removing like.", ex);
            }
        }

        public bool ValidateLike(Like like)
        {
            try
            {
                if (!_userService.ValidateID(like.UserID))
                {
                    throw new ArgumentException("Given UserID doesn't exist.");
                }
                else if (!_socialMediaPostService.ValidateID(like.PostID))
                {
                    throw new ArgumentException("Given PostID doesn't exist.");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error validating like.", ex);
            }
        }

        public bool Exists(Like like)
        {
            try
            {
                return _dbConnector.Likes.Contains(like);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error checking if like exists.", ex);
            }
        }
    }
}
