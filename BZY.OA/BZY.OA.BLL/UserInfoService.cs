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
        /// <summary>
        /// 批量删除多条用户
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteEntities(List<int> list)
        {
            //去数据库中查询出要删除数据的ef对象
            var userInfoList = this.CurrentDBSession.UserInfoDal.LoadEntities(t => list.Contains(t.ID));
            foreach (var item in userInfoList)
            {
                this.CurrentDBSession.UserInfoDal.DeleteEntity(item);
            }
            return this.CurrentDBSession.SaveChanges();
        }

        public override void SetCurrentDal()
        {
            this.CurrentDal = this.CurrentDBSession.UserInfoDal;
        }
        //public void SetUserInfo(UserInfo userInfo)
        //{
        //    this.CurrentDBSession.UserInfoDal.AddEntity(userInfo);
        //    this.CurrentDBSession.UserInfoDal.DeleteEntity(userInfo);
        //    this.CurrentDBSession.UserInfoDal.EditEntity(userInfo);
        //    this.CurrentDBSession.SaveChanges();
        //}
    }
}
