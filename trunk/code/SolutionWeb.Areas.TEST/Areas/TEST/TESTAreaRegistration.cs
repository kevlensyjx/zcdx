using System.Web.Http;
using System.Web.Mvc;

namespace SolutionWeb.Areas.TEST
{
    public class TESTAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "TEST";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_default",
                this.AreaName + "/{controller}/{action}/{id}/{*catchall}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "SxshWeb.Areas." + this.AreaName + ".Controllers" }
            );
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                this.AreaName + "Api",
                "api/" + this.AreaName + "/{controller}/{action}/{id}",
                new
                {
                    action = RouteParameter.Optional,
                    id = RouteParameter.Optional,
                    namespaceName = new string[] { "SxshWeb.Areas." + this.AreaName + ".Controllers" }
                },
                null
            );
        }
    }
}