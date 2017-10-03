using System.Web.Mvc;

namespace WebApp.Areas.Gap
{
    public class GapAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Gap";
            }
        }

        //ứng dụng dành cho nông nghiệp chuẩn VIETGAP
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Gap_default",
                "Gap/{controller}/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
