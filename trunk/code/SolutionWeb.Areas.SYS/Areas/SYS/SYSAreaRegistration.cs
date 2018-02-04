using System.Web.Http;
using System.Web.Mvc;

namespace SolutionWeb.Areas.SYS
{
    public class SYSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SYS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}/{*catchall}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SolutionWeb.Areas." + this.AreaName + ".Controllers" }
            );
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                this.AreaName + "Api",
                "api/" + this.AreaName + "/{controller}/{action}/{id}",
                new
                {
                    action = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    namespaceName = new string[] { "SolutionWeb.Areas." + this.AreaName + ".Controllers" }
                },
                null
            );
        }
    }
}