﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class RpGLJournal3 : SenVietReportObject
    {
        public RpGLJournal3(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "RpGLJournal3";
            this._metaname = "RpGLJournal3";
            this._keyfield = "DocumentID";
            this._storeName = @"sp_RpGLJournal3 '{0}','{1}','{2}'";
            base.InitObject();
        }

        public System.Collections.Hashtable GetReportParameter()
        {
            var para = this.GetParamCache();
            System.Collections.Hashtable ReportParameter = new System.Collections.Hashtable();
            #region tham so co ban cua bao cao
            ReportParameter.Add("DateFrom", ((DateTime)para["DateFrom"]).ToShortDateString());
            ReportParameter.Add("DateTo", ((DateTime)para["DateTo"]).ToShortDateString());

            ReportParameter.Add("DisplayNumberDebitList", para["DisplayNumberDebitList"].ToString());
            ReportParameter.Add("DisplayNumberCreditList", para["DisplayNumberCreditList"].ToString());
            #endregion
            return ReportParameter;
        }

        public List<Models.RpGLJournal3> GetData()
        {
            var datefrom = DateTime.Today;
            var dateto = DateTime.Today;
            Uti.Date.SetDateRange(this._request, out datefrom, out dateto);

            string DisplayNumberDebitList = this._request.Params["DisplayNumberDebitList"] != null ? this._request.Params["DisplayNumberDebitList"].ToString() : "15,62,64,211";
            string DisplayNumberCreditList = this._request.Params["DisplayNumberCreditList"] != null ? this._request.Params["DisplayNumberCreditList"].ToString() : "111,112,331,141";

            _params.Add("DateFrom", datefrom);
            _params.Add("DateTo", dateto);

            _params.Add("DisplayNumberDebitList", DisplayNumberDebitList);
            _params.Add("DisplayNumberCreditList", DisplayNumberCreditList);

            string paravoucherdate = string.Format("VoucherDate>=''{0}'' and VoucherDate<=''{1}''", datefrom.ToString("yyyyMMdd"), dateto.ToString("yyyyMMdd"));

            string strProcedure = string.Format(this._storeName, paravoucherdate, DisplayNumberDebitList, DisplayNumberCreditList);
            List<Models.RpGLJournal3> result = null;
            if (!this.IsSession())
            {
                result = this._db.Database.SqlQuery<Models.RpGLJournal3>(strProcedure).ToList();
            }
            else
            {
                var resultsession = this.GetDataCache<Models.RpGLJournal3>();
                var parasession = this.GetParamCache();

                if (!(this._params["DateFrom"].Equals(parasession["DateFrom"]) 
                    && this._params["DateTo"].Equals(parasession["DateTo"])
                    && this._params["DisplayNumberDebitList"].Equals(parasession["DisplayNumberDebitList"])
                    && this._params["DisplayNumberCreditList"].Equals(parasession["DisplayNumberCreditList"])
                    ))
                {
                    result = this._db.Database.SqlQuery<Models.RpGLJournal3>(strProcedure).ToList();
                }
                else
                {
                    result = resultsession;
                }
            }
            this.SetDataCache(result);
            var model = Services.GridHelper.GetResults(this._request, this._metaname, result.AsQueryable<Models.RpGLJournal3>());
            this.SetParamCache(_params);

            return model;
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetDataCacheAll<Models.RpGLJournal3>(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}