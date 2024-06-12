using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Models
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class MyExceptionAttribute : HandleErrorAttribute
    {
        //创建一个队列
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();
        /// <summary>
        /// 可以捕获异常数据
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            Exception ex = filterContext.Exception;
            //写入队列
            ExceptionQueue.Enqueue(ex);
            //跳转至错误页面
            filterContext.HttpContext.Response.Redirect("/Error.html");
        }
    }
}