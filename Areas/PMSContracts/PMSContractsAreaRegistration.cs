using System.Web.Mvc;

namespace WebApp.Areas.PMSContracts
{
    public class PMSContractsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PMSContracts";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PMSContracts_default",
                "PMSContracts/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
