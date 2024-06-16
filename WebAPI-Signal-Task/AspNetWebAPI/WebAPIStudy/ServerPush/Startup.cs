using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(ServerPush.Startup))]

namespace ServerPush
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //配置路由
            app.MapSignalR();
        }
    }
}
