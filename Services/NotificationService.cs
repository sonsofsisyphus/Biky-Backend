using Entities;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NotificationService
    {
        private List<Notification> dbNotifications;

        public void AddNotification(NotificationAddRequest notification)
        {
            dbNotifications.Add(notification.ToNotification());
        }


        public void RemoveLikeNotification(Like like) 
        {
            dbNotifications = dbNotifications.Where(a => a.UserID != like.UserID 
            && a.ContentID != like.PostID
            && a.NotificationType == NotificationType.LIKE).ToList();
        }

        public void RemoveFollowNotification(Following following)
        {
            dbNotifications = dbNotifications.Where(a => a.UserID != following.FollowingID
            && a.ReceiverID != following.FollowerID
            && a.NotificationType == NotificationType.FOLLOW).ToList();
        }

        public void RemoveFollowPostNotification(Guid receiverID, Guid contentID)
        {
            dbNotifications = dbNotifications.Where(a => a.ContentID != contentID 
            && a.ReceiverID != receiverID
            && a.NotificationType == NotificationType.FOLLOW_POST).ToList();
        }

        public void RemoveCommentNotification(Guid contentID)
        {
            dbNotifications = dbNotifications.Where(a => a.ContentID != contentID
            && a.NotificationType == NotificationType.COMMENT).ToList();
        }

        public void SetAllSeen(Guid receiverID)
        {
            foreach(var n in dbNotifications)
            {
                if(n.ReceiverID == receiverID)
                {
                    n.IsSeen = true;
                }
            }
        }

        public int GetUnseenNotificationNumber(Guid receiverID)
        {
            return dbNotifications.Count(a => !a.IsSeen && a.ReceiverID == receiverID);
        }

        public List<Notification> GetAllNotifications(Guid receiverID)
        {
            return dbNotifications.Where(a => a.ReceiverID == receiverID).ToList();
        }

        public List<Notification> GetAllUnseenNotifications(Guid receiverID)
        {
            return dbNotifications.Where(a => a.ReceiverID == receiverID && !a.IsSeen).ToList();
        }
    }
}
