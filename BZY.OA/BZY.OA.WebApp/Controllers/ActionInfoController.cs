using BZY.OA.IBLL;
using BZY.OA.Model;
using BZY.OA.Model.EnumType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    public class ActionInfoController : BaseController
    {
        IActionInfoService ActionInfoService { get; set; }
        // GET: ActionInfo
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetActionInfoList()
        {
            int pageIndex = Request["page"] != null ? int.Parse(Request["page"]) : 1;
            int pageSize = Request["rows"] != null ? int.Parse(Request["rows"]) : 5;
            int totalCount;
            short delFlag = (short)DeleteEnumType.Normarl;
            var actionInfoList = ActionInfoService.LoadPageEntities<int>(pageIndex, pageSize, out totalCount, r => r.DelFlag == delFlag, r => r.ID, true);
            var temp = from r in actionInfoList
                       select new { ID = r.ID, ActionInfoName = r.ActionInfoName, Sort = r.Sort, SubTime = r.SubTime, Remark = r.Remark, Url = r.Url, ActionTypeEnum = r.ActionTypeEnum, HttpMethod = r.HttpMethod };
            return Json(new { rows = temp, total = totalCount }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取上传的文件
        /// </summary>
        /// <returns></returns>
        public ActionResult GetFileUp()
        {
            HttpPostedFileBase file = Request.Files["fileUp"];
            string fileName = Path.GetFileName(file.FileName);
            string fileExt = Path.GetExtension(fileName);
            if (fileExt == ".jpg")
            {
                string dir = "/ImageIcon/" + DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + "/";
                Directory.CreateDirectory(Path.GetDirectoryName(Request.MapPath(dir)));
                string newfileName = Guid.NewGuid().ToString();
                string fullDir = dir + newfileName + fileExt;
                file.SaveAs(Request.MapPath(fullDir));
                //自己加上图片的缩略图
                return Content("ok:" + fullDir);
            }
            else
            {
                return Content("no:文件类型错误!!");
            }
        }
        /// <summary>
        /// 权限添加
        /// </summary>
        /// <param name="actionInfo"></param>
        /// <returns></returns>
        public ActionResult AddActionInfo(ActionInfo actionInfo)
        {
            actionInfo.DelFlag = 0;
            actionInfo.ModifiedOn = DateTime.Now.ToString();
            actionInfo.SubTime = DateTime.Now;
            actionInfo.Url = actionInfo.Url.ToLower();
            ActionInfoService.AddEntity(actionInfo);
            return Content("ok");
        }
    }
}