using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Models;
using WebApp.Services.Helpers;

namespace WebApp.Services.Logging
{
    /// <summary>
    /// Default logger
    /// </summary>
    public partial class DefaultLogger : ILogger
    {
        #region Fields

        //private readonly IRepository<Log> _logRepository;
        private readonly IWebHelper _webHelper;
        private readonly SenContext _dbContext;
        //private readonly HttpContext _httpContext;
        //private readonly IDataProvider _dataProvider;
        //private readonly CommonSettings _commonSettings;
        
        #endregion

        #region Ctor

        ///// <summary>
        ///// Ctor
        ///// </summary>
        ///// <param name="logRepository">Log repository</param>
        ///// <param name="webHelper">Web helper</param>>
        ///// <param name="dbContext">DB context</param>>
        ///// <param name="dataProvider">WeData provider</param>
        ///// <param name="commonSettings">Common settings</param>
        //public DefaultLogger(IRepository<Log> logRepository, IWebHelper webHelper,
        //    IDbContext dbContext, IDataProvider dataProvider, CommonSettings commonSettings)
        //{
        //    this._logRepository = logRepository;
        //    this._webHelper = webHelper;
        //    this._dbContext = dbContext;
        //    this._dataProvider = dataProvider;
        //    this._commonSettings = commonSettings;
        //}

        public DefaultLogger(SenContext dbContext, HttpContextBase httpContext)
        {
            this._dbContext = dbContext;
            this._webHelper = new WebApp.Services.Helpers.WebHelper(httpContext);
        }

        public DefaultLogger()
        {
            SenContext dbContext = new SenContext();
            this._dbContext = dbContext;
            this._webHelper = new WebApp.Services.Helpers.WebHelper(null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether a log level is enabled
        /// </summary>
        /// <param name="level">Log level</param>
        /// <returns>Result</returns>
        public virtual bool IsEnabled(SenLogLevel level)
        {
            switch(level)
            {
                case SenLogLevel.Debug:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Deletes a log item
        /// </summary>
        /// <param name="log">Log item</param>
        public virtual void DeleteLog(SenLog log)
        {
            if (log == null)
                throw new ArgumentNullException("log");
            _dbContext.Entry(log).State = System.Data.Entity.EntityState.Deleted;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public virtual void ClearLog()
        {
            _dbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [SenLog]");

            //if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            //{
            //    //although it's not a stored procedure we use it to ensure that a database supports them
            //    //we cannot wait until EF team has it implemented - http://data.uservoice.com/forums/72025-entity-framework-feature-suggestions/suggestions/1015357-batch-cud-support
            //    //do all databases support "Truncate command"?
            //    //TODO: do not hard-code the table name
            //    _dbContext.ExecuteSqlCommand("TRUNCATE TABLE [Log]");
            //}
            //else
            //{
            //    var log = _logRepository.Table.ToList();
            //    foreach (var logItem in log)
            //        _logRepository.Delete(logItem);
            //}
        }

        ///// <summary>
        ///// Gets all log items
        ///// </summary>
        ///// <param name="fromUtc">Log item creation from; null to load all records</param>
        ///// <param name="toUtc">Log item creation to; null to load all records</param>
        ///// <param name="message">Message</param>
        ///// <param name="SenLogLevel">Log level; null to load all records</param>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageSize">Page size</param>
        ///// <returns>Log item collection</returns>
        //public virtual IPagedList<Log> GetAllLogs(DateTime? fromUtc, DateTime? toUtc,
        //    string message, SenLogLevel? SenLogLevel, int pageIndex, int pageSize)
        //{
        //    var query = _logRepository.Table;
        //    if (fromUtc.HasValue)
        //        query = query.Where(l => fromUtc.Value <= l.CreatedOnUtc);
        //    if (toUtc.HasValue)
        //        query = query.Where(l => toUtc.Value >= l.CreatedOnUtc);
        //    if (SenLogLevel.HasValue)
        //    {
        //        int SenLogLevelId = (int)SenLogLevel.Value;
        //        query = query.Where(l => SenLogLevelId == l.SenLogLevelId);
        //    }
        //     if (!String.IsNullOrEmpty(message))
        //        query = query.Where(l => l.ShortMessage.Contains(message) || l.FullMessage.Contains(message));
        //    query = query.OrderByDescending(l => l.CreatedOnUtc);

        //    var log = new PagedList<Log>(query, pageIndex, pageSize);
        //    return log;
        //}

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public virtual SenLog GetLogById(int logId)
        {
            if (logId == 0)
                return null;

            return _dbContext.SenLogs.SingleOrDefault(m => m.LogId == logId);
            //return _logRepository.GetById(logId);
        }

        /// <summary>
        /// Get log items by identifiers
        /// </summary>
        /// <param name="logIds">Log item identifiers</param>
        /// <returns>Log items</returns>
        public virtual IList<SenLog> GetLogByIds(int[] logIds)
        {
            if (logIds == null || logIds.Length == 0)
                return new List<SenLog>();

            //var query = from l in _logRepository.Table
            //            where logIds.Contains(l.Id)
            //            select l;

            var query = from l in _dbContext.SenLogs
                        where logIds.Contains(l.LogId)
                        select l;

            var logItems = query.ToList();
            //sort by passed identifiers
            var sortedLogItems = new List<SenLog>();
            foreach (int id in logIds)
            {
                var log = logItems.Find(x => x.LogId == id);
                if (log != null)
                    sortedLogItems.Add(log);
            }
            return sortedLogItems;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="SenLogLevel">Log level</param>
        /// <param name="shortMessage">The short message</param>
        /// <param name="fullMessage">The full message</param>
        /// <param name="customer">The customer to associate log record with</param>
        /// <returns>A log item</returns>
        public virtual SenLog InsertLog(SenLogLevel senloglevel, string shortMessage, string fullMessage = "", MembershipUser membershipuser = null)
        {

            Guid userid = Guid.Empty;
            if (membershipuser!=null) userid = (Guid)membershipuser.ProviderUserKey;
            
            var log = new SenLog()
            {
                LogLevelId = (int)senloglevel,
                ShortMessage = shortMessage,
                FullMessage = fullMessage,
                IpAddress = _webHelper.GetCurrentIpAddress(),
                UserId = userid,
                PageUrl = _webHelper.GetThisPageUrl(true),
                ReferrerUrl = _webHelper.GetUrlReferrer(),
                CreatedOnUtc = DateTime.UtcNow
            };

            _dbContext.SenLogs.Add(log);
            _dbContext.SaveChanges();

            return log;
        }

        #endregion
    }
}