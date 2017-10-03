using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpPurchaseLedger : SenVietReportObject
    {
        public RpPurchaseLedger(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpPurchaseLedger";
            this._metaname = "RpPurchaseLedger";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpPurchaseLedger '{0}','{1}'";
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

        public List<Models.RpPurchaseLedger> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"));
            List<Models.RpPurchaseLedger> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpPurchaseLedger>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpPurchaseLedger>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateFrom"].Equals(parasession["DateFrom"]) && this._params["DateTo"].Equals(parasession["DateTo"])))
                {
                    result = this._db.Database.SqlQuery<Models.RpPurchaseLedger>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpPurchaseLedger>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpPurchaseLedger>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}