using System.Web.Mvc;
using System.Web.Routing;

namespace WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
    name: "sendpm",
    url: "sendpm/{userid}",
    defaults: new { controller = "Account", action = "PrivateMessageSend", userid = UrlParameter.Optional }
    );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
    name: "HomePage",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Contract", action = "Index", id = UrlParameter.Optional, area = "" }
);


            routes.MapRoute(
            name: "PMSContracts",
            url: "pmscontracts/{controller}/{action}",
            defaults: new { controller = "Contract", action = "Index" }
        );
            //đăng nhập app kế toán
            routes.MapRoute(
                name: "AppAccLogin",
                url: "Accounting/AppAccount/Login");
            //đăng nhập app pos online
            routes.MapRoute(
                name: "AppPosLogin",
                url: "Pos/AppAccount/Login");
            //đăng nhập app res online
            routes.MapRoute(
                name: "AppResLogin",
                url: "Res/AppAccount/Login");
            //đăng nhập app gap online
            routes.MapRoute(
                name: "AppGapLogin",
                url: "Gap/AppAccount/Login");

            //upload hình
            routes.MapRoute(
            name: "UploadImage",
            url: "Common/AsyncUpload");

        }
    }
}