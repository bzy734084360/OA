using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebAPIStudy.Models;

namespace WebApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync();
            Console.WriteLine("我是主线程执行的啦");
            Console.ReadKey();
        }
        static async void RunAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:57240/api/");
                //client.DefaultRequestHeaders.aC
                var result = await client.GetAsync("students/get");
                var list = await result.Content.ReadAsAsync<IEnumerable<Student>>();
                foreach (var item in list)
                {
                    Console.WriteLine(item.Name);
                }
            }
        }
    }
}
