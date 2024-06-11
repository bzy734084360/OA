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
    public class AbstractFactory
    {
        /// <summary>
        /// 程序集名称
        /// </summary>
        private static readonly string AssemblyPath = ConfigurationManager.AppSettings["AssemblyPath"];
        /// <summary>
        /// 命名空间
        /// </summary>
        private static readonly string NameSpace = ConfigurationManager.AppSettings["NameSpace"];
        public static IUserInfoDal CreateUserInfoDal()
        {
            string fullClassName = NameSpace + ".UserInfoDal";
            return CreateInstacne(fullClassName) as IUserInfoDal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        private static object CreateInstacne(string className)
        {
            var assembly = Assembly.Load(AssemblyPath);
            return assembly.CreateInstance(className);
        }
    }
}
