using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysReport : SenVietListObject
    {
        public SysReport(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysReport";
            this._metaname = "SysReport";
            this._keyfield = "SysReportID";
            this._storeNameI = @"";
            this._storeNameU = @"";
            this._storeNameD = @"sp_SysReportD {0}";
            base.InitObject();
        }

        public List<Models.SysReport> GetData(string businesscode)
        {
            
            var metaitem = this._metaobject.MetaTable.Find(m => m.ColumnName == "BusinessCode");
            metaitem.FilterExpression = businesscode;
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysReports);
            return model;
        }


        public List<Models.SysReport> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysReports);
            return model;
        }

        public Models.SysReport GetById(int Id)
        {
            return this._db.SysReports.SingleOrDefault(m => m.SysReportID == Id);
        }

        public Models.SysReport GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SysReport();
        }

        public Models.SysReport GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SysReport GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SysReport data)
        {
            try
            {
                this.Validate(data);
                this._db.SysReports.Add(data);
                this._db.SaveChanges();
                return data.SysReportID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SysReport data)
        {
            try
            {
                this.Validate(data);
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.SysReportID;
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
                this._db.Database.ExecuteSqlCommand(this._storeNameD, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}