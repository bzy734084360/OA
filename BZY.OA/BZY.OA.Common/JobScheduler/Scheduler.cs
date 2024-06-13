using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Quartz.Impl;

namespace BZY.OA.Common
{
    /// <summary>
    /// 调度器
    /// </summary>
    public class Scheduler
    {
        private static IScheduler _instance;

        private Scheduler()
        {
        }
        /// <summary>
        /// 获得本类实例的唯一全局访问点。
        /// </summary>
        /// <returns></returns>
        public static IScheduler GetIntance()
        {
            if (_instance == null)
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _instance = schedFact.GetScheduler();
            }
            return _instance;
        }

    }
}
