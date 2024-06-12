using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Models
{
    /// <summary>
    /// 方法过滤器
    /// </summary>
    public class MyActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 执行控制器中方法之前 先执行该方法
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (filterContext.HttpContext.Session["userInfo"] == null)
            {
                //filterContext.HttpContext.Response.Redirect("/Login/Index");
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }
    }
}