using Biky_Backend.Services.DTO;
using Entities;
using Services.DTO;

namespace Services
{
    public class NotificationService
    {
        private readonly DBConnector _dbConnector;

        public NotificationService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        // Method to add a new notification to the database.
        public void AddNotification(NotificationAddRequest notification)
        {
            try
            {
                _dbConnector.Notifications.Add(notification.ToNotification());
                _dbConnector.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding notification.", ex);
            }
        }

        // Method to delete a specific notification by receiver ID and content.
        public void DeleteNotification(Guid receiverID, string content)
        {
            try
            {
                var notification = _dbConnector.Notifications
                    .FirstOrDefault(n => n.ReceiverID == receiverID && n.Content == content);
                if (notification != null)
                {
                    _dbConnector.Notifications.Remove(notification);
                    _dbConnector.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting notification.", ex);
            }
        }

        // Method to mark all notifications as seen for a specific user.
        public void SetAllSeen(Guid receiverID)
        {
            try
            {
                var notifications = _dbConnector.Notifications.Where(n => n.ReceiverID == receiverID);
                foreach (var notification in notifications)
                {
                    notification.IsSeen = true;
                }

                _dbConnector.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error setting all notifications as seen.", ex);
            }
        }

        // Method to get the number of unseen notifications for a specific user.
        public int GetUnseenNotificationNumber(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Count(a => a.ReceiverID == receiverID && !a.IsSeen);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting unseen notification count.", ex);
            }
        }

        // Method to get all notifications for a specific user.
        public List<NotificationSendRequest> GetAllNotifications(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Where(a => a.ReceiverID == receiverID)
                    .ToList()
                    .ConvertAll(n => NotificationSendRequest.ToNotificationSendRequest(n));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting all notifications.", ex);
            }
        }

        // Method to get all unseen notifications for a specific user.
        public List<NotificationSendRequest> GetAllUnseenNotifications(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Where(a => a.ReceiverID == receiverID && !a.IsSeen)
                    .ToList()
                    .ConvertAll(n => NotificationSendRequest.ToNotificationSendRequest(n));
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting all unseen notifications.", ex);
            }
        }

        // Method to generate notification content based on the notification type and user nickname.
        public string GetNotificationContent(NotificationType type, string nickname)
        {
            return type switch
            { 
                NotificationType.LIKE => $"{nickname} has liked your post",
                NotificationType.COMMENT => $"{nickname} has made a comment on your post",
                NotificationType.FOLLOW => $"{nickname} has followed you",
                _ => ""
            };
        }
    }
}
