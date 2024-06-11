using BZY.OA.IBLL;
using BZY.OA.IDAL;
using BZY.OA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.BLL
{
    /// <summary>
    /// UserInfo业务类
    /// </summary>
    public class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.UserInfoDal;
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            this.CurrentDBSession.UserInfoDal.AddEntity(userInfo);
            this.CurrentDBSession.UserInfoDal.DeleteEntity(userInfo);
            this.CurrentDBSession.UserInfoDal.EditEntity(userInfo);
            this.CurrentDBSession.SaveChanges();
        }
    }
}
