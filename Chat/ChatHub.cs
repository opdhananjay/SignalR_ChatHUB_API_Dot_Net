using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TC.Models;

namespace TC.Chat
{
    public class ChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IDictionary<string, UserConnection> _connections;
        public ChatHub(IDictionary<string,UserConnection> connection)
        {
            _connections = connection;
        }

        public async Task joinRoom(UserConnection userConnection)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.Room);
                await Clients.All.SendAsync("ReceiveMessage", "Lets Program Bot", $"{userConnection.User} has joined Room", DateTime.Now);
                _connections[Context.ConnectionId] = userConnection;
                foreach (var connection in _connections.Values)
                {
                    Console.WriteLine("Connection ID:" + connection);
                    
                }
                await SendConnectedUser(userConnection.Room!); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // Send Message 
        public async Task SendMessage(string message)
        {
            // First Try To Get Value From Dictory by context.ConnectioniD then if found then return UserConnection object that have room and user name 
            // if found then add in sedn the message in room 

            if(_connections.TryGetValue(Context.ConnectionId,out UserConnection userConnection))
            {
                await Clients.Groups(userConnection.Room!).SendAsync(method: "ReceiveMessage",arg1: userConnection.User,arg2:message, arg3:DateTime.Now);
            }

        }

        // Send Connected users
        public Task SendConnectedUser(string Room)
        {
            var connectedUser = _connections.Values.Where(u => u.Room == Room).Select(s => s.User);
            return Clients.Group(Room).SendAsync(method: "ConnectedUser",connectedUser);
        }

        // on User Disconnect => 
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (!_connections.TryGetValue(Context.ConnectionId, out UserConnection userConnection))
            {
                return base.OnDisconnectedAsync(exception);
            }
 
            Clients.Group(userConnection.Room).SendAsync(method: "ReceiveMessage", arg1: "Lets Disconnect Bot", arg2: $"{userConnection.User} has Left the Group", DateTime.Now);
            SendConnectedUser(userConnection.Room!);
            return base.OnDisconnectedAsync(exception);
        }


    }
}
