using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            UserInfoServiceClient client = new UserInfoServiceClient();
            int sum = client.Add(3, 6);
            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
