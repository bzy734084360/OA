using BZY.OA.DALFactory;
using BZY.OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.BLL
{
    public abstract class BaseService<T> where T : class, new()
    {
        /// <summary>
        /// 数据会话对象
        /// </summary>
        public IDBSession CurrentDBSession
        {
            get
            {
                //return new DbSession();
                return DBSessionFactory.CreateDBSession();
            }
        }
        public IBaseDal<T> CurrentDal { get; set; }
        public abstract void SetCurrentDal();
        public BaseService()
        {
            //子类一定要实现抽象方法。
            SetCurrentDal();
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="whereLambda">lambda表达式处理数据</param>
        /// <returns>延迟加载</returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return CurrentDal.LoadEntities(whereLambda);
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">所新增的实体</param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            CurrentDal.AddEntity(entity);
            CurrentDBSession.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <typeparam name="O">排序条件所用的类型</typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="totalCount">总数据</param>
        /// <param name="whereLambda">过滤条件</param>
        /// <param name="orderbyLambda">排序条件</param>
        /// <param name="isAsc">升降序</param>
        /// <returns></returns>
        public IQueryable<T> LoadPageEntities<O>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, O>> orderbyLambda, bool isAsc)
        {
            return CurrentDal.LoadPageEntities(pageIndex, pageSize, out totalCount, whereLambda, orderbyLambda, isAsc);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">所删除的实体</param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            CurrentDal.DeleteEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">所更新的实体</param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            CurrentDal.EditEntity(entity);
            return CurrentDBSession.SaveChanges();
        }
    }
}
