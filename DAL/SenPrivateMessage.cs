using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApp.Services;

namespace WebApp.DAL
{
    public partial class SenPrivateMessage : SenVietListObject
    {
        public SenPrivateMessage(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenPrivateMessage";
            this._metaname = "SenPrivateMessage";
            this._keyfield = "PrivateMessageId";
            base.InitObject();
        }

        public List<Models.SenPrivateMessage> GetData()
        {
            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname, this._db.SenPrivateMessages);
            return model;
        }

        public List<Models.SenPrivateMessage> GetDataByUser(string username)
        {
            string search = "";
            if (this._request.Form["Paging.Search"] != null) search = this._request.Form["Paging.Search"];
            var MetaObject = Services.GlobalMeta.GetMetaObject(this._metaname);
            MetaObject.Paging.Search = search;
            var query = this._db.SenPrivateMessages.Where(m => m.ToUser.UserName == username && !m.IsDeletedByRecipient).OrderByDescending(m => m.CreatedOnUtc).AsQueryable<Models.SenPrivateMessage>();
            if (!string.IsNullOrEmpty(search)) query = query.Where(m => (m.Subject.Contains(search)||m.CreatedOnUtc.ToString().Contains(search))).AsQueryable<Models.SenPrivateMessage>();

            

            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname, query);
            return model;
        }

        public void PrivateMessageReads(string username, string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                PrivateMessageRead(username, int.Parse(ids[i]));
            }
        }

        public Models.SenPrivateMessage PrivateMessageRead(string username, int id)
        {
            var query = this._db.SenPrivateMessages.Where(m => m.ToUser.UserName == username && m.PrivateMessageId == id).SingleOrDefault();
            if (query != null)
            {
                query.IsRead = true;
                this.Update(query);
            }
            return query;
        }

        public int PrivateMessageSend()
        {
            Models.SenPrivateMessage data = new Models.SenPrivateMessage();
            data.ToUserId = Guid.Parse(this._request["ToUserId"]);
            data.FromUserId = Guid.Parse(GlobalUserContext.GetContext().User.ProviderUserKey.ToString());
            data.Subject = this._request["Subject"];
            data.Text = this._request["Text"];
            return this.Insert(data); 
        }

        public void PrivateMessageDeleteByRecipients(string username, string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                PrivateMessageDeleteByRecipient(username, int.Parse(ids[i]));
            }
        }

        public Models.SenPrivateMessage PrivateMessageDeleteByRecipient(string username, int id)
        {
            var query = this._db.SenPrivateMessages.Where(m => m.ToUser.UserName == username && m.PrivateMessageId == id).SingleOrDefault();
            if (query != null)
            {
                query.IsDeletedByRecipient = true;
                this.Update(query);
            }
            return query;
        }

        public void PrivateMessageDeleteByAuthors(string username, string[] ids)
        {
            for (int i = 0; i < ids.Count(); i++)
            {
                PrivateMessageDeleteByAuthor(username, int.Parse(ids[i]));
            }
        }

        public Models.SenPrivateMessage PrivateMessageDeleteByAuthor(string username, int id)
        {
            var query = this._db.SenPrivateMessages.Where(m => m.ToUser.UserName == username && m.PrivateMessageId == id).SingleOrDefault();
            if (query != null)
            {
                query.IsDeletedByAuthor = true;
                this.Update(query);
            }
            return query;
        }

        public List<T> GetData<T>(IQueryable<T> query)
        {
            var model = Services.Helpers.GridHelper.GetResults(this._request, this._metaname,query);
            return model;
        }

        public Models.SenPrivateMessage GetById(int Id)
        {
            return this._db.SenPrivateMessages.SingleOrDefault(m => m.PrivateMessageId == Id);
        }

        public Models.SenPrivateMessage GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenPrivateMessage();
        }

        public Models.SenPrivateMessage GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenPrivateMessage GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SenPrivateMessage data)
        {
            try
            {
                this.Validate(data);
                data.CreatedOnUtc = DateTime.UtcNow;
                this._db.SenPrivateMessages.Add(data);
                this._db.SaveChanges();
                return data.PrivateMessageId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenPrivateMessage data)
        {
            try
            {
                this.Validate(data);
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.PrivateMessageId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int Id)
        {
            try
            {
                var _rec = GetById(Id);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}