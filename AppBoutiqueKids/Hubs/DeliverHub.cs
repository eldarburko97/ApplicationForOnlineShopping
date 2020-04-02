using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppBoutiqueKids.Hubs
{
    public class DeliverHub: Hub
    {
        public Task SendUpdateQuantity(int quantity)
        {

            return Clients.All.SendAsync("RecieveUpdatedQuantity", quantity);

        }
    }
}
