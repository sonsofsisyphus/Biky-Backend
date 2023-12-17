using Biky_Backend.Services.DTO;
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

        public void AddLike(LikeRequest like)
        {
            try
            {
                if (ValidateLike(like) && !Exists(like))
                {
                    var receiverID = _socialMediaPostService.PostOwner(like.PostID);

                    if (receiverID != like.UserID)
                    {
                        _notificationService.AddNotification(new NotificationAddRequest()
                        {
                            ReceiverID = _socialMediaPostService.PostOwner(like.PostID),
                            Content = _notificationService.GetNotificationContent(NotificationType.LIKE, _userService.GetUserByID(like.UserID).Nickname)
                        });
                    }

                    _dbConnector.Likes.Add(like.ToLike());
                    _dbConnector.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding like.", ex);
            }
        }

        public void RemoveLike(LikeRequest like)
        {
            try
            {
                if (ValidateLike(like) && Exists(like))
                {
                    var likeToRemove = _dbConnector.Likes.FirstOrDefault(l => (l.PostID == like.PostID && l.UserID == like.UserID));
                    if (likeToRemove != null)
                    {
                        _dbConnector.Remove(likeToRemove);
                        _dbConnector.SaveChanges();

                        _notificationService.DeleteNotification(
                            _socialMediaPostService.PostOwner(like.PostID),
                            _notificationService.GetNotificationContent(NotificationType.LIKE, _userService.GetUserByID(like.UserID).Nickname)
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error removing like.", ex);
            }
        }

        public bool ValidateLike(LikeRequest like)
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

        public bool Exists(LikeRequest like)
        {
            try
            {
                return _dbConnector.Likes.Any(l => (l.PostID == like.PostID && l.UserID == like.UserID));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error checking if like exists.", ex);
            }
        }

        public int CountLike(Guid postID)
        {
            return _dbConnector.Likes.Count(l => l.PostID == postID);
        }
    }
}
