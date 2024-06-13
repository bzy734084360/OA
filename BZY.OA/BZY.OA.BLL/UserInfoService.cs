using BZY.OA.IBLL;
using BZY.OA.IDAL;
using BZY.OA.Model;
using BZY.OA.Model.Search;
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
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
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
        /// <summary>
        /// 查询用户信息-包含分页
        /// </summary>
        /// <param name="userInfoSearch"></param>
        /// <param name="delFlag"></param>
        /// <returns></returns>
        public IQueryable<UserInfo> LoadSearchEntities(UserInfoSearch userInfoSearch, short delFlag)
        {
            var temp = this.CurrentDBSession.UserInfoDal.LoadEntities(t => t.DelFlag == delFlag);
            if (!string.IsNullOrEmpty(userInfoSearch.UserName))
            {
                temp = temp.Where(t => t.UName.Contains(userInfoSearch.UserName));
            }
            if (!string.IsNullOrEmpty(userInfoSearch.UserRemark))
            {
                temp = temp.Where(t => t.Remark.Contains(userInfoSearch.UserRemark));
            }
            userInfoSearch.TotalCount = temp.Count();
            return temp.OrderBy(t => t.ID).Skip((userInfoSearch.PageIndex - 1) * userInfoSearch.PageSize).Take(userInfoSearch.PageSize);
        }
        /// <summary>
        /// 为角色分配权限
        /// </summary>
        /// <param name="roleId">角色编号</param>
        /// <param name="actionIdList">权限编号列表</param>
        /// <returns></returns>
        public bool SetUserRoleInfo(int roleId, List<int> actionIdList)
        {
            //获取用户信息.
            var userInfo = this.CurrentDBSession.UserInfoDal.LoadEntities(r => r.ID == roleId).FirstOrDefault();
            if (userInfo != null)
            {
                userInfo.RoleInfo.Clear();
                foreach (int actionId in actionIdList)
                {
                    var roleInfo = this.CurrentDBSession.RoleInfoDal.LoadEntities(a => a.ID == actionId).FirstOrDefault();
                    userInfo.RoleInfo.Add(roleInfo);
                }
                return this.CurrentDBSession.SaveChanges();
            }
            return false;
        }
        /// <summary>
        /// 完成用户权限的分配
        /// </summary>
        /// <param name="actionId"></param>
        /// <param name="userId"></param>
        /// <param name="isPass"></param>
        /// <returns></returns>
        public bool SetUserActionInfo(int actionId, int userId, bool isPass)
        {
            //判断userId以前是否有了该actionId,如果有了只需要修改isPass状态，否则插入。
            var r_userInfo_actionInfo = this.CurrentDBSession.R_UserInfo_ActionInfoDal.LoadEntities(a => a.ActionInfoID == actionId && a.UserInfoID == userId).FirstOrDefault();
            if (r_userInfo_actionInfo == null)
            {
                R_UserInfo_ActionInfo userInfoActionInfo = new R_UserInfo_ActionInfo();
                userInfoActionInfo.ActionInfoID = actionId;
                userInfoActionInfo.UserInfoID = userId;
                userInfoActionInfo.IsPass = isPass;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.AddEntity(userInfoActionInfo);
            }
            else
            {
                r_userInfo_actionInfo.IsPass = isPass;
                this.CurrentDBSession.R_UserInfo_ActionInfoDal.EditEntity(r_userInfo_actionInfo);
            }
            return this.CurrentDBSession.SaveChanges();

        }
    }
}
