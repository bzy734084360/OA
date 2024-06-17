using BZY.OA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace BZY.OA.WebApp
{
    /// <summary>
    /// StudyWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class StudyWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        [WebMethod]
        public int Add(int a, int b)
        {
            return a + b;
        }
        [WebMethod]
        public string LoadUserInfoList()
        {
            //该方式解耦
            //IApplicationContext ctx = ContextRegistry.GetContext();
            //IUserInfoService userInfoService = (IUserInfoService)ctx.GetObject("UserInfoService");
            IBLL.IUserInfoService userInfoService = new BLL.UserInfoService();

            List<Model.UserInfo> list = userInfoService.LoadEntities(t => true).ToList();
            Thread.Sleep(3000);
            return JsonHelper.ToJson(list);
        }
    }
}
