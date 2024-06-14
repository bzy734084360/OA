using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    /// <summary>
    /// 服务契约
    /// </summary>
    [ServiceContract]
    public interface IUserInfoService
    {
        /// <summary>
        /// 操作契约
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [OperationContract]
        int Add(int a, int b);
    }
}
