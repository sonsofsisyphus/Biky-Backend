using Entities;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class NotificationService
    {
        private readonly DBConnector _dbConnector;

        public NotificationService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

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

        public int GetUnseenNotificationNumber(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Count(a => !a.IsSeen && a.ReceiverID == receiverID);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting unseen notification count.", ex);
            }
        }

        public List<Notification> GetAllNotifications(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Where(a => a.ReceiverID == receiverID).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting all notifications.", ex);
            }
        }

        public List<Notification> GetAllUnseenNotifications(Guid receiverID)
        {
            try
            {
                return _dbConnector.Notifications.Where(a => a.ReceiverID == receiverID && !a.IsSeen).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error getting all unseen notifications.", ex);
            }
        }
    }
}
