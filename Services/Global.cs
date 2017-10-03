using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;

namespace WebApp.Services
{
    public static class CacheKey
    {
        public static string UserContextKeys = "UserContextKey";

        public static string GlobalSysOptionKeys = "WebAppSysOption";
        public static string GlobalMetaKeys = "WebAppGlobeMeta";
        public static string GlobalCultureInfoKeys = "WebAppCultureInfo";
    }

    public class GlobalVariant
    {
        public static bool useSsl { get; set; }
        public static string UrlRoot { get; set; }
        public static int LanguageId { get; set; }
        public static SenEmailAccount EmailAccount { get; set; }

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
            else
            {
                InitVariant();
                return (Hashtable)HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys];
            }

            //return null;
        }

        public static void InitVariant()
        {

            Models.SenContext db = new SenContext();

            HttpContext.Current.Session[CacheKey.GlobalSysOptionKeys] = new Hashtable();
            HttpContext.Current.Session[CacheKey.GlobalCultureInfoKeys] = new Hashtable();

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

    public static class GlobalToken
    {
        //dùng đễ email khách hàng

        private static Dictionary<string, string> tokens = new Dictionary<string, string>();
        public static void setToken(string key, string value)
        {
            if (!tokens.ContainsKey(key))
            {
                tokens.Add(key, value);
            }
            else
            {
                tokens[key] = value;
            }
        }
        public static string MappingToken(string template)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException("template");

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            foreach (var token in tokens)
            {
                string tokenValue = token.Value;
                //do not encode URLs
                //if (htmlEncode && !token.NeverHtmlEncoded) tokenValue = HttpUtility.HtmlEncode(tokenValue);
                template = Replace(template, String.Format(@"%{0}%", token.Key), tokenValue);
            }
            return template;
        }
        private static  string Replace(string original, string pattern, string replacement)
        {
            StringComparison _stringComparison = StringComparison.OrdinalIgnoreCase;
            if (_stringComparison == StringComparison.Ordinal)
            {
                return original.Replace(pattern, replacement);
            }
            else
            {
                int count, position0, position1;
                count = position0 = position1 = 0;
                int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
                char[] chars = new char[original.Length + Math.Max(0, inc)];
                while ((position1 = original.IndexOf(pattern, position0, _stringComparison)) != -1)
                {
                    for (int i = position0; i < position1; ++i)
                        chars[count++] = original[i];
                    for (int i = 0; i < replacement.Length; ++i)
                        chars[count++] = replacement[i];
                    position0 = position1 + pattern.Length;
                }
                if (position0 == 0) return original;
                for (int i = position0; i < original.Length; ++i)
                    chars[count++] = original[i];
                return new string(chars, 0, count);
            }
        }
    }

    public static class GlobalErrors
    {

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
    }

    public static class GlobalUserContext
    {

        public static void SetContext()
        {
            var request = HttpContext.Current;
            if (string.IsNullOrEmpty(request.User.Identity.Name))
            {
                request.Session.Remove(CacheKey.UserContextKeys);
                return;
            }

            var usercontext = new UserContext(new SenContext(),Membership.GetUser(request.User.Identity.Name));
            request.Session[CacheKey.UserContextKeys] = usercontext;
        }

        public static UserContext GetContext()
        {
            var request = HttpContext.Current;
            if (request.Session[CacheKey.UserContextKeys] == null) SetContext();
            var usercontext = (UserContext)request.Session[CacheKey.UserContextKeys];
            return usercontext;
        }

        public static void AddProfile(string pname, string pvalue)
        {
            WebApp.Models.SenContext db = new WebApp.Models.SenContext();
            var senuser = WebApp.Services.GlobalUserContext.GetContext().SenUser;
            var editprofile = db.SenUserProfiles.Where(m => m.SenUserId == senuser.SenUserId && m.PropertyName == pname).SingleOrDefault();
            if (editprofile==null)
            {
                var addprofile = new WebApp.Models.SenUserProfile() { SenUserId = senuser.SenUserId, PropertyName = pname, PropertyValue = pvalue, LastUpdatedDate = DateTime.Now };
                db.SenUserProfiles.Add(addprofile);
            }
            else
            {
                editprofile.PropertyValue = pvalue;
                editprofile.LastUpdatedDate = DateTime.Now;
                db.Entry(editprofile).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        public static string GetProfile(string pname)
        {
            WebApp.Models.SenContext db = new WebApp.Models.SenContext();
            var senuser = WebApp.Services.GlobalUserContext.GetContext().SenUser;
            var profile = db.SenUserProfiles.Where(m => m.SenUserId == senuser.SenUserId && m.PropertyName == pname).SingleOrDefault();

            if (profile!=null)
            {
                return profile.PropertyValue;
            }

            return "";
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
                Models.SenContext db = new Models.SenContext();

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

    public static class GlobalAjax
    {
        public static Dictionary<string, string> GetAjaxOption(object ajaxoption)
        {
            Dictionary<string, string> _ajaxoption =(Dictionary<string, string>)ajaxoption;
            return _ajaxoption;
        }
        public static string StringAjaxOption(Dictionary<string, string> ajaxoption)
        {
            string ajaxstring = string.Format("data-ajax-update=#{0} ", ajaxoption["ajaxupdateid"]);
            ajaxstring += "data-ajax-mode=replace ";
            ajaxstring += "data-ajax-method=GET ";
            ajaxstring += string.Format("data-ajax-loading=#{0} ", ajaxoption["ajaxloadingid"]);
            ajaxstring += "data-ajax=true ";
            return ajaxstring;
        }
    }

    public static class Extension
    {
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }
    }
}