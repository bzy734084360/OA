using BZY.OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.IBLL
{
    public interface IBaseService<T> where T : class, new()
    {
        IDBSession CurrentDBSession { get; }
        IBaseDal<T> CurrentDal { get; set; }
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        T AddEntity(T entity);
        IQueryable<T> LoadPageEntities<O>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, O>> orderbyLambda, bool isAsc);
        bool DeleteEntity(T entity);
        bool EditEntity(T entity);
    }
}
