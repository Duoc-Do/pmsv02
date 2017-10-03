using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpCustomerTrialBalance : SenVietReportObject
    {
        public RpCustomerTrialBalance(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "RpCustomerTrialBalance";
            this._metaname = "RpCustomerTrialBalance";
            this._keyfield = "StockCardID";
            this._storeName = @"sp_RpCustomerTrialBalance '{0}','{1}','{2}','{3}'";

            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham số cơ bản của báo cáo
            ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());
            #endregion

            ReportParameter.Add("CustomerCode", para["CustomerCode"].ToString());
            ReportParameter.Add("DisplayNumber", para["DisplayNumber"].ToString());

            return ReportParameter;
        }

        public List<Models.RpCustomerTrialBalance> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            string CustomerCode = this._request.Params["CustomerCode"] != null ? this._request.Params["CustomerCode"].ToString() : "";
            string DisplayNumber = this._request.Params["DisplayNumber"] != null ? this._request.Params["DisplayNumber"].ToString() : "";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            _params.Add("CustomerCode", CustomerCode);
            _params.Add("DisplayNumber", DisplayNumber);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), CustomerCode, DisplayNumber);
            List<Models.RpCustomerTrialBalance> result = null;

            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpCustomerTrialBalance>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpCustomerTrialBalance>();
                var parasession = this.GetParamCache();

                if (!(
                    this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["CustomerCode"].Equals(parasession["CustomerCode"])
                    && this._params["DisplayNumber"].Equals(parasession["DisplayNumber"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpCustomerTrialBalance>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }

            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpCustomerTrialBalance>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpCustomerTrialBalance>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }

    }

}