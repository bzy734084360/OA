using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace BZY.OA.Common
{

    /// <summary>
    /// 说明：作业调度帮助类
    /// </summary>
    public class JobHelper
    {

        #region 命名前缀(可以自行设置)

        /// <summary>
        /// 默认组名
        /// </summary>
        private static string DefaultGroupName = "Default";

        #endregion

        #region 初始化作业实体

        /// <summary>
        /// 获取JobKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static JobKey GetJobKey(string jobName, string jobGroupName)
        {
            return new JobKey(jobName, jobGroupName);
        }

        /// <summary>
        /// 获取TriggerKey
        /// </summary>
        /// <param name="triggerName"></param>
        /// <returns></returns>
        private static TriggerKey GetTriggerKey(string triggerName)
        {
            return new TriggerKey(triggerName, DefaultGroupName);
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// 调度器是否启动
        /// </summary>
        /// <returns></returns>
        public static bool SchedulerIsStart()
        {
            var sched = Scheduler.GetIntance();
            return sched.IsStarted && !sched.InStandbyMode;
        }

        /// <summary>
        /// 开启调度
        /// </summary>
        public static void StartScheduler()
        {
            var sched = Scheduler.GetIntance();
            if (!sched.IsStarted)
            {
                sched.Start();
            }
            if (sched.InStandbyMode)
            {
                sched.Start();
            }
        }

        /// <summary>
        /// 关闭调度：如有正在执行中的作业，
        /// </summary>
        public static void ShutdownScheduler()
        {
            var sched = Scheduler.GetIntance();
            if (sched.IsStarted && !sched.InStandbyMode)
            {
                sched.Standby();
            }
        }

        /// <summary>
        /// 创建作业，默认需提供一个触发器
        /// </summary>
        /// <param name="jobName">1-名称</param>
        /// <param name="jobDesc"></param>
        /// <param name="jobType"></param>
        /// <param name="jobGroup"></param>
        /// <param name="triggerID"></param>
        /// <param name="cronExpr"></param>
        /// <returns></returns>
        public static bool CreateJob(string jobName, string jobDesc, Type jobType, string jobGroup, string triggerID, string cronExpr)
        {
            var sched = Scheduler.GetIntance();
            IJobDetail job = (IJobDetail)sched.GetJobDetail(GetJobKey(jobName, jobGroup));
            if (!sched.IsStarted)
            {
                sched.Start();
            }
            if (job != null)
            {
                sched.ResumeJob(GetJobKey(jobName, jobGroup));
            }
            else
            {
                var jobData = new JobDataMap();
                IList<string> paras = jobDesc.Split('$');
                foreach (string para in paras)
                {
                    IList<string> resut = para.Split('=');
                    if (resut.Count == 2)
                    {
                        jobData.Put(resut[0], resut[1]);
                    }
                }
                IJobDetail detail = JobBuilder.Create(jobType)
                                              .WithIdentity(GetJobKey(jobName, jobGroup))
                                              .WithDescription(jobDesc)
                                              .SetJobData(jobData)
                                              .StoreDurably(true)//持久化
                                              .Build();
                ITrigger trigger = TriggerBuilder.Create()
                                                 .WithIdentity(GetTriggerKey(triggerID))
                                                 //.StartNow() 只适用简单触发器
                                                 .WithCronSchedule(cronExpr).ForJob(detail)
                                                 .Build();
                //Scheduler.GetIntance().ScheduleJob(detail, trigger);
                sched.AddJob(detail, false);
                sched.ScheduleJob(trigger);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 给作业添加触发器
        /// </summary>
        /// <param name="triggerID">触发器ID</param>
        /// <param name="cronExpr">cron表达式</param>
        /// <param name="jobName">作业名称</param>
        /// <param name="jobGroup">作业分组</param>
        /// <returns></returns>
        public static bool AddTriggerForJob(string triggerID, string cronExpr, string jobName, string jobGroup)
        {
            var sched = Scheduler.GetIntance();
            IJobDetail job = sched.GetJobDetail(GetJobKey(jobName, jobGroup));
            if (job == null)
            {
                throw new ApplicationException("任务不存在");
            }
            if (!sched.IsStarted)
            {
                sched.Start();
            }
            if (job != null)
            {
                //sched.ResumeJob(GetJobKey(jobName, jobGroup));
                var trigger = sched.GetTrigger(GetTriggerKey(triggerID));
                if (trigger != null)
                {
                    throw new ApplicationException("触发器ID已存在");
                }
                else
                {
                    ITrigger newTrigger = TriggerBuilder.Create()
                                                     .WithIdentity(GetTriggerKey(triggerID))
                                                     .WithCronSchedule(cronExpr).ForJob(job)
                                                     .Build();
                    sched.ScheduleJob(newTrigger);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 删除作业
        /// </summary>
        /// <param name="triggerID"></param>
        /// <returns></returns>
        public static bool DeleteJob(string jobName, string jobGroup)
        {
            var sched = Scheduler.GetIntance();
            IJobDetail detail = sched.GetJobDetail(GetJobKey(jobName, jobGroup));
            //if (detail != null && !Scheduler.GetIntance().IsJobGroupPaused(jobGroup))
            //{
            Scheduler.GetIntance().PauseJob(GetJobKey(jobName, jobGroup));
            Scheduler.GetIntance().DeleteJob(GetJobKey(jobName, jobGroup));
            //}
            return true;
        }

        /// <summary>
        /// 删除触发器
        /// </summary>
        /// <param name="triggerID"></param>
        /// <returns></returns>
        public static bool DeleteTrigger(string triggerID)
        {
            IScheduler sched = Scheduler.GetIntance();
            ITrigger trigger = sched.GetTrigger(GetTriggerKey(triggerID));
            if (trigger == null)
            {
                throw new ApplicationException("触发器不存在");
            }
            else
            {
                return sched.UnscheduleJob(GetTriggerKey(triggerID));
            }
        }

        /// <summary>
        /// 立即执行一次作业
        /// </summary>
        /// <param name="jobName"></param>
        public static void RunJob(string jobName, string jobGroup)
        {
            IScheduler sched = Scheduler.GetIntance();
            if (sched.GetJobDetail(GetJobKey(jobName, jobGroup)) == null)
            {
                throw new ApplicationException("任务不存在");
            }
            sched.TriggerJob(GetJobKey(jobName, jobGroup));
        }


        /// <summary>
        /// 重启作业
        /// </summary>
        /// <param name="jobName">作业名称</param>
        /// <returns></returns>
        public static bool ResumeJob(string jobName, string jobGroup)
        {
            IScheduler sched = Scheduler.GetIntance();
            IJobDetail job = sched.GetJobDetail(GetJobKey(jobName, jobGroup));
            //if (job != null && !Scheduler.GetIntance().IsJobGroupPaused(jobGroup))
            //{
            //    return true;
            //}
            if (!sched.IsStarted)
            {
                sched.Start();
            }
            if (job != null)
            {
                sched.ResumeJob(GetJobKey(jobName, jobGroup));
                return true;
            }
            return false;
        }

        /// <summary>
        /// 停止作业
        /// </summary>
        /// <returns></returns>
        public static bool StopJob(string jobName, string jobGroup)
        {
            IScheduler sched = Scheduler.GetIntance();
            IJobDetail detail = sched.GetJobDetail(GetJobKey(jobName, jobGroup));
            if (detail == null)
            {
                throw new ApplicationException("任务不存在");
            }
            //if (detail != null && Scheduler.GetIntance().IsJobGroupPaused(jobGroup))
            //{
            //    return true;
            //}
            sched.PauseJob(GetJobKey(jobName, jobGroup));
            return true;
        }

        /// <summary>
        /// 重启触发器
        /// </summary>
        /// <param name="triggerID"></param>
        /// <returns></returns>
        public static bool ResumeTrigger(string triggerID)
        {
            IScheduler sched = Scheduler.GetIntance();
            ITrigger trigger = sched.GetTrigger(GetTriggerKey(triggerID));
            if (trigger == null)
            {
                throw new ApplicationException("触发器不存在");
            }
            if (trigger != null)
            {
                sched.ResumeTrigger(GetTriggerKey(triggerID));
            }
            return true;
        }

        /// <summary>
        /// 暂停触发器
        /// </summary>
        /// <param name="triggerID"></param>
        /// <returns></returns>
        public static bool PauseTrigger(string triggerID)
        {
            IScheduler sched = Scheduler.GetIntance();
            ITrigger trigger = sched.GetTrigger(GetTriggerKey(triggerID));
            if (trigger == null)
            {
                throw new ApplicationException("触发器不存在");
            }
            if (trigger != null)
            {
                sched.PauseTrigger(GetTriggerKey(triggerID));
            }
            return true;
        }



        #endregion



        #region 取得作业运行状态

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triggerID"></param>
        /// <returns>正常 = 0,暂停 = 1,	完成 = 2,错误 = 3,锁定 = 4,	None = 5,</returns>
        public static int GetTriggerStatus(string triggerID)
        {
            TriggerState state = Scheduler.GetIntance().GetTriggerState(GetTriggerKey(triggerID));
            return (int)state;
        }

        #endregion
    }
}
