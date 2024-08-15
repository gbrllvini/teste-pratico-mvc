using System.Web.Mvc;
using System.Web.Routing;

namespace TestePraticoMvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Habilitar o roteamento por atributos
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Pessoas", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
