using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using WebApp.Services.Tasks;
using WebApp.Models;



//using Microsoft.Owin;
//using Owin;
//using WebApp;

namespace WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class AppDecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            if ((String)controllerContext.RouteData.DataTokens["area"] != "Accounting" && (String)controllerContext.RouteData.DataTokens["area"] != "Admin")
            { return base.BindModel(controllerContext, bindingContext); }

            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                System.Globalization.CultureInfo CICC;
                if ((String)controllerContext.RouteData.DataTokens["area"] == "Admin")
                {
                    CICC = (System.Globalization.CultureInfo)WebApp.Areas.Admin.Services.GlobalVariant.GetCultureInfo()["CICC"];
                }
                else
                {
                    CICC = (System.Globalization.CultureInfo)WebApp.Areas.Accounting.Services.GlobalVariant.GetCultureInfo()["CICC"];
                }
                actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CICC);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }

    public class AppIntModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //
            if ((String)controllerContext.RouteData.DataTokens["area"] != "Accounting" && (String)controllerContext.RouteData.DataTokens["area"] != "Admin")
            { return base.BindModel(controllerContext, bindingContext); }

            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                System.Globalization.CultureInfo CICC;
                if ((String)controllerContext.RouteData.DataTokens["area"] == "Admin")
                {
                    CICC = (System.Globalization.CultureInfo)WebApp.Areas.Admin.Services.GlobalVariant.GetCultureInfo()["CICC"];
                }
                else
                {
                    var _CICC = WebApp.Areas.Accounting.Services.GlobalVariant.GetCultureInfo();
                    if (_CICC != null)
                    {
                        CICC = (System.Globalization.CultureInfo)_CICC["CICC"];
                    }
                    else
                    {
                        CICC = Thread.CurrentThread.CurrentCulture;
                    }
                }
                if (valueResult != null)
                {
                    var tmpValue = Convert.ToDecimal(valueResult.AttemptedValue, CICC);
                    actualValue = int.Parse(tmpValue.ToString());
                }

            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }
            if (bindingContext.ModelState[bindingContext.ModelName] == null)
            {
                bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            }
            else
            {
                bindingContext.ModelState[bindingContext.ModelName] = modelState;
            }
            return actualValue;
        }
    }

    public class AppDatetimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            if ((String)controllerContext.RouteData.DataTokens["area"] != "Accounting" && (String)controllerContext.RouteData.DataTokens["area"] != "Admin")
            { return base.BindModel(controllerContext, bindingContext); }

            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);
            ModelState modelState = new ModelState { Value = valueResult };
            object actualValue = null;
            try
            {
                System.Globalization.CultureInfo CICC;
                if ((String)controllerContext.RouteData.DataTokens["area"] == "Admin")
                {
                    CICC = (System.Globalization.CultureInfo)WebApp.Areas.Admin.Services.GlobalVariant.GetCultureInfo()["CICC"];
                }
                else
                {
                    CICC = (System.Globalization.CultureInfo)WebApp.Areas.Accounting.Services.GlobalVariant.GetCultureInfo()["CICC"];
                }

                actualValue = Convert.ToDateTime(valueResult.AttemptedValue, CICC);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;


        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            #region khởi tạo hệ thống sen việt

            var db = new SenContext();
            WebApp.Services.Helpers.IWebHelper webhelper = new WebApp.Services.Helpers.WebHelper(new HttpContextWrapper(Context));

            #region khai báo biến toàn cục

            WebApp.Services.GlobalVariant.useSsl = false;
            WebApp.Services.GlobalVariant.UrlRoot = db.SysOptions.Where(m => m.SysOptionName == "UrlRoot").SingleOrDefault().SysOptionValue;
            WebApp.Services.GlobalVariant.LanguageId = int.Parse(db.SysOptions.Where(m => m.SysOptionName == "LanguageId").SingleOrDefault().SysOptionValue);

            //string.Format("{0}://{1}", Context.Request.Url.Scheme, Context.Request.Url.Authority);// webhelper.GetStoreHost(WebApp.Services.GlobalVariant.useSsl);// string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority);
            WebApp.Services.GlobalVariant.EmailAccount = db.SenEmailAccounts.FirstOrDefault();

            WebApp.Services.GlobalToken.setToken("Company.Name", "Phần mềm Sen Việt");
            /*
            Account.ChangePasswordURL
            Company.Name
            */
            #endregion


            TaskManager.Instance.Initialize();
            TaskManager.Instance.Start();


            ModelBinders.Binders.Add(typeof(decimal), new AppDecimalModelBinder());
            ModelBinders.Binders.Add(typeof(Nullable<decimal>), new AppDecimalModelBinder());

            ModelBinders.Binders.Add(typeof(int), new AppIntModelBinder());
            ModelBinders.Binders.Add(typeof(Nullable<int>), new AppIntModelBinder());

            ModelBinders.Binders.Add(typeof(DateTime), new AppDatetimeModelBinder());
            ModelBinders.Binders.Add(typeof(Nullable<DateTime>), new AppDatetimeModelBinder());
            #endregion


        }

        //protected void Application_Error(Object sender, EventArgs e)
        //{
        //    var exception = Server.GetLastError();

        //    //log error
        //    LogException(exception);

        //    //process 404 HTTP errors
        //    var httpException = exception as HttpException;
        //    if (httpException != null && httpException.GetHttpCode() == 404)
        //    {

        //        Response.Clear();
        //        Server.ClearError();
        //        Response.TrySkipIisCustomErrors = true;

        //        // Call target Controller and pass the routeData.
        //        IController errorController = new WebApp.Controllers.CommonController();


        //        var routeData = new RouteData();
        //        routeData.Values.Add("controller", "Common");
        //        routeData.Values.Add("action", "PageNotFound");

        //        errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //    }
        //}

        //protected void LogException(Exception exc)
        //{
        //    if (exc == null)
        //        return;

        //    //if (!DataSettingsHelper.DatabaseIsInstalled()) return;

        //    ////ignore 404 HTTP errors
        //    //var httpException = exc as HttpException;
        //    //if (httpException != null && httpException.GetHttpCode() == 404 &&
        //    //    !EngineContext.Current.Resolve<CommonSettings>().Log404Errors)
        //    //    return;

        //    try
        //    {
        //        //log

        //        //var logger = EngineContext.Current.Resolve<ILogger>();
        //        //var workContext = EngineContext.Current.Resolve<IWorkContext>();
        //        //logger.Error(exc.Message, exc, workContext.CurrentCustomer);

        //        var _db = new Models.SenContext();
        //        var httpcontextbase = new System.Web.HttpContextWrapper(Context);
        //        ILogger logger = new DefaultLogger(_db, httpcontextbase);
        //        logger.Error(exc.Message, exc, Membership.GetUser());

        //    }
        //    catch (Exception)
        //    {
        //        //don't throw new exception if occurs
        //    }
        //}
    }






    //public class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        app.MapSignalR();
    //    }
    //}


}