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
            //log4net��־������Ϣ��ȡ
            XmlConfigurator.Configure();
            //����ע��
            AreaRegistration.RegisterAllAreas();
            //������ע��
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //·��ע��
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //����һ���̴߳��������Ϣ
            string filterPath = Server.MapPath("/Log/");
            ThreadPool.QueueUserWorkItem((a) =>
            {
                while (true)
                {
                    //�ж�һ�¶������Ƿ�������
                    if (MyExceptionAttribute.ExceptionQueue.Count > 0)
                    {
                        Exception ex = MyExceptionAttribute.ExceptionQueue.Dequeue();
                        if (ex != null)
                        {
                            //���쳣��Ϣд����־�ļ���
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
