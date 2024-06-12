using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.SpringNet
{
    public class UserInfoService : IUserInfoService
    {
        public string UserName { get; set; }
        public Person PersonR { get; set; }
        public string ShowMsg() 
        {
            return "Hello Word" + UserName + ":年龄是：" + PersonR.Age;
        }
    }
}
