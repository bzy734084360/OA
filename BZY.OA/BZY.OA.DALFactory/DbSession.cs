using BZY.OA.DAL;
using BZY.OA.IDAL;
using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.DALFactory
{
    /// <summary>
    /// 数据会话层：工厂类，负责完成所有数据操作类的实力的创建，然后业务层通过数据会话层获来获取获取要操作的数据类的实力。
    /// 所以数据会话层将业务层与数据层解耦
    /// 在数据会话层中提供一个方法；完成所有数据的保存。
    /// </summary>
    public class DbSession : IDBSession
    {
        public DbContext Db
        {
            get
            {
                return DBContextFactory.CreateDbContext();
            }
        }
        private IUserInfoDal _UserInfoDal;
        public IUserInfoDal UserInfoDal
        {
            get
            {
                if (_UserInfoDal == null)
                {
                    //_UserInfoDal = new UserInfoDal();
                    //通过抽象工厂封装了类的实例的创建
                    _UserInfoDal = AbstractFactory.CreateUserInfoDal();
                }
                return _UserInfoDal;
            }
            set
            {
                _UserInfoDal = value;
            }
        }
        /// <summary>
        /// 一个业务中涉及到多张数据表的操作，我们希望连接一次数据库，完成对多张表数据的操作。
        /// 提高性能
        /// </summary>
        /// <returns></returns>
        public bool SaveChanges()
        {
            return Db.SaveChanges() > 0;
        }
    }
}
