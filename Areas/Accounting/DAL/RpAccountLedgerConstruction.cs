using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpAccountLedgerConstruction : SenVietReportObject
    {
        public RpAccountLedgerConstruction(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "RpAccountLedgerConstruction";
            this._metaname = "RpAccountLedgerConstruction";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpAccountLedgerConstruction '{0}','{1}','{2}','{3}'";

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
            ReportParameter.Add("DisplayNumber", para["DisplayNumber"].ToString());
            ReportParameter.Add("ConstructionCode", para["ConstructionCode"].ToString());


            return ReportParameter;
        }

        public List<Models.RpAccountLedgerConstruction> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            string DisplayNumber = this._request.Params["DisplayNumber"] != null ? this._request.Params["DisplayNumber"].ToString() : "";
            string ConstructionCode = this._request.Params["ConstructionCode"] != null ? this._request.Params["ConstructionCode"].ToString() : "";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);
            _params.Add("DisplayNumber", DisplayNumber);
            _params.Add("ConstructionCode", ConstructionCode);


            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), DisplayNumber,ConstructionCode);
            List<Models.RpAccountLedgerConstruction> result = null;

            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpAccountLedgerConstruction>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpAccountLedgerConstruction>();
                var parasession = this.GetParamCache();

                if (!(
                    this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["ConstructionCode"].Equals(parasession["ConstructionCode"])
                    && this._params["DisplayNumber"].Equals(parasession["DisplayNumber"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpAccountLedgerConstruction>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }

            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpAccountLedgerConstruction>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpAccountLedgerConstruction>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }

    }

}