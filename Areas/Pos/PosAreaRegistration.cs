using System.Web.Mvc;

namespace WebApp.Areas.Pos
{
    public class PosAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Pos";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Pos_default",
                "Pos/{controller}/{action}/{id}",
                new {controller="Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
