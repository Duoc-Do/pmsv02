using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using WebApp.Areas.Admin.Services;

namespace WebApp.Areas.Admin.DAL
{

    //public class SenVietGeneralObject
    //{
    //    public Models.AdminContext _db;
    //    public HttpRequestBase _request;

    //    public Models.AppAjaxOption appajaxoption = new Models.AppAjaxOption();
    //    public Models.SysBusiness sysbusiness;
    //    public Models.SysBusinessRole sysbusinessrole;

    //    public string _businesscode;


    //    public SenVietGeneralObject(HttpRequestBase request, string businesscode)
    //    {
    //        this.InitObject();

    //        this._db = new Models.AdminContext(Services.GlobalVariant.GetConnection());
    //        this._request = request;
    //        this._businesscode = businesscode;

    //        appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._businesscode.ToLower()));
    //        appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
    //        appajaxoption.ajaxoption.Add("metaname", this._businesscode);
    //        appajaxoption.ajaxoption.Add("keyfield", "");
    //        appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);

    //        this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();
    //        int roleid = Services.GlobalVariant.GetAppUser().RoleID;
    //        this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();
    //    }
    //    public virtual void InitObject() { }

    //    public object AutoComplete()
    //    {
    //        return Data.GetByKeyword(this._request);
    //    }

    //    public object FieldChange()
    //    {
    //        return Services.Data.GetByCode(this._request);
    //    }

    //}

    //public abstract class SenVietReportObject
    //{
    //    public Models.AdminContext _db;
    //    public HttpRequestBase _request;
    //    public Models.MetaObject _metaobject;
    //    public HttpSessionState _currentsession;

    //    public Dictionary<string, object> _params = new Dictionary<string, object>();
    //    public Models.AppAjaxOption appajaxoption = new Models.AppAjaxOption();
    //    public List<Models.SysReport> sysreports;
    //    public Models.SysBusiness sysbusiness;
    //    public Models.SysBusinessRole sysbusinessrole;

    //    public string _businesscode;
    //    public string _metaname;
    //    public string _keyfield;

    //    public string _storeName;

    //    public string _sessionkeys;
    //    public string _sessionparamskeys;


    //    public SenVietReportObject(HttpRequestBase request)
    //    {
    //        this.InitObject();

    //        this._db = new Models.AdminContext(Services.GlobalVariant.GetConnection()); 
    //        this._request = request;
    //        this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);
    //        this._currentsession = HttpContext.Current.Session;

    //        this._sessionkeys = string.Format("{0}.Model", this._businesscode);
    //        this._sessionparamskeys = string.Format("{0}.Params", this._businesscode);

    //        appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._metaname.ToLower()));
    //        appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
    //        appajaxoption.ajaxoption.Add("metaname", this._metaname);
    //        appajaxoption.ajaxoption.Add("keyfield", this._keyfield);
    //        appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);

    //        this.sysreports = this._db.SysReports.Where(m => m.BusinessCode == this._businesscode).OrderBy(m => m.IsDefault).ToList();
    //        this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();

    //        int roleid = Services.GlobalVariant.GetAppUser().RoleID;
    //        this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();
    //    }
    //    public virtual void InitObject() { }

    //    public virtual Dictionary<string, object> GetParamCache()
    //    {
    //        var parasession = (Dictionary<string, object>)this._currentsession[this._sessionparamskeys];
    //        return parasession;
    //    }

    //    public virtual void SetParamCache(Dictionary<string, object> _paramcache)
    //    {
    //        this._currentsession[this._sessionparamskeys] = _paramcache;
    //    }

    //    public virtual void SetDataCache<T>(T data)
    //    {
    //        this._currentsession[this._sessionkeys] = data;
    //    }
    //    public bool IsSession()
    //    {
    //        return this._currentsession[this._sessionkeys] != null;
    //    }

    //    public List<T> GetDataCache<T>()
    //    {
    //        var result = (List<T>)this._currentsession[this._sessionkeys];
    //        return result;
    //    }

    //    public List<T> GetDataCacheAll<T>()
    //    {
    //        var result = this.GetDataCache<T>();
    //        var model = Services.GridHelper.GetResultAlls(this._request, this._metaname, result.AsQueryable<T>());
    //        return model;
    //    }


    //    public object AutoComplete()
    //    {
    //        return Data.GetByKeyword(this._request);
    //    }

    //    public object FieldChange()
    //    {
    //        return Services.Data.GetByCode(this._request);
    //    }

    //}

    public abstract class SenVietListObject
    {
        public Models.AdminContext _db;
        public HttpRequestBase _request;
        public Models.MetaObject _metaobject;
        //public Models.SysBusiness sysbusiness;
        //public Models.SysBusinessRole sysbusinessrole;

        public Dictionary<string, string> Errors = new Dictionary<string, string>();
        public Models.AppAjaxOption appajaxoption = new Models.AppAjaxOption();
        public string _paramnameoutput;
        public string _paramnameupdate;

        public string _businesscode;
        public string _metaname;
        public string _keyfield;

        public string _storeNameI;
        public string _storeNameU;
        public string _storeNameD;

        public SenVietListObject(HttpRequestBase request)
        {
            this.InitObject();
            this._db = new Models.AdminContext();
            this._request = request;
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

            this._paramnameoutput = this._keyfield;
            this._paramnameupdate = string.Format("Original_{0}", this._keyfield);

            appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._metaname.ToLower()));

            appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
            appajaxoption.ajaxoption.Add("metaname", this._metaname);
            appajaxoption.ajaxoption.Add("keyfield", this._keyfield);
            appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);

            //this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();
            //int roleid = Services.GlobalVariant.GetAppUser().RoleID;
            //this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();
        }

        public virtual void InitObject() { }

        public object AutoComplete()
        {
            return Data.GetByKeyword(this._request);
        }

        public virtual object FieldChange()
        {

            return Services.Data.GetByCode(this._request);
        }

        public void Validate(dynamic data)
        {

            #region Business logic validate
            #region Object validate

            foreach (var item in this._metaobject.MetaTable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                if (item.AllowDBNull != true)
                {
                    var value = data.GetType().GetProperty(fieldmap).GetValue(data, null) ?? String.Empty;

                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        Errors.Add(fieldmap, string.Format("phải nhập {0}", item.Des.ToLower()));
                    }
                }
            }
            #endregion

            #endregion

            if (Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");

        }

        public void MapView2Table(dynamic view, dynamic table)
        {
            this.MapView2Table(view, table, this._businesscode);
        }

        public void MapView2Table(dynamic view, dynamic table, string businesscode)
        {
            var metatable = Services.GlobalMeta.GetMetaObject(businesscode).MetaTable;//lấy meta của bảng gốc

            foreach (var item in metatable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                var value = view.GetType().GetProperty(fieldmap).GetValue(view, null);
                table.GetType().GetProperty(fieldmap).SetValue(table, value, null);
            }
        }

        public virtual byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, null, this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }

    }


    //public abstract class SenVietVoucherObject
    //{
    //    #region Tham số
    //    public Models.AdminContext _db;
    //    public HttpRequestBase _request;
    //    public Models.MetaObject _metaobject;
    //    public Models.SysBusiness sysbusiness;
    //    public Models.SysBusinessRole sysbusinessrole;

    //    public Dictionary<string, string> Errors = new Dictionary<string, string>();
    //    public Models.AppAjaxOption appajaxoption = new Models.AppAjaxOption();

    //    public string _paramnamemasteroutput;
    //    public string _paramnamemasterupdate;

    //    public string _paramnamelineoutput;
    //    public string _paramnamelineupdate;

    //    public string _paramnamevatoutput;
    //    public string _paramnamevatupdate;


    //    public string _businesscode;
    //    public string _keyfieldmaster;
    //    public string _keyfieldline;
    //    public string _keyfieldvat;


    //    public string _metaname;
    //    public string _metalinename;
    //    public string _metavatname;


    //    public string _storeNameI;
    //    public string _storeNameU;
    //    public string _storeNameD;


    //    public string _storeNameLineI;
    //    public string _storeNameLineU;
    //    public string _storeNameLineD;

    //    public string _storeNameVATI;
    //    public string _storeNameVATU;
    //    public string _storeNameVATD;

    //    #endregion

    //    public SenVietVoucherObject(HttpRequestBase request)
    //    {
    //        this.InitObject();
    //        this._db = new Models.AdminContext(Services.GlobalVariant.GetConnection()); ;
    //        this._request = request;
    //        this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

    //        this._paramnamemasteroutput = this._keyfieldmaster;
    //        this._paramnamemasterupdate = string.Format("Original_{0}", this._keyfieldmaster);

    //        this._paramnamelineoutput = this._keyfieldline;
    //        this._paramnamelineupdate = string.Format("Original_{0}", this._keyfieldline);

    //        this._paramnamevatoutput = this._keyfieldvat;
    //        this._paramnamevatupdate = string.Format("Original_{0}", this._keyfieldvat);

    //        appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._metaname.ToLower()));
    //        appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
    //        appajaxoption.ajaxoption.Add("metaname", this._metaname);
    //        appajaxoption.ajaxoption.Add("keyfield", this._keyfieldmaster);
    //        appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);

    //        this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();
    //        int roleid = Services.GlobalVariant.GetAppUser().RoleID;
    //        this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();
    //    }
    //    public virtual void InitObject() { }

    //    public virtual object AutoComplete()
    //    {
    //        return Data.GetByKeyword(this._request);
    //    }

    //    public virtual object FieldChange()
    //    {
    //        return Services.Data.GetByCode(this._request);
    //    }

    //    #region kiểm tra kiểu nhập dữ liệu
        
    //    public virtual void ValidateMaster<T>(T data)
    //    {

    //        #region Business logic validate
    //        #region Object validate

    //        foreach (var item in this._metaobject.MetaTable)
    //        {
    //            string fieldmap = string.Format("{0}", item.ColumnName.ToString());
    //            if (item.AllowDBNull != true)
    //            {
    //                var value = data.GetType().GetProperty(fieldmap).GetValue(data, null) ?? String.Empty;

    //                if (string.IsNullOrEmpty(value.ToString()))
    //                {
    //                    this.Errors.Add(fieldmap, string.Format("phải nhập {0}", item.Des.ToLower()));
    //                }
    //            }
    //        }
    //        #endregion

    //        #endregion

    //        if (this.Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");

    //    }

    //    public virtual void ValidateLine<T>(ICollection<T> data, ICollection<long> dataz)
    //    {
    //        //nếu không có chi tiết thì không được lưu
    //        if (data == null)
    //        {
    //            this.Errors.Add(this._metalinename, "Phải nhập chi tiết hạch toán");
    //        }
    //        else
    //        {
    //            var line = data.ToList();
    //            var linez = dataz.ToList();

    //            bool notline = true;//ngầm định chi tiết đã bị xóa hết
    //            for (int i = 0; i < line.Count; i++)
    //            {
    //                var itemz = linez[i];
    //                if (itemz != -1)
    //                {
    //                    var dataline = line[i];
    //                    foreach (var item in this._metaobject.MetaTable)
    //                    {
    //                        string fieldmap = string.Format("{0}", item.ColumnName.ToString());
    //                        if (item.AllowDBNull != true)
    //                        {
    //                            var value = dataline.GetType().GetProperty(fieldmap).GetValue(dataline, null) ?? String.Empty;

    //                            if (string.IsNullOrEmpty(value.ToString()))
    //                            {
    //                                string fielderror = string.Format("{0}s[{1}].{2}", this._metalinename, i, item.ColumnName);
    //                                this.Errors.Add(fielderror, string.Format("phải nhập {0}", item.Des.ToLower()));
    //                            }
    //                        }
    //                    }
    //                    notline = false;
    //                }

    //            }
    //            if (notline)
    //            {
    //                this.Errors.Add(this._metalinename, "Phải nhập chi tiết hạch toán");
    //            }
    //        }
    //    }

    //    public virtual void ValidateVAT<T>(ICollection<T> data, ICollection<long> dataz)
    //    {
    //        if (data != null)
    //        {
    //            var vat = data.ToList();
    //            var vatz = dataz.ToList();

    //            for (int i = 0; i < vat.Count; i++)
    //            {
    //                var itemz = vatz[i];
    //                if (itemz != -1)
    //                {
    //                    var datavat = vat[i];
    //                    foreach (var item in this._metaobject.MetaTable)
    //                    {
    //                        string fieldmap = string.Format("{0}", item.ColumnName.ToString());
    //                        if (item.AllowDBNull != true)
    //                        {
    //                            var value = datavat.GetType().GetProperty(fieldmap).GetValue(datavat, null) ?? String.Empty;

    //                            if (string.IsNullOrEmpty(value.ToString()))
    //                            {
    //                                string fielderror = string.Format("{0}s[{1}].{2}", this._metavatname, i, item.ColumnName);
    //                                this.Errors.Add(fielderror, string.Format("phải nhập {0}", item.Des.ToLower()));
    //                            }
    //                        }
    //                    }
    //                }

    //            }

    //        }
    //    }

    //    #endregion

    //    #region kiểm tra nghiệp vụ
        
    //    public virtual void ValidateInsert<T>(T data)
    //    {

    //        #region Business logic validate
    //        #region Object validate
    //        var documentid = data.GetType().GetProperty("DocumentID").GetValue(data, null) ?? String.Empty;
    //        var voucherdate = data.GetType().GetProperty("VoucherDate").GetValue(data, null) ?? String.Empty;
    //        var vouchernumber = data.GetType().GetProperty("VoucherNumber").GetValue(data, null) ?? String.Empty;
    //        var voucherid = data.GetType().GetProperty("VoucherID").GetValue(data, null) ?? String.Empty;

    //        if (!string.IsNullOrEmpty(documentid.ToString()) && !string.IsNullOrEmpty(voucherdate.ToString()))
    //        {
    //            if (!Services.Voucher.CheckLockDate((long)documentid, (DateTime)voucherdate))
    //            {
    //                Errors.Add("VoucherDate", string.Format("Chứng từ đã bị khóa sổ đến ngày {0}", Services.GlobalVariant.GetSysOption()["LockDate"].ToString()));
    //            }

    //            if (!string.IsNullOrEmpty(vouchernumber.ToString()) && !string.IsNullOrEmpty(voucherid.ToString()))
    //            {
    //                if (!Services.Voucher.CheckVoucherNumber((long)documentid, (int)voucherid, vouchernumber.ToString(), (DateTime)voucherdate))
    //                {
    //                    Errors.Add("VoucherNumber", string.Format("{0}", "Trùng số chứng từ"));
    //                }

    //            }

    //        }


    //        #endregion
    //        if (Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");
    //        #endregion

    //    }

    //    public virtual void ValidateDelete(long documentid)
    //    {

    //        #region Business logic validate
    //        #region Object validate
    //        if (!Services.Voucher.CheckLockDate(documentid,DateTime.Today))
    //        {
    //            Errors.Add("VoucherDate", string.Format("Chứng từ đã bị khóa sổ đến ngày {0}", Services.GlobalVariant.GetSysOption()["LockDate"].ToString()));
    //        }

    //        #endregion
    //        if (Errors.Count > 0) throw new InvalidOperationException("Không xóa được");
    //        #endregion

    //    }

    //    public virtual void ValidateUpdate<T>(T data)
    //    {

    //        #region Business logic validate
    //        #region Object validate
    //        var documentid = data.GetType().GetProperty("DocumentID").GetValue(data, null) ?? String.Empty;
    //        var voucherdate = data.GetType().GetProperty("VoucherDate").GetValue(data, null) ?? String.Empty;
    //        var vouchernumber = data.GetType().GetProperty("VoucherNumber").GetValue(data, null) ?? String.Empty;
    //        var voucherid = data.GetType().GetProperty("VoucherID").GetValue(data, null) ?? String.Empty;

    //        if (!string.IsNullOrEmpty(documentid.ToString()) && !string.IsNullOrEmpty(voucherdate.ToString()))
    //        {
    //            if (!Services.Voucher.CheckLockDate((long)documentid,(DateTime)voucherdate))
    //            {
    //                Errors.Add("VoucherDate", string.Format("Chứng từ đã bị khóa sổ đến ngày {0}",Services.GlobalVariant.GetSysOption()["LockDate"].ToString()));
    //            }

    //            if (!string.IsNullOrEmpty(vouchernumber.ToString()) && !string.IsNullOrEmpty(voucherid.ToString()))
    //            {
    //                if (!Services.Voucher.CheckVoucherNumber((long)documentid,(int)voucherid,vouchernumber.ToString(),(DateTime)voucherdate))
    //                {
    //                    Errors.Add("VoucherNumber", string.Format("{0}", "Trùng số chứng từ"));
    //                }

    //            }

    //        }


    //        #endregion
    //        if (Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");
    //        #endregion

    //    }

    //    #endregion

    //}
}