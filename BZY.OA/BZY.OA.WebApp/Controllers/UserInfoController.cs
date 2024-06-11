using BZY.OA.BLL;
using BZY.OA.IBLL;
using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebApp.Controllers
{
    public class UserInfoController : Controller
    {
        IUserInfoService bll = new UserInfoService();
        // GET: UserInfo
        public ActionResult Index()
        {
            //bll.LoadEntities(t => t.ID == 2);
            bll.AddEntity(new UserInfo());
            return View();
        }
    }
}