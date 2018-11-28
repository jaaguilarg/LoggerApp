using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerApp.DataAccess
{
    public class LogDAO : SqlServerBase
    {
        public LogDAO()
        {
        }

        public void InsertLogMessage(string strMessage, string type, Guid  logId)
        {
            try
            {
                BeginTransaction();
                CreateCommand("spInsertLog");
                CreateInputParameter("LogId", logId);
                CreateInputParameter("EventDate", DateTime.Now);
                CreateInputParameter("Description", strMessage);
                CreateInputParameter("Type", type);

                int result = ExecuteNonQuery();
                if (result != 0)
                {
                    CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                this.RollbackTransaction();
                throw ex;
            }
        }
    }
}
