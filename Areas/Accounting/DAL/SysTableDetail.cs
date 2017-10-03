using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysTableDetail : SenVietListObject
    {
        public SysTableDetail(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysTableDetail";
            this._metaname = "SysTableDetail";
            this._keyfield = "TableName,ColumnName";
            this.appajaxoption.ajaxoption.Add("title", "Khai báo đối tượng");
            base.InitObject();
        }

        public List<Models.SysTableDetail> GetData(string tablename)
        {
            var metaitem = this._metaobject.MetaTable.Find(m => m.ColumnName == "TableName");
            metaitem.FilterExpression = tablename;
            //var query = this._db.SysTableDetails.Where(m => m.TableName == tablename);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysTableDetails);
            return model;
        }

        public List<Models.SysTableDetail> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysTableDetails);
            return model;
        }

        public Models.SysTableDetail GetById(string TableName, string ColumnName)
        {
            return this._db.SysTableDetails.SingleOrDefault(m => m.TableName == TableName && m.ColumnName == ColumnName);
        }

        public Models.SysTableDetail GetNew(string TableName, string ColumnName)
        {
            var model = GetById(TableName, ColumnName);
            if (model != null)
            {
                return model;
            }
            return new Models.SysTableDetail();
        }

        public Models.SysTableDetail GetEdit(string TableName, string ColumnName)
        {
            return GetById(TableName, ColumnName);
        }

        public Models.SysTableDetail GetDelete(string TableName, string ColumnName)
        {
            return GetById(TableName, ColumnName);
        }
        public int Insert(Models.SysTableDetail data)
        {
            try
            {
                this.Validate(data);
                this._db.SysTableDetails.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SysTableDetail data)
        {
            try
            {
                this.Validate(data);
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                #region cập nhật lại cache
                var metaobject = WebApp.Areas.Accounting.Services.GlobalMeta.GetMetaObject(data.TableName);
                var row = metaobject.MetaTable.Where(m => m.ColumnName == data.ColumnName).SingleOrDefault();
                this.MapView2Table(data, row);
                //row.GridViewShow = data.GridViewShow;
                #endregion

                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string TableName, string ColumnName)
        {
            try
            {
                var _rec = GetById(TableName, ColumnName);
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