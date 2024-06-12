using BZY.OA.WebApp.Models;
using log4net;
using log4net.Config;
using Spring.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BZY.OA.WebApp
{
    public class MvcApplication : SpringMvcApplication
    {
        protected void Application_Start()
        {
            //log4net日志配置信息读取
            XmlConfigurator.Configure();
            //区域注册
            AreaRegistration.RegisterAllAreas();
            //过滤器注册
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //路由注册
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //开启一个线程处理队列信息
            string filterPath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    //判断一下队列中是否有数据
                    if (MyExceptionAttribute.ExceptionQueue.Count > 0)
                    {
                        Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();
                        if (ex != null)
                        {
                            //将异常信息写到日志文件中
                            ILog logger = LogManager.GetLogger("errorMsg");
                            logger.Error(ex.ToString());
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            }, filterPath);
        }
    }
}
