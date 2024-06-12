using BZY.OA.Common;
using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    /// <summary>
    /// 基类控制器
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 登陆用户信息
        /// </summary>
        public UserInfo LoginUser { get; set; }
        /// <summary>
        /// 执行控制器中方法之前 先执行该方法 与方法过滤器机制一样
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            //if (Session["userInfo"] == null)
            //{
            //    //此方式会继续执行Index中的方法。直到返回ActionResult
            //    //filterContext.HttpContext.Response.Redirect("/Login/Index");
            //    //此方式不会
            //    filterContext.Result = Redirect("/Login/Index");
            //}
            bool isSuccess = false;
            if (Request.Cookies["sessionId"] != null)
            {
                string sessionId = Request.Cookies["sessionId"].Value;
                //检验缓存中是否存储了用户登录信息
                object obj = MemcacheHelper.Get(sessionId);
                if (obj != null)
                {
                    UserInfo userInfo = SerializeHelper.DeserializeToObject<UserInfo>(obj.ToString());
                    LoginUser = userInfo;
                    isSuccess = true;
                    //模拟出滑动过期时间.
                    MemcacheHelper.Set(sessionId, obj, DateTime.Now.AddMinutes(20));
                }
            }
            if (!isSuccess)
            {
                filterContext.Result = Redirect("/Login/Index");
            }
        }
    }
}