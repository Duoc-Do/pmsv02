using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpAccountLedgerExpense : SenVietReportObject
    {
        public RpAccountLedgerExpense(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpAccountLedgerExpense";
            this._metaname = "RpAccountLedgerExpense";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpAccountLedgerExpense '{0}','{1}','{2}','{3}'";
            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham so co ban cua bao cao
            ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());
            ReportParameter.Add("DisplayNumber", para["DisplayNumber"].ToString());
            ReportParameter.Add("ExpenseCode", para["ExpenseCode"].ToString());
            #endregion
            return ReportParameter;
        }

        public List<Models.RpAccountLedgerExpense> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);
            string DisplayNumber = this._request.Params["DisplayNumber"] != null ? this._request.Params["DisplayNumber"].ToString() : "";
            string ExpenseCode = this._request.Params["ExpenseCode"] != null ? this._request.Params["ExpenseCode"].ToString() : "";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);
            _params.Add("DisplayNumber", DisplayNumber);
            _params.Add("ExpenseCode", ExpenseCode);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), DisplayNumber, ExpenseCode);
            List<Models.RpAccountLedgerExpense> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpAccountLedgerExpense>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpAccountLedgerExpense>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["DisplayNumber"].Equals(parasession["DisplayNumber"])
                    && this._params["ExpenseCode"].Equals(parasession["ExpenseCode"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpAccountLedgerExpense>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpAccountLedgerExpense>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpAccountLedgerExpense>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}
