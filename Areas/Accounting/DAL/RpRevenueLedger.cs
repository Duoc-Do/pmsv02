﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpRevenueLedger : SenVietReportObject
    {
        public RpRevenueLedger(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "RpRevenueLedger";
            this._metaname = "RpRevenueLedger";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpRevenueLedger '{0}','{1}','{2}','{3}'";

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
            ReportParameter.Add("ItemCode", para["ItemCode"].ToString());

            return ReportParameter;
        }

        public List<Models.RpRevenueLedger> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            string CustomerCode = this._request.Params["CustomerCode"] != null ? this._request.Params["CustomerCode"].ToString() : "";
            string ItemCode = this._request.Params["ItemCode"] != null ? this._request.Params["ItemCode"].ToString() : "";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            _params.Add("CustomerCode", CustomerCode);
            _params.Add("ItemCode", ItemCode);

            string strProcedure = string.Format(this._storeName, datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"), CustomerCode, ItemCode);
            List<Models.RpRevenueLedger> result = null;

            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpRevenueLedger>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpRevenueLedger>();
                var parasession = this.GetParamCache();

                if (!(
                    this._params["DateFrom"].Equals(parasession["DateFrom"])
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["CustomerCode"].Equals(parasession["CustomerCode"])
                    && this._params["ItemCode"].Equals(parasession["ItemCode"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpRevenueLedger>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }

            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpRevenueLedger>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpRevenueLedger>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }

    }

}