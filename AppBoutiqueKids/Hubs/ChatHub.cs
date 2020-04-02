using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Hubs
{
    public class ChatHub:Hub
    {
        public Task SendMessage(string user,string message)
        {
           return Clients.User(user).SendAsync("RecieveMessage",message);
        }
    }
}
