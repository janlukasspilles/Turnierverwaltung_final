using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using Turnierverwaltung.ControllerNS;
using TVModelLib.Database;

namespace Turnierverwaltung
{
    public class Global : HttpApplication
    {
        private static Controller _controller;
        public static Controller Controller { get => _controller; set => _controller = value; }

        void Application_Start(object sender, EventArgs e)
        {
            // Code, der beim Anwendungsstart ausgeführt wird
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            DatabaseCreator.GenerateDatabase();

            Controller = new Controller();
        }
    }
}