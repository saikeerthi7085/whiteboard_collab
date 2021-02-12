using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using whiteboard_collab;

[assembly: OwinStartup(typeof(whiteboard_collab.Startup))]

namespace whiteboard_collab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
