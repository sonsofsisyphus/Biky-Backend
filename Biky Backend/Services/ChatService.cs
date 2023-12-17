using Biky_Backend.Services.DTO;
using Entities;
using Services;

namespace Biky_Backend.Services
{
    public class ChatService
    {
        private readonly DBConnector _dbContext;
        private readonly UserService _userService;

        public ChatService(DBConnector dbContext, UserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        // Method to get messages between two users.
        public List<ChatMessageRequest>? GetMessages(ChatMessageSendRequest msg)
        {
            /*
            Since all chats include 2 people texting each other, receiver and sender ID changes
            depending on who sent the message. Therefore, all messages in a chat have the same
            receiver and sender IDs although they can be swapped in some other messages.
             */
            var where = _dbContext.ChatMessages
            .Where(m => (
                            m.ReceiverID == msg.ReceiverID && m.SenderID == msg.SenderID
                            ) || (
                            m.ReceiverID == msg.SenderID && m.SenderID == msg.ReceiverID
                            ) && m.Content != ""
                );
            var orderBy = where.OrderBy(m => m.DateTime);
            var toList = orderBy.ToList();
            var convertAll = toList.ConvertAll(m => ChatMessageRequest.ToChatMessageRequest(m));

            return convertAll;
        }

        // Method to open a chat between two users if it doesn't already exist.
        public void OpenChat(Guid senderID, Guid receiverID)
        {
            if(!_dbContext.ChatMessages.Any(m => (m.ReceiverID == receiverID && m.SenderID == senderID
                            ) || (
                            m.ReceiverID == receiverID && m.SenderID == senderID
                            )))
            {
                SendMessage(new ChatMessageAddRequest
                {
                    SenderID = senderID,
                    ReceiverID = receiverID,
                    Content = ""
                });
            }
        }

        // Method to send a chat message.
        public async Task<ChatMessage> SendMessage(ChatMessageAddRequest message)
        {
            try
            {
                var msg = message.ToMsg();
                _dbContext.ChatMessages.Add(msg);
                await _dbContext.SaveChangesAsync();

                return msg;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                return null;
            }
        }

        // Method to get all chat history for a user.
        public List<ChatHistorySendRequest> GetAllChats(Guid currentUserID)
        {
            var allDistintUsers = _dbContext.ChatMessages
                .Where(m => m.SenderID == currentUserID || m.ReceiverID == currentUserID)
                .OrderBy(m => m.DateTime)
                .Select(m => m.SenderID == currentUserID ? m.ReceiverID : m.SenderID)
                .Distinct()
                .ToList();

            var allChats = new List<ChatHistorySendRequest>();
            foreach (var userID in allDistintUsers)
                allChats.Add(new ChatHistorySendRequest(
                    _userService.GetUserByID(userID), GetLastMessage(currentUserID, userID)));

            return allChats;
        }

        // Helper method to get the last message in a chat between two users to show on chat screen.
        private ChatMessageRequest GetLastMessage(Guid firstUser, Guid secondUser)
        {
            var lastMessage = _dbContext.ChatMessages
                .Where(m => (
                                m.ReceiverID == firstUser && m.SenderID == secondUser
                                ) || (
                                m.ReceiverID == secondUser && m.SenderID == firstUser
                                )
                    )
                .OrderBy(m => m.DateTime)
                .Last();

            return ChatMessageRequest.ToChatMessageRequest(lastMessage);
        }
    }
}
