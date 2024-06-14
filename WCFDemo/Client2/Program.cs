using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client2
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference2.UserInfoServiceClient client = new ServiceReference2.UserInfoServiceClient();
            int sum = client.Add(3, 6);
            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
