using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(UserInfoService)))
            {
                host.Open();
                Console.WriteLine("服务启动成功");
                Console.ReadKey();
            }
        }
    }
}
