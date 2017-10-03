using System.Web.Mvc;

namespace WebApp.Areas.Accounting
{
    public class AccountingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Accounting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new {controller="Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
