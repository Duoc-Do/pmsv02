using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class GapTree : SenVietListObject
    {
        public GapTree(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "GapTree";
            this._metaname = "GapTree";
            this._keyfield = "TreeId";
            base.InitObject();
        }

        public List<Models.GapTree> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.GapTrees);
            return model;
        }

        public Models.GapTree GetById(int Id)
        {
            return this._db.GapTrees.SingleOrDefault(m => m.TreeId == Id);
        }
        public Models.GapTree GetById2(int Id)
        {
            return this._db.GapTrees.SingleOrDefault(m => m.TreeId == Id);
        }

        public Models.GapTree GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.GapTree() {IsActive=true };
        }

        public Models.GapTree GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.GapTree GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.GapTree data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.GapTree();
                this.MapView2Table(data, _data);
                this._db.GapTrees.Add(_data);
                this._db.SaveChanges();
                return data.TreeId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.GapTree data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.TreeId);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.TreeId;
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
                var _rec = GetById2(Id);
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