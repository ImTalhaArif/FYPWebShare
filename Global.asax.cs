using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TheFinalProduct_FYP_
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
        
            RouteConfig.RegisterRoutes(RouteTable.Routes);

           // SqlDependency.Start(@"Data Source = DESKTOP - O4JDQJU\SQLEXPRESS; initial catalog = dbFYP; integrated security = True");
        }

    }
}
