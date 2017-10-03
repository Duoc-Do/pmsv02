using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysBusiness : SenVietListObject
    {
        public SysBusiness(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysBusiness";
            this._metaname = "SysBusiness";
            this._keyfield = "BusinessCode";
            this._storeNameI = @"sp_SysBusinessI @BusinessCode,@Des,@Type,@AssemblyName,@AssemblyTypeName,@TableUIList";
            this._storeNameU = @"sp_SysBusinessU @BusinessCode,@Des,@Type,@AssemblyName,@AssemblyTypeName,@TableUIList,@Original_BusinessCode";
            this._storeNameD = @"sp_SysBusinessD {0}";
            base.InitObject();
        }

        public List<Models.SysBusiness> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysBusinesses);
            return model;
        }

        public Models.SysBusiness GetById(string Id)
        {
            return this._db.SysBusinesses.SingleOrDefault(m => m.BusinessCode == Id);
        }

        public Models.SysBusiness GetNew(string id)
        {
            if (id != null) return GetById(id);
            return new Models.SysBusiness();
        }

        public Models.SysBusiness GetEdit(string id)
        {
            return GetById(id);
        }

        public Models.SysBusiness GetDelete(string id)
        {
            return GetById(id);
        }
        public string Insert(Models.SysBusiness data)
        {
            try
            {
                this.Validate(data);
                this._db.SysBusinesses.Add(data);
                this._db.SaveChanges();

                //data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                //data.CreatedDateTime = DateTime.Now;

                //SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                //this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return data.BusinessCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Update(Models.SysBusiness data)
        {
            try
            {
                this.Validate(data);

                //data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                //data.ModifiedDateTime = DateTime.Now;

                //SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                //var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                //this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.BusinessCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(string id)
        {
            try
            {
                this._db.Database.ExecuteSqlCommand(this._storeNameD, id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}