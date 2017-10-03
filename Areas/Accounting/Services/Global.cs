using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Accounting.Models;

namespace WebApp.Areas.Accounting.Services
{
    public static class CacheKey
    {
        public static string UserKeys = "AppAccountingUserKey";
        public static string ConnectionKeys = "AppAccConnectionString";
        public static string CompanyKeys = "AppAccCompanyName";
        public static string CompanyIdKeys = "AppAccCompanyId";
        public static string GlobalMetaKeys = "AppAccGlobeMeta";

        public static string GlobalSysCompanyKeys = "AppAccSysCompany";
        public static string GlobalSysOptionKeys = "AppAccSysOption";
        public static string GlobalCultureInfoKeys = "AppAccCultureInfo";

    }

    public static class GlobalVariant
    {
        public static string AjaxInfo = "data-ajax=true data-ajax-loading=#ajaxloadingelementid data-ajax-method=GET data-ajax-mode=replace data-ajax-success=ajaxmenu_onsuccess data-ajax-update=#page-body-contain-id";

        public static bool IsLogin()
        {
            if (String.IsNullOrEmpty(GetConnection()))
                return false;

            if (GetAppUser() == null)
                return false;

            InitVariant();
            return true;
        }

        public static string GetConnection()
        {

            if (HttpContext.Current.Session[CacheKey.ConnectionKeys] != null)
            {
                return HttpContext.Current.Session[CacheKey.ConnectionKeys].ToString();
            }
            else
            {
                return "";
            }

        }

        public static void SetConnection(string connectionstring)
        {
            HttpContext.Current.Session[CacheKey.ConnectionKeys] = connectionstring;
        }

        public static string GetCompanyName()
        {
            if (HttpContext.Current.Session[CacheKey.CompanyKeys] != null)
            {
                return HttpContext.Current.Session[CacheKey.CompanyKeys].ToString();
            }
            else
            {
                return "";
            }

        }

        public static void SetCompanyName(string companyname)
        {
            HttpContext.Current.Session[CacheKey.CompanyKeys] = companyname;
        }

        public static string GetCompanyId()
        {
            if (HttpContext.Current.Session[CacheKey.CompanyIdKeys] != null)
            {
                return HttpContext.Current.Session[CacheKey.CompanyIdKeys].ToString();
            }
            else
            {
                return "";
            }

        }

        public static void SetCompanyId(string companyid)
        {
            HttpContext.Current.Session[CacheKey.CompanyIdKeys] = companyid;
        }


        public static SysUserView GetAppUser()
        {
            if (HttpContext.Current.Session[CacheKey.UserKeys] != null)
            {
                return (SysUserView)HttpContext.Current.Session[CacheKey.UserKeys];
            }
            else
            {
                return null;
            }

        }

        public static SysUserView GetAppUserById(int? UserID)
        {


            var db = new WebApp.Areas.Accounting.Models.WebAppAccEntities(GlobalVariant.GetConnection());
            if (db != null)
            {
                int _UserID = UserID ?? 0;
                return db.SysUserViews.SingleOrDefault(m => m.UserID == _UserID);
            }
            else
            {
                return null;
            }

        }

        public static void SetAppUser(SysUserView user)
        {
            HttpContext.Current.Session[CacheKey.UserKeys] = user;
        }

        public static Hashtable GetSysCompany()
        {
            if (HttpContext.Current.Session[CacheKey.GlobalSysCompanyKeys] != null)
            {
                return (Hashtable)HttpContext.Current.Session[CacheKey.GlobalSysCompanyKeys];
            }

            return null;
        }

        public static Hashtable GetSysOption()
        {
            if (HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys] != null)
            {
                return (Hashtable)HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys];
            }

            return null;

        }

        public static Hashtable GetCultureInfo()
        {
            if (HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys] != null)
            {
                return (Hashtable)HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys];
            }

            return null;
        }

        public static void InitVariant()
        {
            //SenViet.Core.Models.SysEntities sl = new SenViet.Core.Models.SysEntities();

            Models.WebAppAccEntities db = new WebAppAccEntities(GetConnection());

            HttpContext.Current.Session[CacheKey.GlobalSysCompanyKeys] = new Hashtable();
            HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys] = new Hashtable();
            HttpContext.Current.Session[CacheKey.GlobalCultureInfoKeys] = new Hashtable();

            //#region Khởi tạo SysCompany
            var SysCompanyList = db.SysCompanies.ToList();


            foreach (var item in SysCompanyList)
            {
                GetSysCompany().Add(item.EnumValue, item.EnumName);
            }

            //#endregion

            //#region Khởi tạo SysOption

            var SysOptionList = db.SysOptions.ToList();

            foreach (var item in SysOptionList)
            {
                GetSysOption().Add(item.SysOptionName, item.SysOptionValue);
            }
            //#endregion

            #region Khởi tạo CultureInfo
            ///tiền công ty. 
            System.Globalization.CultureInfo CICC = new System.Globalization.CultureInfo("en-US");
            CICC.NumberFormat = new System.Globalization.CultureInfo("vi-VN").NumberFormat;
            CICC.DateTimeFormat = new System.Globalization.CultureInfo("vi-VN").DateTimeFormat;
            CICC.NumberFormat.CurrencySymbol = GetSysOption()["CurrencySymbol"].ToString();

            ///tiền ngoại tệ.
            System.Globalization.CultureInfo CIFC = new System.Globalization.CultureInfo("vi-VN");
            ///giá theo tiền công ty.
            System.Globalization.CultureInfo CIUPCC = new System.Globalization.CultureInfo("vi-VN");
            ///giá theo tiền ngoại tệ.
            System.Globalization.CultureInfo CIUPFC = new System.Globalization.CultureInfo("vi-VN");

            CIFC.NumberFormat.NumberDecimalDigits = int.Parse(GetSysOption()["RoundAmountFC"].ToString());
            CIFC.NumberFormat.CurrencyDecimalDigits = int.Parse(GetSysOption()["RoundAmountFC"].ToString());
            CIFC.NumberFormat.CurrencySymbol = "";
            CIFC.NumberFormat.NumberDecimalSeparator = GetSysOption()["NumberDecimalSeparator"].ToString();
            CIFC.NumberFormat.NumberGroupSeparator = GetSysOption()["NumberGroupSeparator"].ToString();

            CIUPFC.NumberFormat.NumberDecimalSeparator = GetSysOption()["NumberDecimalSeparator"].ToString();
            CIUPFC.NumberFormat.NumberGroupSeparator = GetSysOption()["NumberGroupSeparator"].ToString();
            CIUPFC.NumberFormat.NumberDecimalDigits = int.Parse(GetSysOption()["RoundUnitPriceFC"].ToString());
            CIUPFC.NumberFormat.CurrencyDecimalDigits = int.Parse(GetSysOption()["RoundUnitPriceFC"].ToString());
            CIUPFC.NumberFormat.CurrencySymbol = "";

            CICC.NumberFormat.NumberDecimalDigits = int.Parse(GetSysOption()["RoundQuantity"].ToString());
            CICC.NumberFormat.CurrencyDecimalDigits = int.Parse(GetSysOption()["RoundAmount"].ToString());
            CICC.NumberFormat.NumberDecimalSeparator = GetSysOption()["NumberDecimalSeparator"].ToString();
            CICC.NumberFormat.NumberGroupSeparator = GetSysOption()["NumberGroupSeparator"].ToString();

            CIUPCC.NumberFormat.NumberDecimalDigits = int.Parse(GetSysOption()["RoundUnitPrice"].ToString());
            CIUPCC.NumberFormat.CurrencyDecimalDigits = int.Parse(GetSysOption()["RoundUnitPrice"].ToString());
            CIUPCC.NumberFormat.NumberDecimalSeparator = GetSysOption()["NumberDecimalSeparator"].ToString();
            CIUPCC.NumberFormat.NumberGroupSeparator = GetSysOption()["NumberGroupSeparator"].ToString();


            GetCultureInfo().Add("CICC", CICC);//Định dạng tiền
            GetCultureInfo().Add("CIFC", CIFC);//Định dạng tiền ngoại tệ
            GetCultureInfo().Add("CIUPCC", CIUPCC);//Định dạng giá
            GetCultureInfo().Add("CIUPFC", CIUPFC);// Định dạng giá ngoại tệ

            Thread.CurrentThread.CurrentUICulture = CICC;
            Thread.CurrentThread.CurrentCulture = CICC;

            #endregion

        }
    }

    public static class GlobalMeta
    {
        //public static Dictionary<string, List<SenViet.Areas.SysAdmin.Models.SysTableDetailView>> MetaData = new Dictionary<string, List<SenViet.Areas.SysAdmin.Models.SysTableDetailView>>();
        //public static Dictionary<string,SenViet.Models.MetaObject> MetaObjects = new Dictionary<string,Models.MetaObject>();

        public static void SetMetaObject(string tablename)
        {
            //Dictionary<string, SenViet.Models.MetaObject> MetaObjects = null;

            Dictionary<string, Models.MetaObject> MetaObjects = null;

            if (HttpContext.Current.Session[CacheKey.GlobalMetaKeys] == null)
            {
                MetaObjects = new Dictionary<string, Models.MetaObject>();
                HttpContext.Current.Session[CacheKey.GlobalMetaKeys] = MetaObjects;
            }
            else
            {
                MetaObjects = (Dictionary<string, Models.MetaObject>)HttpContext.Current.Session[CacheKey.GlobalMetaKeys];
            }


            if (!MetaObjects.ContainsKey(tablename))
            {

                var MetaObject = new Models.MetaObject();
                Models.WebAppAccEntities db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

                MetaObject.MetaTable = db.SysTableDetailViews.Where(m => m.TableName == tablename).ToList();
                MetaObject.Paging = new Models.Paging();

                MetaObjects.Add(tablename, MetaObject);


            }
            //else // dành cho phát triển sau này phải bỏ
            //{
            //    var MetaObject = new SenViet.Models.MetaObject();
            //    SenViet.Areas.SysAdmin.Models.SysAdminEntities _slmeta = new SenViet.Areas.SysAdmin.Models.SysAdminEntities();
            //    var _result = _slmeta.SysTableDetailViews.Where(m => m.TableName == tablename).ToList();
            //    MetaObject.MetaTable = _result;
            //    MetaObject.Paging = new Models.Paging();

            //    MetaObjects.Remove(tablename);
            //    MetaObjects.Add(tablename, MetaObject);
            //}

        }

        public static Models.MetaObject GetMetaObject(string tablename)
        {

            SetMetaObject(tablename);
            //return MetaObjects[tablename]; // trả về đối tượng trong cache server
            return (Models.MetaObject)((Dictionary<string, Models.MetaObject>)HttpContext.Current.Session[CacheKey.GlobalMetaKeys])[tablename]; //trả về đối tượng trong session
        }

    }

    public static class ModelMeta
    {
        public static Dictionary<string, Dictionary<string, string>> GetModelMetadata(System.Reflection.PropertyInfo[] proinfo)
        {
            //Lấy DisplayName của một trường.
            //string a = ModelMetadata.FromLambdaExpression<Models.CMSFeedbackMetaData, string>(x => x.CMSFeedbackName, new ViewDataDictionary<Models.CMSFeedbackMetaData>()).DisplayName;
            //ViewBag.test = a;

            Dictionary<string, Dictionary<string, string>> _ModelMeta = new Dictionary<string, Dictionary<string, string>> { };
            Dictionary<string, string> _MetaDisplayName = new Dictionary<string, string> { };

            foreach (var property in proinfo)
            {
                var attribute = property.GetCustomAttributes(typeof(System.ComponentModel.DisplayNameAttribute), false)
                                        .Cast<System.ComponentModel.DisplayNameAttribute>()
                                        .FirstOrDefault();
                if (attribute != null)
                {
                    _MetaDisplayName.Add(property.Name, attribute.DisplayName);
                }
                else
                {
                    _MetaDisplayName.Add(property.Name, property.Name);
                }
            }

            _ModelMeta.Add("DisplayName", _MetaDisplayName);


            return _ModelMeta;
        }

        //public static List<SenViet.Models.SysTableDetail> GetMetaData(string TableName)
        //{
        //    SenViet.Models.AdminViewDataEntities _slmeta = new SenViet.Models.AdminViewDataEntities();

        //    var _result = _slmeta.SysTableDetails.Where(m => m.TableName == TableName).ToList();

        //    return _result;
        //}

        public static string GetFieldName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
        {
            char[] delimiterChars = { '.' };
            string[] arrexpression = ExpressionHelper.GetExpressionText(expression).Split(delimiterChars);
            if (arrexpression.Count() > 0)
            {
                return arrexpression[arrexpression.Count() - 1];
            }
            else
            {
                return "";
            }
        }
    }

    public static class GlobalErrors
    {
        public static void Parse(ModelStateDictionary modelState, Dictionary<string, string> errors, Exception ex)
        {
            //modelState.Clear();
            foreach (var item in modelState)
            {
                item.Value.Errors.Clear();
            }

            foreach (var item in errors)
            {
                if (modelState[item.Key] != null)
                {
                    modelState[item.Key].Errors.Add(item.Value);
                }
                else
                {
                    modelState.AddModelError(item.Key, item.Value);
                }
            }

            if (ex.InnerException == null)
            {
                modelState.AddModelError(ex.Message, Translate(ex.Message));
            }
            else
            {
                modelState.AddModelError(ex.Message, Translate(ex.InnerException.Message));
            }
        }

        public static void Parse(ModelStateDictionary modelState, Exception ex)
        {
            if (ex.InnerException == null)
            {
                modelState.AddModelError(ex.Message, ex.Message);
            }
            else
            {
                modelState.AddModelError(ex.Message, ex.InnerException.Message);
            }
        }

        private static string Translate(string ex)
        {
            string _ex = ex;
            if (ex.Contains("The DELETE statement conflicted with the REFERENCE constraint ")) { _ex = "Đã có phát sinh không được xóa!"; }
            if (ex.Contains("Cannot insert duplicate key row in object")) { _ex = "Đối tượng đã có không thêm được!"; }

            return _ex;
        }
    }



    

    //public class AppModelBinder : DefaultModelBinder
    //{
    //    protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
    //    {
    //        return base.CreateModel(controllerContext, bindingContext, modelType);
    //    }

    //    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        if (bindingContext.ModelType == typeof(decimal) || bindingContext.ModelType == typeof(Nullable<decimal>))
    //        {
    //            ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
    //            ModelState modelState = new ModelState { Value = valueProviderResult };
    //            object actualValue = null;
    //            try
    //            {
    //                actualValue = Convert.ToDecimal(valueProviderResult.AttemptedValue,
    //                    Thread.CurrentThread.CurrentCulture);
    //            }
    //            catch (FormatException e)
    //            {
    //                modelState.Errors.Add(e);
    //            }

    //            return actualValue;

    //        }

    //        try
    //        {
    //            return base.BindModel(controllerContext, bindingContext);
    //        }
    //        catch (Exception)
    //        {
    //            return null;
    //        }
    //    }

    //}

}