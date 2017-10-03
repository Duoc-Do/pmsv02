using System.Web.Mvc;

namespace WebApp.Areas.Res
{
    public class ResAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Res";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Res_default",
                "Res/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
