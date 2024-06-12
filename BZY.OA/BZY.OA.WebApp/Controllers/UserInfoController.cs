using BZY.OA.BLL;
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
    public class UserInfoController : Controller
    {
        IUserInfoService userInfoService { get; set; }
        public ActionResult Index()
        {
            return View();
        }

        #region 获取用户列表数据

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
            var userInfoList = userInfoService.LoadSearchEntities(userInfoSearch, delFlag);
            //var userInfoList = userInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, t => t.DelFlag == delFlag, t => t.ID, true);

            var temp = from u in userInfoList
                       select new { u.ID, u.UName, u.UPwd, u.Remark, u.SubTime };
            return Json(new { rows = temp, total = userInfoSearch.TotalCount });
        }

        #endregion

        #region 删除数据

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
            bool isDelete = userInfoService.DeleteEntities(list);
            return Content(isDelete ? "ok" : "no");
        }

        #endregion

        #region 添加用户数据

        public ActionResult AddUserInfo(UserInfo userInfo)
        {
            userInfo.DelFlag = 0;
            userInfo.ModifiedOn = DateTime.Now;
            userInfo.SubTime = DateTime.Now;
            userInfoService.AddEntity(userInfo);
            return Content("ok");
        }

        #endregion

        #region 修改用户数据

        /// <summary>
        /// 展示修改用户数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowEditUserInfo()
        {
            int id = int.Parse(Request["id"]);
            var userInfo = userInfoService.LoadEntities(t => t.ID == id).FirstOrDefault();
            return Json(userInfo, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改用户数据
        /// </summary>
        /// <returns></returns>
        public ActionResult EditUserInfo(UserInfo userInfo)
        {
            userInfo.ModifiedOn = DateTime.Now;
            var isEidt = userInfoService.EditEntity(userInfo);
            return Content(isEidt ? "ok" : "no");
        }

        #endregion
    }
}