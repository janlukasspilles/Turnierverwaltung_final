using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using TVModelLib.Model.TeilnehmerNS;
using TVModelLib.Extensions;
using Turnierverwaltung.CustomControls;
using TVModelLib;

namespace Turnierverwaltung.View.Pages
{
    public partial class Einstellungen : Page
    {
        private Controller _controller;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            Controller.GetAlleMannschaften();
            gView.DataSource = Controller.Teilnehmer;
            gView.DataBind();
        }        
    }
}