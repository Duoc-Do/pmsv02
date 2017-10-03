using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SysTable : SenVietListObject
    {
        public SysTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysTable";
            this._metaname = "SysTable";
            this._keyfield = "TableName";
            this.appajaxoption.ajaxoption.Add("title", "Mô tả bảng");
            base.InitObject();
        }

        public List<Models.SysTable> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysTables);
            return model;
        }

        public Models.SysTable GetById(string Id)
        {
            return this._db.SysTables.SingleOrDefault(m => m.TableName == Id);
        }

        public Models.SysTable GetNew(string Id)
        {
            if (!string.IsNullOrEmpty(Id)) return GetById(Id);
            return new Models.SysTable();
        }

        public Models.SysTable GetEdit(string Id)
        {
            return GetById(Id);
        }

        public Models.SysTable GetDelete(string Id)
        {
            return GetById(Id);
        }

        public string Insert(Models.SysTable data)
        {
            try
            {
                this.Validate(data);

                this._db.SysTables.Add(data);
                this._db.SaveChanges();
                return data.TableName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Update(Models.SysTable data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.TableName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string Id)
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