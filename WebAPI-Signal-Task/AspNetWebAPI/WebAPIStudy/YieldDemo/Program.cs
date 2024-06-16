using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YieldDemo
{
    class Program
    {
        #region yield 学习

        //static void Main(string[] args)
        //{
        //    string[] strs = { "0", "2", "3", "4", "5", "6", "7" };

        //    //var ints = ConvertToInts(strs);
        //    //foreach (var item in ints)
        //    //{
        //    //    if (item > 5)
        //    //    {
        //    //        break;
        //    //    }
        //    //    Console.WriteLine("value{0};type:{1}", item, item.GetType());
        //    //}

        //    Console.ReadKey();
        //}
        //private async static IEnumerable<int> ConvertToInts(string[] strs)
        //{
        //    //IList<int> result = new List<int>();
        //    //foreach (var item in strs)
        //    //{
        //    //    Console.WriteLine("value{0};type:{1}", item, item.GetType());
        //    //    result.Add(int.Parse(item));
        //    //}
        //    //return result;


        //    //正常使用
        //    //return strs.Select(s => int.Parse(s));


        //    //yield
        //    //foreach (var item in strs)
        //    //{
        //    //    Console.WriteLine("value{0};type:{1}", item, item.GetType());
        //    //    //yield 适用于，当前方法返回一个特定类型的集合 弹 使用时弹出一个元素。
        //    //    yield return int.Parse(item);
        //    //}

        //    //使用场景
        //    using (SqlCommand command = new SqlCommand())
        //    {
        //        using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //        {
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    yield return 1;
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion

        static void Main(string[] args)
        {
            var mc = new MyClass();
            foreach (var item in mc)
            {
                Console.WriteLine(item);
            }

            //foreach 原理
            //var enumerator = mc.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    enumerator.Current;
            //}

            string[] strs = { "0", "2", "3", "4", "5", "6", "7" };

            //var enumerator = strs.GetEnumerator();
            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current);
            //}

            Console.ReadKey();
        }
    }

    public class MyClass
    {
        string[] strs = { "0", "2", "3", "4", "5", "6", "7" };

        public MyClassEnumer GetEnumerator()
        {
            return new MyClassEnumer(strs);
        }
    }
    public class MyClassEnumer
    {
        private string[] strs;
        private int index = -1;
        /// <summary>
        /// 让迭代器的指针 指向下一位
        /// </summary>
        /// <returns>有没有下一个元素</returns>
        public bool MoveNext()
        {
            return ++index < strs.Length;
        }
        public MyClassEnumer(string[] strs)
        {
            this.strs = strs;
        }
        public object Current { get { return strs[index]; } }
    }
}
