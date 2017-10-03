using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class SysMenu : SenVietListObject
    {
        public SysMenu(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SysMenu";
            this._metaname = "SysMenu";
            this._keyfield = "MenuID";
            this._storeNameI = @"sp_SysMenuI @MenuID,@ParentID,@Des,@MenuIcon,@MenuOrder,@MenuType,@BusinessCode,@Note,@IsActive";
            this._storeNameU = @"sp_SysMenuU @MenuID,@ParentID,@Des,@MenuIcon,@MenuOrder,@MenuType,@BusinessCode,@Note,@IsActive,@Original_MenuID";
            this._storeNameD = @"sp_SysMenuD {0}";
            base.InitObject();
        }

        public List<Models.SysMenu> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SysMenus);
            return model;
        }

        public Models.SysMenu GetById(string Id)
        {
            return this._db.SysMenus.SingleOrDefault(m => m.MenuID == Id);
        }

        public Models.SysMenu GetNew(string id)
        {
            if (id != null) return GetById(id);
            return new Models.SysMenu();
        }

        public Models.SysMenu GetEdit(string id)
        {
            return GetById(id);
        }

        public Models.SysMenu GetDelete(string id)
        {
            return GetById(id);
        }
        public string Insert(Models.SysMenu data)
        {
            try
            {
                this.Validate(data);
                this._db.SysMenus.Add(data);
                this._db.SaveChanges();
                return data.MenuID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Update(Models.SysMenu data)
        {
            try
            {
                this.Validate(data);
                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
                return data.MenuID;
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