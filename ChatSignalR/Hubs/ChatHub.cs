using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace ChatSignalR.Hubs
{

    public class User
    {
        public string Username { get; set; }
        public string ConnectionId { get; set; }
    }

    public static class UserHandler
    {
        public static List<User> ConnectedUsers = new List<User>();
    }


    public class ChatHub : Hub
    {
        public void Hello(string username)
        {
            Clients.All.hello(username);
        }

        public void Update(string username)
        {
            UserHandler.ConnectedUsers.Add(new User { Username = username, ConnectionId = Context.ConnectionId });
            string usernames = string.Join(", ", UserHandler.ConnectedUsers.Select(u => u.Username));
            Clients.All.update(usernames);
        }

        public void Send(string username, string message)
        {
            Clients.All.addNewMessageToPage(username, message);
        }

        public override Task OnDisconnected(bool d)
        {
            User userToRemove = UserHandler.ConnectedUsers.First(u => u.ConnectionId == Context.ConnectionId);
            UserHandler.ConnectedUsers.Remove(userToRemove);
            string usernames = string.Join(", ", UserHandler.ConnectedUsers.Select(u => u.Username));
            Clients.All.update(usernames);
            return base.OnDisconnected(true);
        }
    }
}