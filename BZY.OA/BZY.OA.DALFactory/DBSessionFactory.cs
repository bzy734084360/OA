using BZY.OA.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace BZY.OA.DALFactory
{
    public class DBSessionFactory
    {
        public static IDBSession CreateDBSession()
        {
            IDBSession dBSession = (IDBSession)CallContext.GetData("dbSession");
            if (dBSession == null)
            {
                dBSession = new DbSession();
                CallContext.SetData("dbSession", dBSession);
            }
            return dBSession;
        }
    }
}
