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
    public class RoleInfoController : BaseController
    {
        IRoleInfoService RoleInfoService { get; set; }
        IActionInfoService ActionInfoService { get; set; }
        // GET: RoleInfo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            short delFlag = (short)DeleteEnumType.Normarl;
            var roleInfoList = RoleInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, r => r.DelFlag == delFlag, r => r.ID, true);
            var temp = from r in roleInfoList
                       select new { ID = r.ID, RoleName = r.RoleName, Sort = r.Sort, SubTime = r.SubTime, Remark = r.Remark };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 展示角色添加列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRoleAddInfo()
        {
            return View();
        }
        /// <summary>
        /// 角色添加
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRoleInfo(RoleInfo roleInfo)
        {
            roleInfo.ModifiedOn = DateTime.Now;
            roleInfo.SubTime = DateTime.Now;
            roleInfo.DelFlag = 0;
            RoleInfoService.AddEntity(roleInfo);
            return Content("ok");
        }
        /// <summary>
        /// 展示要分配的权限
        /// </summary>
        /// <returns></returns>
        public ActionResult ShowRoleAction()
        {
            int id = int.Parse(Request["id"]);
            var roleInfo = RoleInfoService.LoadEntities(r => r.ID == id).FirstOrDefault();//获取要分配的权限的角色信息。
            ViewBag.RoleInfo = roleInfo;
            //获取所有的权限。
            short delFlag = (short)DeleteEnumType.Normarl;
            var actionInfoList = ActionInfoService.LoadEntities(a => a.DelFlag == delFlag).ToList();
            //要分配权限的角色以前有哪些权限。
            var actionIdList = (from a in roleInfo.ActionInfo
                                select a.ID).ToList();
            ViewBag.ActionInfoList = actionInfoList;
            ViewBag.ActionIdList = actionIdList;
            return View();
        }
        /// <summary>
        /// 完成角色权限的分配
        /// </summary>
        /// <returns></returns>
        public ActionResult SetRoleAction()
        {
            int roleId = int.Parse(Request["roleId"]);//获取角色编号
            string[] allKeys = Request.Form.AllKeys;//获取所有表单元素name属性的值。
            List<int> list = new List<int>();
            foreach (string key in allKeys)
            {
                if (key.StartsWith("cba_"))
                {
                    string k = key.Replace("cba_", "");
                    list.Add(Convert.ToInt32(k));
                }
            }
            if (RoleInfoService.SetRoleActionInfo(roleId, list))
            {
                return Content("ok");
            }
            else
            {
                return Content("no");
            }
        }

    }
}