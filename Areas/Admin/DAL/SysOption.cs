using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SysOption : SenVietListObject
    {
        public SysOption(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysOption";
            this._metaname = "SysOption";
            this._keyfield = "SysOptionID";
            this._storeNameI = @"sp_SysOptionI @SysOptionID OUTPUT,@SysOptionName,@Type,@Des,@SysOptionValue,@SysOptionDefault";
            this._storeNameU = @"sp_SysOptionU @SysOptionID OUTPUT,@SysOptionName,@Type,@Des,@SysOptionValue,@SysOptionDefault,@Original_SysOptionID";
            this._storeNameD = @"sp_SysOptionD {0}";
            base.InitObject();
        }

        public List<Models.SysOption> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysOptions);
            return model;
        }

        public Models.SysOption GetById(int Id)
        {
            return this._db.SysOptions.SingleOrDefault(m => m.SysOptionID == Id);
        }

        public Models.SysOption GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SysOption();
        }

        public Models.SysOption GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SysOption GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SysOption data)
        {
            try
            {
                this.Validate(data);
                this._db.SysOptions.Add(data);
                this._db.SaveChanges();
                return data.SysOptionID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SysOption data)
        {
            try
            {
                this.Validate(data);
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.SysOptionID;
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