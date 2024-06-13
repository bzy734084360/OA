using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BZY.OA.IDAL;
using System.Reflection;

namespace BZY.OA.DALFactory
{
    /// <summary>
    /// 抽象工厂类
    /// </summary>
    public partial class AbstractFactory
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        /// <summary>
        /// 命名空间
        /// </summary>
        private static readonly string NameSpace = ConfigurationManager.AppSettings["NameSpace"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static object CreateInstance(string className)
        {
            var assembly = Assembly.Load(AssemblyPath);
            return assembly.CreateInstance(className);
        }
    }
}
