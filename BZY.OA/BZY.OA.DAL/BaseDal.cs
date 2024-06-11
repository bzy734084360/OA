using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.DAL
{
    public class BaseDal<T> where T : class, new()
    {
        OAEntities Db = new OAEntities();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">所新增的实体</param>
        /// <returns></returns>
        public T AddEntity(T entity)
        {
            Db.Set<T>().Add(entity);
            Db.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">所删除的实体</param>
        /// <returns></returns>
        public bool DeleteEntity(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
            return Db.SaveChanges() > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">所更新的实体</param>
        /// <returns></returns>
        public bool EditEntity(T entity)
        {
            Db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            return Db.SaveChanges() > 0;
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="whereLambda">lambda表达式处理数据</param>
        /// <returns>延迟加载</returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return Db.Set<T>().Where(whereLambda);
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
            var temp = Db.Set<T>().Where(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                temp = temp.OrderBy(orderbyLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending(orderbyLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return temp;
        }
    }
}
