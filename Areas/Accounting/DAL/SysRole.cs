
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysRole : SenVietListObject
    {
        public SysRole(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysRole";
            this._metaname = "SysRole";
            this._keyfield = "RoleID";
            this._storeNameI = @"sp_SysRoleI @RoleID OUTPUT,@Name,@FullName,@IsAdmin,@Note";
            this._storeNameU = @"sp_SysRoleU @RoleID OUTPUT,@Name,@FullName,@IsAdmin,@Note,@Original_RoleID";
            this._storeNameD = @"sp_SysRoleD {0}";
            base.InitObject();
        }

        public List<Models.SysRole> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysRoles);
            return model;
        }

        public Models.SysRole GetById(int Id)
        {
            return this._db.SysRoles.SingleOrDefault(m => m.RoleID == Id);
        }

        public Models.SysRole GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SysRole();
        }

        public Models.SysRole GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SysRole GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.SysRole data)
        {
            try
            {
                this.Validate(data);

                //data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                //data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SysRole data)
        {
            try
            {
                this.Validate(data);

                //data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                //data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.RoleID;
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
    }
}