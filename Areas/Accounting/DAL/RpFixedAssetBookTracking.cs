using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpFixedAssetBookTracking : SenVietReportObject
    {
        public RpFixedAssetBookTracking(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpFixedAssetBookTracking";
            this._metaname = "RpFixedAssetBookTracking";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpFixedAssetBookTracking '{0}'";
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

        public List<Models.RpFixedAssetBookTracking> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            string strProcedure = string.Format(this._storeName, dateto.ToString("yyyyMMdd"));
            List<Models.RpFixedAssetBookTracking> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpFixedAssetBookTracking>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpFixedAssetBookTracking>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateTo"].Equals(parasession["DateTo"])))
                {
                    result = this._db.Database.SqlQuery<Models.RpFixedAssetBookTracking>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpFixedAssetBookTracking>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpFixedAssetBookTracking>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}