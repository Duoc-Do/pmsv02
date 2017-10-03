using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppExpenseBudgetTable : SenVietListObject
    {
        public AppExpenseBudgetTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppExpenseBudgetTable";
            this._metaname = "AppExpenseBudgetView";
            this._keyfield = "ExpenseBudgetID";
            this._storeNameI = @"sp_AppExpenseBudgetTableI @ExpenseBudgetID OUTPUT,@DateOfExecution,@ExpenseID,@Amount,@AmountFC,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
            this._storeNameU = @"sp_AppExpenseBudgetTableU @ExpenseBudgetID OUTPUT,@DateOfExecution,@ExpenseID,@Amount,@AmountFC,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_ExpenseBudgetID";
            this._storeNameD = @"sp_AppExpenseBudgetTableD {0}";
            base.InitObject();
        }

        public List<Models.AppExpenseBudgetView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppExpenseBudgetViews);
            return model;
        }

        public Models.AppExpenseBudgetView GetById(int Id)
        {
            return this._db.AppExpenseBudgetViews.SingleOrDefault(m => m.ExpenseBudgetID == Id);
        }

        public Models.AppExpenseBudgetView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppExpenseBudgetView() { DateOfExecution=DateTime.Today,IsActive=true};
        }

        public Models.AppExpenseBudgetView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppExpenseBudgetView GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.AppExpenseBudgetView data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppExpenseBudgetView data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.ExpenseBudgetID;
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