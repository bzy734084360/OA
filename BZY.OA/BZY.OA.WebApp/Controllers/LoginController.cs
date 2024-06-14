using BZY.OA.Common;
using BZY.OA.Common.Cache;
using BZY.OA.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    public class LoginController : Controller
    {
        IUserInfoService UserInfoService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        #region 完成用户登录

        public ActionResult UserLogin()
        {
            //验证码校验
            string validateCode = Session["validateCode"] != null ? Session["validateCode"].ToString() : string.Empty;
            if (string.IsNullOrEmpty(validateCode))
            {
                return Content("no:验证码错误");
            }
            Session["validateCode"] = null;
            string vCode = Request["vCode"];
            if (!validateCode.Equals(vCode, StringComparison.InvariantCultureIgnoreCase))
            {
                return Content("no:验证码错误");
            }
            //登录信息校验 可BLL层封装处理登录校验
            string userName = Request["LoginCode"];
            string userPwd = Request["LoginPwd"];

            var userInfo = UserInfoService.LoadEntities(t => t.UName == userName && t.UPwd == userPwd).FirstOrDefault();
            if (userInfo != null)
            {
                //Session["userInfo"] = userInfo;
                //产生一个guid作为memache的键
                string sessionId = Guid.NewGuid().ToString();
                //缓存工厂进行初始化Redis缓存调用 嵌套调用依然不能用泛型 只能string
                CacheFactory.Cache().WriteCache(SerializeHelper.SerializeToString(userInfo), sessionId, DateTime.Now.AddMinutes(20));
                //添加缓存信息
                //MemcacheHelper.Set(sessionId, SerializeHelper.SerializeToString(userInfo), DateTime.Now.AddMinutes(20));
                //将Memcache的key以Cookie的形式返回给浏览器。
                Response.Cookies["sessionId"].Value = sessionId;
                return Content("ok:登陆成功");
            }
            else
            {
                return Content("no:登陆失败");
            }
        }

        #endregion


        #region 显示验证码

        public ActionResult ShowValidateCode()
        {
            ValidateCode validateCode = new ValidateCode();
            //产生验证码
            string code = validateCode.CreateValidateCode(4);
            Session["validateCode"] = code;
            //将验证码画到画布上
            byte[] buffer = validateCode.CreateValidateGraphic(code);
            return File(buffer, "image/png");
        }

        #endregion

    }
}