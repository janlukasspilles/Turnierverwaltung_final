using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;

namespace Turnierverwaltung.View.Pages
{
    public partial class Anmeldung : Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
        }

        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {            
            if (Controller.CheckLogin((sender as Login).UserName, (sender as Login).Password))
                e.Authenticated = true;
        }
    }
}