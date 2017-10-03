﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpTrialBalanceExpense : SenVietReportObject
    {
        public RpTrialBalanceExpense(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpTrialBalanceExpense";
            this._metaname = "RpTrialBalanceExpense";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpTrialBalanceExpense '{0}','{1}'";
            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham so co ban cua bao cao
            ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());
            #endregion
            return ReportParameter;
        }

        public List<Models.RpTrialBalanceExpense> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"));
            List<Models.RpTrialBalanceExpense> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpTrialBalanceExpense>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpTrialBalanceExpense>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateFrom"].Equals(parasession["DateFrom"]) && this._params["DateTo"].Equals(parasession["DateTo"])))
                {
                    result = this._db.Database.SqlQuery<Models.RpTrialBalanceExpense>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpTrialBalanceExpense>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpTrialBalanceExpense>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}