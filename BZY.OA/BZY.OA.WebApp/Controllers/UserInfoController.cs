using BZY.OA.BLL;
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
    public class UserInfoController : Controller
    {
        IUserInfoService userInfoService = new UserInfoService();
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();
        }

        #region 获取用户列表数据

        public ActionResult GetUserInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount = 0;
            short delFlag = (short)DeleteEnumType.Normarl;
            var userInfoList = userInfoService.LoadPageEntities(pageIndex, pageSize, out totalCount, t => t.DelFlag == delFlag, t => t.ID, true);

            var temp = from u in userInfoList
                       select new { u.ID, u.UName, u.UPwd, u.Remark, u.SubTime };
            return Json(new { rows = temp, total = totalCount });
        }

        #endregion
    }
}