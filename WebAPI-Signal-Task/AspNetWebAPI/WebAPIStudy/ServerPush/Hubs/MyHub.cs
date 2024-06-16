using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerPush.Hubs
{
    [HubName("BZYHub")]
    public class MyHub : Hub
    {
        /// <summary>
        /// 服务端的HUB方法
        /// </summary>
        public void Send(string name, string message)
        {
            //ALL所有连接的客户端
            //Clients.All.recive(name, message);
            //调用者 谁掉我 我掉谁
            //Clients.Caller
            //除了我之外的
            //Clients.Others
            Clients.Others.recive(name, message);
        }
    }
}