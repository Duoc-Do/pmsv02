using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpSalesLinePrice : SenVietReportObject
    {
        public RpSalesLinePrice(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpSalesLinePrice";
            this._metaname = "RpSalesLinePrice";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpSalesLinePrice '{0}'";
            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham so co ban cua bao cao
            //ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            //ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());

            ReportParameter.Add("ItemGroupCode", para["ItemGroupCode"].ToString());
            #endregion
            return ReportParameter;
        }

        public List<Models.RpSalesLinePrice> GetData()
        {
            //var datefrom = DateTime.Today;
            //var dateto = DateTime.Today;
            //Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            //_params.Add("DateFrom", datefrom);
            //_params.Add("DateTo", dateto);

            string itemgroupcode = this._request.Params["ItemGroupCode"] != null ? this._request.Params["ItemGroupCode"].ToString() : "";
            _params.Add("ItemGroupCode", itemgroupcode);
            string strProcedure = string.Format(this._storeName, itemgroupcode);
            List<Models.RpSalesLinePrice> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpSalesLinePrice>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpSalesLinePrice>();
                var parasession = this.GetParamCache();

                if (!(this._params["ItemGroupCode"].Equals(parasession["ItemGroupCode"])))
                {
                    result = this._db.Database.SqlQuery<Models.RpSalesLinePrice>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpSalesLinePrice>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpSalesLinePrice>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}