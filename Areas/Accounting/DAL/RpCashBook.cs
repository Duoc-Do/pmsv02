using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpCashBook : SenVietReportObject
    {
        public RpCashBook(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpCashBook";
            this._metaname = "RpCashBook";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpCashBook '{0}','{1}','{2}'";
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
            #endregion
            return ReportParameter;
        }

        public List<Models.RpCashBook> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);
            string DisplayNumber = this._request.Params["DisplayNumber"] != null ? this._request.Params["DisplayNumber"].ToString() : "";


            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);
            _params.Add("DisplayNumber", DisplayNumber);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), DisplayNumber);
            List<Models.RpCashBook> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpCashBook>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpCashBook>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["DisplayNumber"].Equals(parasession["DisplayNumber"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpCashBook>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpCashBook>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpCashBook>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}
