using BZY.OA.BLL;
using BZY.OA.Common;
using BZY.OA.IBLL;
using BZY.OA.Model;
using BZY.OA.Model.EnumType;
using BZY.OA.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    public class UserInfoController : BaseController
    {
        IUserInfoService UserInfoService { get; set; }
        IRoleInfoService RoleInfoService { get; set; }
        IActionInfoService ActionInfoService { get; set; }
        IR_UserInfo_ActionInfoService R_UserInfo_ActionInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        #region 用户数据维护

        /// <summary>
        /// 获取用户列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            //接受搜索条件
            string userName = Request["name"];
            string userRemark = Request["remark"];
            //int totalCount = 0;
            short delFlag = (short)DeleteEnumType.Normarl;
            UserInfoSearch userInfoSearch = new UserInfoSearch()
            {
                UserName = userName,
                UserRemark = userRemark,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var userInfoList = UserInfoService.LoadSearchEntities(userInfoSearch, delFlag);
            //var userInfoList = userInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, t => t.DelFlag == delFlag, t => t.ID, true);

            var temp = from u in userInfoList
                       select new { u.ID, u.UName, u.UPwd, u.Remark, u.SubTime };
            return Json(new { rows = temp, total = userInfoSearch.TotalCount });
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteUserInfo()
        {
            string strId = Request["strId"];
            string[] strIds = strId.Split(',');
            List<int> list = new List<int>();
            foreach (string item in strIds)
            {
                list.Add(Convert.ToInt32(item));
            }
            //将List集合存储的要删除的记录数据传递到业务层
            bool isDelete = UserInfoService.DeleteEntities(list);
            return Content(isDelete ? "ok" : "no");
        }
        /// <summary>
        /// 添加用户数据
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfo.SubTime = DateTime.Now;
            UserInfoService.AddEntity(userInfo);
            return Content("ok");
        }
        /// <summary>
        /// 展示修改用户数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowEditUserInfo()
        {
            int id = int.Parse(Request["id"]);
            var userInfo = UserInfoService.LoadEntities(t => t.ID == id).FirstOrDefault();
            string userInfoJson = JsonHelper.ToJson(userInfo, false, "yyyy-MM-dd HH:mm:ss");

            return Content(userInfoJson);
        }
        /// <summary>
        /// 修改用户数据
        /// </summary>
        /// <returns></returns>
        public ActionResult EditUserInfo(UserInfo userInfo)
        {
            userInfo.ModifiedOn = DateTime.Now;
            var isEidt = UserInfoService.EditEntity(userInfo);
            return Content(isEidt ? "ok" : "no");
        }

        #endregion

        #region 用户与角色数据维护

        /// <summary>
        /// 展示用户已有角色信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowUserRoleInfo()
        {
            int id = int.Parse(Request["id"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == id).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //查询所有的角色.
            short delFlag = (short)DeleteEnumType.Normarl;
            var allRoleList = RoleInfoService.LoadEntities(r => r.DelFlag == delFlag).ToList();
            //查询一下要分配角色的用户以前具有了哪些角色编号。
            var allUserRoleIdList = (from r in userInfo.RoleInfo
                                     select r.ID).ToList();
            ViewBag.AllRoleList = allRoleList;
            ViewBag.AllUserRoleIdList = allUserRoleIdList;
            return View();
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserRoleInfo()
        {
            int userId = int.Parse(Request["userId"]);
            string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性值。
            List<int> roleIdList = new List<int>();
            foreach (string key in allKeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_", "");
                    roleIdList.Add(Convert.ToInt32(k));
                }
            }
            if (UserInfoService.SetUserRoleInfo(userId, roleIdList))//设置用户的角色
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

        #endregion

        #region 用户与权限维护

        /// <summary>
        /// 展示用户权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowUserAction()
        {
            int userId = int.Parse(Request["userId"]);
            var userInfo = UserInfoService.LoadEntities(u => u.ID == userId).FirstOrDefault();
            ViewBag.UserInfo = userInfo;
            //获取所有的权限。
            short delFlag = (short)DeleteEnumType.Normarl;
            var allActionList = ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //获取要分配的用户已经有的权限。
            var allActionIdList = (from a in userInfo.R_UserInfo_ActionInfo
                                   select a).ToList();
            ViewBag.AllActionList = allActionList;
            ViewBag.AllActionIdList = allActionIdList;
            return View();
        }
        /// <summary>
        /// 用户单个权限的分配
        /// </summary>
        /// <returns></returns>
        public ActionResult SetUserAction()
        {
            int actionId = int.Parse(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            bool isPass = Request["isPass"] == "true" ? true : false;
            if (UserInfoService.SetUserActionInfo(actionId, userId, isPass))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }
        /// <summary>
        /// 权限删除
        /// </summary>
        /// <returns></returns>
        public ActionResult ClearUserAction()
        {
            int actionId = int.Parse(Request["actionId"]);
            int userId = int.Parse(Request["userId"]);
            var r_userInfo_actionInfo = R_UserInfo_ActionInfoService.LoadEntities(r => r.ActionInfoID == actionId && r.UserInfoID == userId).FirstOrDefault();
            if (r_userInfo_actionInfo != null)
            {
                if (R_UserInfo_ActionInfoService.DeleteEntity(r_userInfo_actionInfo))
                {
                    return Content("ok:删除成功!!");
                }
                else
                {
                    return Content("ok:删除失败!!");
                }
            }
            else
            {
                return Content("no:数据不存在!!");
            }

        }

        #endregion
    }
}