using System.Collections.Generic;
using System.Web.Security;
using WebApp.Models;

namespace WebApp.Services.Logging
{
    /// <summary>
    /// Null logger
    /// </summary>
    public partial class NullLogger : ILogger
    {
        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public bool IsEnabled(SenLogLevel level)
        {
            return false;
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public void DeleteLog(SenLog log)
        {
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public void ClearLog()
        {
        }

        ///// <summary>
        ///// Gets all log items
        ///// </summary>
        ///// <param name="fromUtc">Log item creation from; null to load all records</param>
        ///// <param name="toUtc">Log item creation to; null to load all records</param>
        ///// <param name="message">Message</param>
        ///// <param name="logLevel">Log level; null to load all records</param>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageSize">Page size</param>
        ///// <returns>Log item collection</returns>
        //public IPagedList<SenLog> GetAllLogs(DateTime? fromUtc, DateTime? toUtc,
        //    string message, SenLogLevel? logLevel, int pageIndex, int pageSize)
        //{
        //    return new PagedList<SenLog>(new List<SenLog>(), pageIndex, pageSize);
        //}

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public SenLog GetLogById(int logId)
        {
            return null;
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<SenLog> GetLogByIds(int[] logIds)
        {
            return new List<SenLog>();
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="User">The User to associate log record with</param>
        /// <returns>A log item</returns>
        public SenLog InsertLog(SenLogLevel logLevel, string shortMessage, string fullMessage = "", MembershipUser membershipuser = null)
        {
            return null;
        }
    }
}
