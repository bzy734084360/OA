using BZY.OA.IBLL;
using BZY.OA.Model;
using BZY.OA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        IUserInfoService UserInfoService { get; set; }
        // GET: Home
        public ActionResult Index()
        {
            ViewData["name"] = LoginUser.UName;
            return View();
        }
        public ActionResult HomePage()
        {
            return View();
        }

        /// <summary>
        /// 过滤登录用户的菜单权限
        /// 1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
        /// 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
        /// 3：将这两个集合合并成一个集合。
        /// 4：把禁止的权限从总的集合中清除。
        /// 5：将总的集合中的重复权限清除。
        /// 6：把过滤好的菜单权限生成JSON返回。
        /// </summary>
        /// <returns></returns>
        public ActionResult Getmenus()
        {
            //1: 可以按照用户---角色---权限这条线找出登录用户的权限，放在一个集合中。
            //获取登录用户的信息
            var userInfo = UserInfoService.LoadEntities(u => u.ID == LoginUser.ID).FirstOrDefault();
            //获取登录用户的角色.
            var userRoleInfo = userInfo.RoleInfo;
            //根据登录用户的角色获取对应的菜单权限。
            short actionTypeEnum = (short)ActionTypeEnum.MenumActionType;
            var loginUserMenuActions = (from r in userRoleInfo
                                        from a in r.ActionInfo
                                        where a.ActionTypeEnum == actionTypeEnum
                                        select a).ToList();

            // 2：可以按照用户---权限这条线找出用户的权限，放在一个集合中。
            var userActions = from a in userInfo.R_UserInfo_ActionInfo
                              select a.ActionInfo;

            var userMenuActions = (from a in userActions
                                   where a.ActionTypeEnum == actionTypeEnum
                                   select a).ToList();

            //3：将这两个集合合并成一个集合。
            loginUserMenuActions.AddRange(userMenuActions);

            //4：把禁止的权限从总的集合中清除。
            var forbidActions = (from a in userInfo.R_UserInfo_ActionInfo
                                 where a.IsPass == false
                                 select a.ActionInfoID).ToList();
            var loginUserAllowActions = loginUserMenuActions.Where(a => !forbidActions.Contains(a.ID));

            //5：将总的集合中的重复权限清除。
            var lastLoginUserActions = loginUserAllowActions.Distinct(new EqualityComparer());
            //6：把过滤好的菜单权限生成JSON返回。
            var temp = from a in lastLoginUserActions
                       select new { icon = a.MenuIcon, title = a.ActionInfoName, url = a.Url };
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

    }
}