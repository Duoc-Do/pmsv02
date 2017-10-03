using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.DAL
{
    public partial class SenScheduleTask : SenVietListObject
    {
        public SenScheduleTask(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "SenScheduleTask";
            this._metaname = "SenScheduleTask";
            this._keyfield = "ScheduleTaskId";
            this._storeNameI = @"sp_SenScheduleTaskI ";
            this._storeNameU = @"sp_SenScheduleTaskU ";
            this._storeNameD = @"sp_SenScheduleTaskD {0}";
            this.appajaxoption.ajaxoption.Add("title", "Lịch tác vụ");
            base.InitObject();
        }

        public List<Models.SenScheduleTask> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.SenScheduleTasks);
            return model;
        }

        public Models.SenScheduleTask GetById(int Id)
        {
            return this._db.SenScheduleTasks.SingleOrDefault(m => m.ScheduleTaskId == Id);
        }

        public Models.SenScheduleTask GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.SenScheduleTask();
        }

        public Models.SenScheduleTask GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.SenScheduleTask GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.SenScheduleTask data)
        {
            try
            {
                this.Validate(data);

                this._db.SenScheduleTasks.Add(data);
                this._db.SaveChanges();
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.SenScheduleTask data)
        {
            try
            {
                this.Validate(data);

                this._db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ScheduleTaskId;
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