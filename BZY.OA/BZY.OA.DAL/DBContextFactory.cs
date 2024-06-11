using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.DAL
{
    /// <summary>
    /// 负责创建EF数据操作上下文实力，必须保证线程内唯一
    /// </summary>
    public class DBContextFactory
    {
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <returns></returns>
        public static DbContext CreateDbContext()
        {
            //CallContext  线程槽。隔离机制 两个线程间是隔离的
            DbContext dbContext = (DbContext)CallContext.GetData("dbContext");
            if (dbContext == null)
            {
                dbContext = new OAEntities();
                CallContext.SetData("dbContext", dbContext);
            }
            return dbContext;
        }
    }
}
