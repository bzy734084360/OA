using BZY.OA.BLL;
using BZY.OA.IBLL;
using Quartz;
using System;
using System.Collections.Specialized;

namespace BZY.OA.WebApp.JobScheduler
{
    /// <summary>
    /// 定时统计热词
    /// </summary>
    [DisallowConcurrentExecution]
    public class SyncHotKeyWordsInfoJob : IJob
    {
        IKeyWordsRankService bll = new KeyWordsRankService();
        /// <summary>
        /// 将明细表中的数据插入到汇总表中。
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            //做定时任务
            bll.DeleteAllKeyWordsRank();
            bll.InsertKeyWordsRank();
        }
    }
}