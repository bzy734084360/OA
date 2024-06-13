using BZY.OA.Common;
using BZY.OA.IBLL;
using BZY.OA.Model;
using Spring.Context;
using Spring.Context.Support;
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

                    //留一个后门，测试方便。发布的时候一定要删除该代码。
                    if (LoginUser.UName == "管理员")
                    {
                        return;
                    }


                    //完成权限校验。
                    //获取用户请求的URL地址.
                    string url = Request.Url.AbsolutePath.ToLower();
                    //获取请求的方式.
                    string httpMehotd = Request.HttpMethod;
                    //根据获取的URL地址与请求的方式查询权限表。
                    IApplicationContext ctx = ContextRegistry.GetContext();
                    IBLL.IActionInfoService ActionInfoService = (IBLL.IActionInfoService)ctx.GetObject("ActionInfoService");
                    var actionInfo = ActionInfoService.LoadEntities(a => a.Url == url && a.HttpMethod == httpMehotd).FirstOrDefault();
                    //说明该页面没被限制
                    if (actionInfo == null)
                    {
                        return;
                    }

                    //判断用户是否具有所访问的地址对应的权限
                    IUserInfoService UserInfoService = (IUserInfoService)ctx.GetObject("UserInfoService");
                    var loginUserInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
                    //1:可以先按照用户权限这条线进行过滤。
                    var isExt = (from a in loginUserInfo.R_UserInfo_ActionInfo
                                 where a.ActionInfoID == actionInfo.ID
                                 select a).FirstOrDefault();
                    if (isExt != null)
                    {
                        if (isExt.IsPass)
                        {
                            return;
                        }
                        else
                        {
                            filterContext.Result = Redirect("/Error.html");
                            return;
                        }

                    }
                    //2：按照用户角色权限这条线进行过滤。
                    var loginUserRole = loginUserInfo.RoleInfo;
                    var count = (from r in loginUserRole
                                 from a in r.ActionInfo
                                 where a.ID == actionInfo.ID
                                 select a).Count();
                    if (count < 1)
                    {
                        filterContext.Result = Redirect("/Error.html");
                        return;
                    }

                }
            }
            if (!isSuccess)
            {
                filterContext.Result = Redirect("/Login/Index");
            }
        }
    }
}