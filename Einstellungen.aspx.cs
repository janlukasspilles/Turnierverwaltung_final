using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Turnierverwaltung_final
{
    public partial class Einstellungen : System.Web.UI.Page
    {
        private List<Control> _detailFelder;

        public List<Control> DetailFelder 
        {
            get 
            {
                if (_detailFelder == null)
                    _detailFelder = new List<Control>();
                return _detailFelder; 
            }
            set => _detailFelder = value; 
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateControls();
        }

        protected void CreateControls()
        {
        }

        protected void btn_add_property_Click(object sender, EventArgs e)
        {
        }
    }
}