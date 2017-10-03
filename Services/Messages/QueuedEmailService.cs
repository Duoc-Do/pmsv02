using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
//using System.Linq.Dynamic;
using WebApp.Models;
//using WebApp.Services.Events;

namespace WebApp.Services.Messages
{
    public partial class QueuedEmailService : IQueuedEmailService
    {
        //private readonly DbSet<SenQueuedEmail> _queuedEmailRepository;

        private readonly SenContext _db;
        //private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="queuedEmailRepository">Queued email repository</param>
        /// <param name="eventPublisher">Event published</param>
        //public QueuedEmailService(SenContext db, IEventPublisher eventPublisher)
        //{
        //    this._db = db;
        //    this._eventPublisher = eventPublisher;
        //}

        public QueuedEmailService(SenContext db)
        {
            this._db = db;
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>        
        public virtual void InsertQueuedEmail(SenQueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");

            //_queuedEmailRepository.Insert(queuedEmail);
            _db.SenQueuedEmails.Add(queuedEmail);
            _db.SaveChanges();

            ////event notification
            //_eventPublisher.EntityInserted(queuedEmail);
        }

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        public virtual void UpdateQueuedEmail(SenQueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");

            //_queuedEmailRepository.Update(queuedEmail);
            _db.Entry(queuedEmail).State = EntityState.Modified;
            _db.SaveChanges();
            ////event notification
            //_eventPublisher.EntityUpdated(queuedEmail);
        }

        /// <summary>
        /// Deleted a queued email
        /// </summary>
        /// <param name="queuedEmail">Queued email</param>
        public virtual void DeleteQueuedEmail(SenQueuedEmail queuedEmail)
        {
            if (queuedEmail == null)
                throw new ArgumentNullException("queuedEmail");

            //_queuedEmailRepository.Delete(queuedEmail);
            _db.Entry(queuedEmail).State = EntityState.Deleted;

            ////event notification
            //_eventPublisher.EntityDeleted(queuedEmail);
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="queuedEmailId">Queued email identifier</param>
        /// <returns>Queued email</returns>
        public virtual SenQueuedEmail GetQueuedEmailById(int queuedEmailId)
        {
            if (queuedEmailId == 0)
                return null;

            //return _queuedEmailRepository.GetById(queuedEmailId);
            return _db.SenQueuedEmails.SingleOrDefault(m => m.QueuedEmailId == queuedEmailId);

        }

        /// <summary>
        /// Get queued emails by identifiers
        /// </summary>
        /// <param name="queuedEmailIds">queued email identifiers</param>
        /// <returns>Queued emails</returns>
        public virtual IList<SenQueuedEmail> GetQueuedEmailsByIds(int[] queuedEmailIds)
        {
            if (queuedEmailIds == null || queuedEmailIds.Length == 0)
                return new List<SenQueuedEmail>();

            var query = from qe in _db.SenQueuedEmails
                        where queuedEmailIds.Contains(qe.QueuedEmailId)
                        select qe;
            var queuedEmails = query.ToList();
            //sort by passed identifiers
            var sortedQueuedEmails = new List<SenQueuedEmail>();
            foreach (int id in queuedEmailIds)
            {
                var queuedEmail = queuedEmails.Find(x => x.QueuedEmailId == id);
                if (queuedEmail != null)
                    sortedQueuedEmails.Add(queuedEmail);
            }
            return sortedQueuedEmails;
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="fromEmail">From Email</param>
        /// <param name="toEmail">To Email</param>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="maxSendTries">Maximum send tries</param>
        /// <param name="loadNewest">A value indicating whether we should sort queued email descending; otherwise, ascending.</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Email item list</returns>
        public virtual IList<SenQueuedEmail> SearchEmails(string fromEmail,
            string toEmail, DateTime? createdFromUtc, DateTime? createdToUtc,
            bool loadNotSentItemsOnly, int maxSendTries,
            bool loadNewest, int pageIndex, int pageSize)
        {
            fromEmail = (fromEmail ?? String.Empty).Trim();
            toEmail = (toEmail ?? String.Empty).Trim();

            var query = _db.SenQueuedEmails.AsQueryable();
            if (!String.IsNullOrEmpty(fromEmail))
                query = query.Where(qe => qe.From.Contains(fromEmail));
            if (!String.IsNullOrEmpty(toEmail))
                query = query.Where(qe => qe.To.Contains(toEmail));
            if (createdFromUtc.HasValue)
                query = query.Where(qe => qe.CreatedOnUtc >= createdFromUtc);
            if (createdToUtc.HasValue)
                query = query.Where(qe => qe.CreatedOnUtc <= createdToUtc);
            if (loadNotSentItemsOnly)
                query = query.Where(qe => !qe.SentOnUtc.HasValue);
            query = query.Where(qe => qe.SentTries < maxSendTries);
            query = query.OrderByDescending(qe => qe.Priority);
            query = loadNewest ?
                ((IOrderedQueryable<SenQueuedEmail>)query).ThenByDescending(qe => qe.CreatedOnUtc) :
                ((IOrderedQueryable<SenQueuedEmail>)query).ThenBy(qe => qe.CreatedOnUtc);

            //var queuedEmails = new PagedList<SenQueuedEmail>(query, pageIndex, pageSize);



            var queuedEmails = query.Skip(pageIndex*pageSize).Take(pageSize).ToList();

            return queuedEmails;
        }
    }
}
