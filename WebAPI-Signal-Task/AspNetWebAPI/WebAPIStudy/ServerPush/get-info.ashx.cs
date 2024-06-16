using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ServerPush
{
    /// <summary>
    /// get_info 的摘要说明
    /// </summary>
    public class get_info : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            NewMethod(context);
        }

        private static void NewMethod(HttpContext context)
        {
            //长链接实现
            while (true)
            {
                //模拟等待消息的逻辑
                Task.Delay(3000).Wait();

                context.Response.Write(DateTime.Now);
                //先拿走
                context.Response.Flush();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}