using BZY.OA.Common;
using BZY.OA.WebApp.JobScheduler;
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

            //开启lucene.net线程
            IndexManager.GetInstance().StartThread();
            //定时统计热词
            SyncHotKeyWordsInfo();
        }
        /// <summary>
        /// 定时统计热词
        /// </summary>
        private void SyncHotKeyWordsInfo()
        {
            //定时统计热词50分钟触发一次
            const string cronExprToDo = "0 0/50 * * * ?";
            if (JobHelper.CreateJob("SyncHotKeyWordsInfoJob", "定时统计热词", typeof(SyncHotKeyWordsInfoJob), "定时统计热词", Guid.NewGuid().ToString(), cronExprToDo))
            {
                var sched = Scheduler.GetIntance();
                sched.Start();
            }
        }
    }
}
