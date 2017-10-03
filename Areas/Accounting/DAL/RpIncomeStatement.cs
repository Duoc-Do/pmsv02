using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpIncomeStatement : SenVietReportObject
    {
        public RpIncomeStatement(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpIncomeStatement";
            this._metaname = "RpIncomeStatement";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpIncomeStatement '{0}','{1}','{2}','{3}'";
            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham so co ban cua bao cao
            ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());

            ReportParameter.Add("DateFromBG", ((DateTime)para["DateFromBG"]).ToShortDateString());
            ReportParameter.Add("DateToBG", ((DateTime)para["DateToBG"]).ToShortDateString());

            #endregion
            return ReportParameter;
        }

        public List<Models.RpIncomeStatement> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;

            var datefrombg = DateTime.Today;
            var datetobg = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            datefrombg = DateTime.Parse(this._request.Params["DateFromBG"] ?? DateTime.Now.Date.ToShortDateString()); // Default min giờ.
            datetobg = DateTime.Parse(this._request.Params["DateToBG"] ?? DateTime.Now.Date.ToShortDateString());
            datetobg = datetobg.Date.AddDays(1).AddTicks(-1); // Set Max giờ.

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            _params.Add("DateFromBG", datefrombg);
            _params.Add("DateToBG", datetobg);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), datefrombg.ToString("yyyyMMdd"), datetobg.ToString("yyyyMMdd"));
            List<Models.RpIncomeStatement> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpIncomeStatement>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpIncomeStatement>();
                var parasession = this.GetParamCache();

                if (!(
                    this._params["DateFrom"].Equals(parasession["DateFrom"]) 
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["DateFromBG"].Equals(parasession["DateFromBG"])
                    && this._params["DateToBG"].Equals(parasession["DateToBG"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpIncomeStatement>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpIncomeStatement>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpIncomeStatement>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}