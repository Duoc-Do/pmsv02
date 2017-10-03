using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApp.DAL
{

    public abstract class SenVietListObject
    {
        public Models.SenContext _db;
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
            this._db = new Models.SenContext();
            this._request = request;
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

            this._paramnameoutput = this._keyfield;
            this._paramnameupdate = string.Format("Original_{0}", this._keyfield);

            appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._metaname.ToLower()));

            appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
            appajaxoption.ajaxoption.Add("metaname", this._metaname);
            appajaxoption.ajaxoption.Add("keyfield", this._keyfield);
            if (this._request!=null)
            {
                appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);
            }
            else
            {
                appajaxoption.ajaxoption.Add("action-return", "");
            }

            //this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();
            //int roleid = Services.GlobalVariant.GetAppUser().RoleID;
            //this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();
        }

        public virtual void InitObject() { }

        //public object AutoComplete()
        //{
        //    return Data.GetByKeyword(this._request);
        //}

        //public object FieldChange()
        //{
        //    return Services.Data.GetByCode(this._request);
        //}

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

    }

    public abstract class SenVietReportObject
    {
        public Models.SenContext _db;
        public HttpRequestBase _request;
        public Models.MetaObject _metaobject;
        public HttpSessionState _currentsession;

        public Dictionary<string, object> _params = new Dictionary<string, object>();
        public Models.AppAjaxOption appajaxoption = new Models.AppAjaxOption();
        //public List<Models.SysReport> sysreports;
        //public Models.SysBusiness sysbusiness;
        //public Models.SysBusinessRole sysbusinessrole;

        public string _businesscode;
        public string _metaname;
        public string _keyfield;

        public string _storeName;

        public string _sessionkeys;
        public string _sessionparamskeys;


        public SenVietReportObject(HttpRequestBase request)
        {
            this.InitObject();

            this._db = new Models.SenContext();
            this._request = request;
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);
            this._currentsession = HttpContext.Current.Session;

            this._sessionkeys = string.Format("{0}.Model", this._businesscode);
            this._sessionparamskeys = string.Format("{0}.Params", this._businesscode);

            appajaxoption.ajaxoption.Add("ajaxupdateid", string.Format("{0}_container", this._metaname.ToLower()));
            appajaxoption.ajaxoption.Add("businesscode", this._businesscode);
            appajaxoption.ajaxoption.Add("metaname", this._metaname);
            appajaxoption.ajaxoption.Add("keyfield", this._keyfield);
            appajaxoption.ajaxoption.Add("action-return", this._request["action_return"]);

            //this.sysreports = this._db.SysReports.Where(m => m.BusinessCode == this._businesscode).OrderBy(m => m.IsDefault).ToList();
            //this.sysbusiness = this._db.SysBusinesses.Where(m => m.BusinessCode == this._businesscode).SingleOrDefault();

            //int roleid = Services.GlobalVariant.GetAppUser().RoleID;
            //this.sysbusinessrole = this._db.SysBusinessRoles.Where(m => m.BusinessCode == this._businesscode & m.RoleID == roleid).SingleOrDefault();

        }
        public virtual void InitObject() { }

        public virtual Dictionary<string, object> GetParamCache()
        {
            var parasession = (Dictionary<string, object>)this._currentsession[this._sessionparamskeys];
            return parasession;
        }

        public virtual void SetParamCache(Dictionary<string, object> _paramcache)
        {
            this._currentsession[this._sessionparamskeys] = _paramcache;
        }

        public virtual void SetDataCache<T>(T data)
        {
            this._currentsession[this._sessionkeys] = data;
        }
        public bool IsSession()
        {
            return this._currentsession[this._sessionkeys] != null;
        }

        public List<T> GetDataCache<T>()
        {
            var result = (List<T>)this._currentsession[this._sessionkeys];
            return result;
        }

        public List<T> GetDataCacheAll<T>()
        {
            var result = this.GetDataCache<T>();
            var model = Services.Helpers.GridHelper.GetResultAlls(this._request, this._metaname, result.AsQueryable<T>());
            return model;
        }


        public object AutoComplete()
        {
            return Services.Data.GetByKeyword(this._request);
        }

        public object FieldChange()
        {
            return Services.Data.GetByCode(this._request);
        }

    }
}