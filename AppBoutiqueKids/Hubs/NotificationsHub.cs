using AppBoutiqueKids.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Hubs
{
    public class NotificationsHub : Hub
    {
        public async Task AddToCartMethod(string count)
        {
            await Clients.Caller.SendAsync("UpdateCart", int.Parse(count) + 1);
        }
    }
}
