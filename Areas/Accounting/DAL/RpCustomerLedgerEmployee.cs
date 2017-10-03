using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpCustomerLedgerEmployee : SenVietReportObject
    {
        public RpCustomerLedgerEmployee(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "RpCustomerLedgerEmployee";
            this._metaname = "RpCustomerLedgerEmployee";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpCustomerLedgerEmployee '{0}','{1}','{2}','{3}','{4}'";

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
            ReportParameter.Add("EmployeeCode", para["EmployeeCode"].ToString());
            ReportParameter.Add("CustomerCode", para["CustomerCode"].ToString());
            ReportParameter.Add("DisplayNumber", para["DisplayNumber"].ToString());

            return ReportParameter;
        }

        public List<Models.RpCustomerLedgerEmployee> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            string EmployeeCode = this._request.Params["EmployeeCode"] != null ? this._request.Params["EmployeeCode"].ToString() : "";
            string CustomerCode = this._request.Params["CustomerCode"] != null ? this._request.Params["CustomerCode"].ToString() : "";
            string DisplayNumber = this._request.Params["DisplayNumber"] != null ? this._request.Params["DisplayNumber"].ToString() : "";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            _params.Add("EmployeeCode", EmployeeCode);
            _params.Add("CustomerCode", CustomerCode);
            _params.Add("DisplayNumber", DisplayNumber);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), EmployeeCode, CustomerCode, DisplayNumber);
            List<Models.RpCustomerLedgerEmployee> result = null;

            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpCustomerLedgerEmployee>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpCustomerLedgerEmployee>();
                var parasession = this.GetParamCache();

                if (!(
                    this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["EmployeeCode"].Equals(parasession["EmployeeCode"])
                    && this._params["CustomerCode"].Equals(parasession["CustomerCode"])
                    && this._params["DisplayNumber"].Equals(parasession["DisplayNumber"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpCustomerLedgerEmployee>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }

            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpCustomerLedgerEmployee>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpCustomerLedgerEmployee>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }

}