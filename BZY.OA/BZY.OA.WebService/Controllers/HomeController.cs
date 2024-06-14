using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BZY.OA.WebService.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 异步调用核心未理解 意义 如何体现吞吐量
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            //调用WebService
            ServiceReference1.StudyWebServiceSoapClient client = new ServiceReference1.StudyWebServiceSoapClient();
            //int sum = client.AddAsync(3, 6);
            var task = client.LoadUserInfoListAsync();
            var result = await task;

            ViewData["sum"] = result.Body.LoadUserInfoListResult;
            ServiceReference2.WeatherWSSoapClient client2 = new ServiceReference2.WeatherWSSoapClient();
            DataSet ds = client2.getRegionDataset();
            ViewData["city"] = ds.Tables[0];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}