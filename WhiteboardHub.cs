﻿using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace whiteboard_collab
{
    [HubName("whiteboard")]
    public class WhiteboardHub : Hub
    {
        private static readonly IDictionary<string, IList<Rectangle>> Shapes = new ConcurrentDictionary<string, IList<Rectangle>>();

        public void Join(string name)
        {
            if (Shapes.ContainsKey(name)) throw new HubException("Nickname already taken");

            Shapes.Add(name, new List<Rectangle>());

            Clients.Caller.name = name;

            Groups.Add(Context.ConnectionId, "Shapes");

            Clients.Caller.draw(
                from u in Shapes.Keys
                from s in Shapes[u]
                select new
                {
                    @from = u, 
                    x = s.Left, 
                    y = s.Top, 
                    width = s.Width, 
                    height = s.Height
                });
        }

        public void Draw(int x, int y, int width, int height)
        {
            Shapes[(string)Clients.Caller.name].Add(new Rectangle(x, y, width, height));

            Clients.Group("Shapes", Context.ConnectionId).draw(new []
            {
                new
                {
                    from = Clients.Caller.name, 
                    x, y, width, height
                }
            });
        }
    }
}