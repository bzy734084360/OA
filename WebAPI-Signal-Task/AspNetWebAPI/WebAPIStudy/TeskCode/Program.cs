using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeskCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //传统同步编程
            //Console.WriteLine("Main Start...");
            ////模拟延迟操作
            //Thread.Sleep(3000);
            //Console.WriteLine("Main End...");
            //Console.ReadKey();

            //旧版本 .net 4.5之前 使用以下方式
            ////异步编程形式
            //Console.WriteLine("Main Start...");
            ////耗时操作交给一个新线程去做。
            //Thread thread = new Thread(() =>
            //{
            //    //子线程做的事情
            //    Console.WriteLine("Do Start...");
            //    Thread.Sleep(3000);
            //    Console.WriteLine("Do End...");
            //});
            //thread.Start();
            //Console.WriteLine("Main End...");
            //Console.ReadKey();


            //4.5过后 内置 await 多了异步的新特性 基本应用

            //Console.WriteLine("Main Start...threadId" + Thread.CurrentThread.ManagedThreadId);
            ////耗时操作交给一个新线程去做。
            ////Thread thread = new Thread(() =>
            ////{
            ////    //子线程做的事情
            ////    Console.WriteLine("Do Start...");
            ////    Thread.Sleep(3000);
            ////    Console.WriteLine("Do End...");
            ////});
            ////thread.Start();
            //Do();
            //Console.WriteLine("Main End...threadId" + Thread.CurrentThread.ManagedThreadId);
            //Console.ReadKey();

            //进行使用方法：
            Console.WriteLine("Main Start TID:" + Thread.CurrentThread.ManagedThreadId);
            Do2();
            Console.WriteLine("Main End TID:" + Thread.CurrentThread.ManagedThreadId);
            //Console.WriteLine("Main Start TID:" + Thread.CurrentThread.ManagedThreadId);
            Console.ReadKey();
        }
        async static void Do()
        {
            Console.WriteLine("Do Start...threadId:" + Thread.CurrentThread.ManagedThreadId);
            //await 要求方法必须为异步方法 也就是方法要添加关键词 async
            //异步中暂停程序的方式 不要使用Thread.Sleep 而是使用Task.Delay(3000);
            await Task.Delay(3000);
            Console.WriteLine("Do End...threadId:" + Thread.CurrentThread.ManagedThreadId);
        }
        async static void Do2()
        {
            Console.WriteLine("Do2 Start...TID" + Thread.CurrentThread.ManagedThreadId);
            //调用带有返回参数的异步方法
            var date = await Do3();
            //输出异步方法中的值
            Console.WriteLine(date.ToString());
            Console.WriteLine("Do2 End...TID" + Thread.CurrentThread.ManagedThreadId);
        }
        /// <summary>
        /// 异步方法带有返回值的情况下 必须返回Task<T> T 返回类型
        /// </summary>
        /// <returns></returns>
        async static Task<DateTime> Do3()
        {
            Console.WriteLine("Do3 Start...TID" + Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(3000);
            Console.WriteLine("Do3 End...TID" + Thread.CurrentThread.ManagedThreadId);

            return DateTime.Now;
        }
    }
}
