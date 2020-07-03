using CESAPSCOREWEBAPP.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CESAPSCOREWEBAPP.Hubs
{
    public class ChatHub : Hub
    {

        private static int _userCount = 0;



        public async Task SendMessage(string user, string message,string pic,string date)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message,pic,date);
        }

        public void Send(string name, string message,string pic,string date)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message,pic,date);
        }


        public async Task SendUserMessage(string user,string pic)
        {
            await Clients.All.SendAsync("ReceiveMessage", user,pic);
        }

        public void SendUser(string name,string pic)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("listlogin", name,pic);
        }


        public void CheckUser(string name, string pic)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("CheckUser", name, pic);
        }






        public Task Draw(int prevX, int prevY, int currentX, int currentY, string color)
        {
            return Clients.Others.SendAsync("draw", prevX, prevY, currentX, currentY, color);
        }








       

    }
}
