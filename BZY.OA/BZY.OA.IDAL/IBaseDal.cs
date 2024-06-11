using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.IDAL
{
    /// <summary>
    /// 公共接口类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseDal<T> where T : class, new()
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="whereLambda">lambda表达式处理数据</param>
        /// <returns>延迟加载</returns>
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
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
        IQueryable<T> LoadPageEntities<O>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, O>> orderbyLambda, bool isAsc);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">所删除的实体</param>
        /// <returns></returns>
        bool DeleteEntity(T entity);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">所更新的实体</param>
        /// <returns></returns>
        bool EditEntity(T entity);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">所新增的实体</param>
        /// <returns></returns>
        T AddEntity(T entity);
    }
}
