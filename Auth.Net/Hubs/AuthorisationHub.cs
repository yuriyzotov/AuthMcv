using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Auth.Net.Hubs
{
    public class AuthorisationHub : Hub
    {

        static readonly ConcurrentDictionary<string, string> _clientConnections = new ConcurrentDictionary<string, string>();

        public   static int GetLiveConnectionsCount(string userName)
        {
            return _clientConnections.Where(c => c.Value == userName).Count();
        }


        public static void Refresh(string userName)
        {
            var conections = _clientConnections.Where(c => c.Value == userName).Select(c => c.Key).Distinct().ToList();
            var hub = GlobalHost.ConnectionManager.GetHubContext<AuthorisationHub>();
            foreach (var con in conections)
            {
                hub.Clients.Client(con).refresh();
                
            }
        }


        public override Task OnConnected()
        {
            var userName = Context.User.Identity.IsAuthenticated? Context.User.Identity.Name:null;
            _clientConnections.AddOrUpdate(Context.ConnectionId, userName, (n, l)=> userName);
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            var userName = Context.User.Identity.IsAuthenticated ? Context.User.Identity.Name : null;
            _clientConnections.AddOrUpdate(Context.ConnectionId, userName, (n, l) => userName);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string userName;
            _clientConnections.TryRemove(Context.ConnectionId, out userName);
            return base.OnDisconnected(stopCalled);
        }
    }
}