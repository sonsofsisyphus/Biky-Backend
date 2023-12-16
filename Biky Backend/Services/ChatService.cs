using Biky_Backend.Entities;
using Biky_Backend.Services.DTO;
using Entities;
using Services;

namespace Biky_Backend.Services
{
    public class ChatService
    {
        private readonly DBConnector _dbConnector;

        public ChatService(DBConnector dbConnector)
        {
            _dbConnector = dbConnector;
        }

        public List<Chat> GetChats(Guid userID)
        {
            return _dbConnector.Chats.Where(c => c.SenderID  == userID || c.ReceiverID == userID).ToList();
        }

        public List<Message> GetChatHistory(Guid chatID)
        {
            return _dbConnector.Messages.Where(m => m.ChatID == chatID).OrderByDescending(m => m.DateTime).ToList();
        }

        public void AddMessage(MesssageAddRequest message)
        {
            _dbConnector.Add(message.ToMessage());
            _dbConnector.SaveChanges();
        }
    }
}
