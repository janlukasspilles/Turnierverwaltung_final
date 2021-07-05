using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using TVModeLib.Model.TeilnehmerNS;

namespace Turnierverwaltung.CustomControls
{
    public class CustomSwitchPanel : Panel
    {
        #region Attributes        
        private List<Teilnehmer> _ds1;
        private List<Teilnehmer> _ds2;
        private string _headlineText;
        private EventHandler _leftButton_ClickCommand;
        private EventHandler _rightButton_ClickCommand;
        private EventHandler _submitButton_ClickCommand;
        private EventHandler _cancelButton_ClickCommand;
        #endregion
        #region Properties
        public List<Teilnehmer> Ds1 { get => _ds1; set => _ds1 = value; }
        public List<Teilnehmer> Ds2 { get => _ds2; set => _ds2 = value; }
        public string HeadlineText { get => _headlineText; set => _headlineText = value; }
        public EventHandler LeftButton_ClickCommand { get => _leftButton_ClickCommand; set => _leftButton_ClickCommand = value; }
        public EventHandler RightButton_ClickCommand { get => _rightButton_ClickCommand; set => _rightButton_ClickCommand = value; }
        public EventHandler CancelButton_ClickCommand { get => _cancelButton_ClickCommand; set => _cancelButton_ClickCommand = value; }
        public EventHandler SubmitButton_ClickCommand { get => _submitButton_ClickCommand; set => _submitButton_ClickCommand = value; }
        #endregion
        #region Constructors
        public CustomSwitchPanel() : base()
        {
            
        }

        
        #endregion
        #region Methods
        public override void DataBind()
        {
            CreateControls();
        }
        private void CreateControls()
        {
            HtmlGenericControl headline = new HtmlGenericControl("h2")
            {
                InnerText = $"{HeadlineText}",
                ID = "headlineMitgliederSwitch"
            };
            Controls.Add(headline);

            Table switcher = new Table()
            {
                ID = "switcherPanel",
                CssClass = "table",
                HorizontalAlign = HorizontalAlign.Left
            };
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();

            Panel leftBox = new Panel
            {
                ID = "pnlLeftBox"
            };

            Label headlineLeft = new Label()
            {
                ID = "lblLeftBox",
                Text = "Mitglieder"
            };

            leftBox.Controls.Add(headlineLeft);

            ListBox lb = new ListBox
            {
                ID = "lbListe1",
                CssClass = "form-control",
                DataSource = Ds1
            };
            lb.DataBind();

            leftBox.Controls.Add(lb);
            tc.Controls.Add(leftBox);
            tr.Cells.Add(tc);

            tc = new TableCell();
            Panel midBox = new Panel
            {
                ID = "pnlMidBox"
            };

            Button btn = new Button
            {
                CssClass = "btn",
                Text = "Add",
                ID = "btnAddMitglied"
            };
            btn.Click += LeftButton_ClickCommand;
            midBox.Controls.Add(btn);

            btn = new Button
            {
                CssClass = "btn",
                Text = "Remove",
                ID = "btnRemoveMitglied"
            };
            btn.Click += RightButton_ClickCommand;
            midBox.Controls.Add(btn);

            btn = new Button
            {
                CssClass = "btn",
                Text = "Speichern",
                ID = "btnSubmitMitglieder"
            };
            btn.Click += SubmitButton_ClickCommand;
            midBox.Controls.Add(btn);

            btn = new Button
            {
                CssClass = "btn",
                Text = "Verwerfen",
                ID = "btnCancelMitglieder"
            };
            btn.Click += CancelButton_ClickCommand;
            midBox.Controls.Add(btn);

            tc.Controls.Add(midBox);
            tr.Cells.Add(tc);

            tc = new TableCell();

            Panel rightBox = new Panel
            {
                ID = "pnlRightBox"
            };
            Label lblRightBox = new Label
            {
                ID = "lblRightBox",
                Text = "Mögliche Mitglieder"
            };
            rightBox.Controls.Add(lblRightBox);

            lb = new ListBox
            {
                ID = "lbListe2",
                CssClass = "form-control",
                DataSource = Ds2
            };
            lb.DataBind();
            rightBox.Controls.Add(lb);
            tc.Controls.Add(rightBox);
            tr.Cells.Add(tc);
            switcher.Rows.Add(tr);

            Controls.Add(switcher);
        }
        #endregion

    }
}