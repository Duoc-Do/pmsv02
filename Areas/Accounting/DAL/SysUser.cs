
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysUser : SenVietListObject
    {
        public SysUser(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysUser";
            this._metaname = "SysUserView";
            this._keyfield = "UserID";
            this._storeNameI = @"sp_SysUserI @UserID OUTPUT,@Name,@Password,@FullName,@RoleID,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
            this._storeNameU = @"sp_SysUserU @UserID OUTPUT,@Name,@Password,@FullName,@RoleID,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_UserID";
            this._storeNameD = @"sp_SysUserD {0}";
            base.InitObject();
        }

        public List<Models.SysUserView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysUserViews);
            var appuser = Services.GlobalVariant.GetAppUser();
            if (appuser.RoleName == "Systems") return model;
            if (appuser.RoleName == "Administrators") return model.Where(m => m.RoleName != "Systems").ToList();
            return model.Where(m => m.UserID==appuser.UserID).ToList();

        }

        public Models.SysUserView GetById(int Id)
        {
            var appuser = Services.GlobalVariant.GetAppUser();
            if (appuser.RoleName == "Systems") return this._db.SysUserViews.SingleOrDefault(m => m.UserID == Id);
            if (appuser.RoleName == "Administrators") return this._db.SysUserViews.SingleOrDefault(m => m.UserID == Id && m.RoleName != "Systems");
            return this._db.SysUserViews.SingleOrDefault(m => m.UserID == appuser.UserID);
        }

        public Models.SysUserView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SysUserView();
        }

        public Models.SysUserView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SysUserView GetDelete(int id)
        {
            return GetById(id);
        }

        //public string EncryptPassWord(string username,string password)
        //{
        //    return Accounting.Services.MD5.GenerateHashDigest(username + password);
        //}

        public int Insert(Models.SysUserView data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                data.Password = Accounting.Services.MD5.GenerateHashDigest(data.Name + data.Password);

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SysUserView data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;
                var olddata = GetById(data.UserID);
                if (olddata.Name!=data.Name||olddata.Password!=data.Password)
                {
                    data.Password = Accounting.Services.MD5.GenerateHashDigest(data.Name + data.Password);
                }

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.UserID;
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