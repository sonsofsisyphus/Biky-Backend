using Biky_Backend.Services;
using Biky_Backend.Services.DTO;
using Microsoft.AspNetCore.SignalR;
using Services;

namespace Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }
        public async Task SendMessage(MesssageAddRequest mar)
        {
           // await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}