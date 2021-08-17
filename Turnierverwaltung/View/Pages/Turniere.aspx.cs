using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.CustomControls;
using TVModelLib.Model.TeilnehmerNS;
using TVModelLib.Model.TurniereNS;
using TVModelLib;
using TVModelLib.Extensions;

namespace Turnierverwaltung.View.Pages
{
    public partial class Turniere : Page
    {
        #region Attributes
        private Controller _controller;
        private CustomTable<Turnier> _turnierTable;
        private CustomSwitchPanel _teilnehmerSwitchPanel;
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }
        public List<Teilnehmer> Teilnehmer
        {
            get
            {
                if (ViewState["Teilnehmer"] != null)
                    return (List<Teilnehmer>)ViewState["Teilnehmer"];
                return null;
            }
            set => ViewState["Teilnehmer"] = value;
        }

        public List<Teilnehmer> MoeglicheTeilnehmer
        {
            get
            {
                if (ViewState["MoeglicheTeilnehmer"] != null)
                    return (List<Teilnehmer>)ViewState["MoeglicheTeilnehmer"];
                return null;
            }
            set => ViewState["MoeglicheTeilnehmer"] = value;
        }

        public int MitgliederAnzeige
        {
            get
            {
                if (ViewState["MitgliederAnzeige"] == null)
                    return -1;
                return (int)ViewState["MitgliederAnzeige"];
            }
            set => ViewState["MitgliederAnzeige"] = value;
        }
        #endregion
        #region Constructors
        public Turniere() : base()
        {

        }
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            _turnierTable = new CustomTable<Turnier>(Type.GetType($"TVModelLib.Model.Turniere.Turnier"), "TurnierTable");
            _turnierTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
            _turnierTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
            _turnierTable.AddButton_ClickCommand += OnAddButton_Click;
            _turnierTable.SubmitButton_ClickCommand += OnSubmitButton_Click;
            _turnierTable.CancelButton_ClickCommand += OnCancelButton_Click;
            _turnierTable.AdditionalRowButtons = GetAdditionalRowButtons();
            if (!IsPostBack)
                Controller.GetAlleTurniere();
            _turnierTable.DataSource = Controller.Turniere;
            _turnierTable.DataBind();

            pnl_turniere.Controls.Add(_turnierTable);

            if (MitgliederAnzeige != -1)
            {
                _teilnehmerSwitchPanel = new CustomSwitchPanel
                {
                    HeadlineText = $"Teilnehmerliste für Turnier: {Controller.Turniere[MitgliederAnzeige]}",
                    Ds1 = Teilnehmer,
                    Ds2 = MoeglicheTeilnehmer
                };
                _teilnehmerSwitchPanel.LeftButton_ClickCommand += OnLeftButton_Click;
                _teilnehmerSwitchPanel.RightButton_ClickCommand += OnRightButton_Click;
                _teilnehmerSwitchPanel.CancelButton_ClickCommand += OnCancelMitgliederButton_Click;
                _teilnehmerSwitchPanel.SubmitButton_ClickCommand += OnSubmitMitgliederButton_Click;
                _teilnehmerSwitchPanel.DataBind();
                pnl_teilnehmer.Controls.Add(_teilnehmerSwitchPanel);
            }
        }

        private List<Tuple<string, string, string, Action<object, CommandEventArgs>>> GetAdditionalRowButtons()
        {
            return new List<Tuple<string, string, string, Action<object, CommandEventArgs>>>
            {
                Tuple.Create("btnZeigeTeilnehmer", "Zeige Teilnehmer", "btn btn-secondary", new Action<object, CommandEventArgs>((o, e) => OnZeigeTeilnehmer_Click(o, e)))
            };
        }

        private void OnZeigeTeilnehmer_Click(object o, CommandEventArgs e)
        {
            MitgliederAnzeige = Convert.ToInt32(e.CommandArgument) - 1;
            Teilnehmer = Controller.Turniere[MitgliederAnzeige].TurnierTeilnehmer;
            MoeglicheTeilnehmer = Controller.GetMoeglicheTurnierteilnehmer(Controller.Turniere[MitgliederAnzeige].Id, Controller.Turniere[MitgliederAnzeige].Turnierart);
            _teilnehmerSwitchPanel = new CustomSwitchPanel()
            {
                HeadlineText = $"Mitgliederliste für {Controller.Turniere[MitgliederAnzeige]}"
            };
            _teilnehmerSwitchPanel.Ds1 = Teilnehmer;
            _teilnehmerSwitchPanel.Ds2 = MoeglicheTeilnehmer;
            _teilnehmerSwitchPanel.LeftButton_ClickCommand += OnLeftButton_Click;
            _teilnehmerSwitchPanel.RightButton_ClickCommand += OnRightButton_Click;
            _teilnehmerSwitchPanel.CancelButton_ClickCommand += OnCancelMitgliederButton_Click;
            _teilnehmerSwitchPanel.SubmitButton_ClickCommand += OnSubmitMitgliederButton_Click;
            _teilnehmerSwitchPanel.DataBind();
            pnl_teilnehmer.Controls.Clear();
            pnl_teilnehmer.Controls.Add(_teilnehmerSwitchPanel);
        }

        private void OnSubmitMitgliederButton_Click(object sender, EventArgs e)
        {
            Controller.Turniere[MitgliederAnzeige].TurnierTeilnehmer = Teilnehmer;
            Controller.Turniere[MitgliederAnzeige].Speichern();
        }

        private void OnCancelMitgliederButton_Click(object sender, EventArgs e)
        {
            Teilnehmer = (Controller.Turniere[MitgliederAnzeige] as Turnier).TurnierTeilnehmer;
            MoeglicheTeilnehmer = Controller.GetMoeglicheMitglieder((Controller.Turniere[MitgliederAnzeige] as Turnier).Id);
            ((ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe1")).DataSource = Teilnehmer;
            ((ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe2")).DataSource = MoeglicheTeilnehmer;
            ((ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe2")).DataBind();
            ((ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe1")).DataBind();
        }

        private void OnRightButton_Click(object sender, EventArgs e)
        {
            //Entfernen
            ListBox lbMoegliche = (ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe2");
            ListBox lbTatsaechliche = (ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe1");
            if (lbTatsaechliche.SelectedIndex != -1)
                Teilnehmer.MoveTo(Teilnehmer[lbTatsaechliche.SelectedIndex], MoeglicheTeilnehmer);
            lbMoegliche.DataBind();
            lbTatsaechliche.DataBind();
        }

        private void OnLeftButton_Click(object sender, EventArgs e)
        {
            //Hinzufügen
            ListBox lbMoegliche = (ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe2");
            ListBox lbTatsaechliche = (ListBox)_teilnehmerSwitchPanel.FindControlRecursive("lbListe1");
            if (lbMoegliche.SelectedIndex != -1)
                MoeglicheTeilnehmer.MoveTo(MoeglicheTeilnehmer[lbMoegliche.SelectedIndex], Teilnehmer);

            lbMoegliche.DataBind();
            lbTatsaechliche.DataBind();
        }
        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Queue<Turnier> deletequeue = new Queue<Turnier>();
            foreach (TableRow tmp in _turnierTable.Rows)
            {
                if (!(tmp is TableHeaderRow) && !(tmp is TableFooterRow))
                {
                    if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{_turnierTable.Rows.GetRowIndex(tmp)}") is CheckBox cb)
                    {
                        if (cb.Checked)
                        {
                            Turnier t = Controller.Turniere[_turnierTable.Rows.GetRowIndex(tmp) - 1];
                            t.Loeschen();
                            deletequeue.Enqueue(t);
                        }
                    }
                }
            }
            foreach (Turnier t in deletequeue)
            {
                Controller.Turniere.Remove(t);
            }
            _turnierTable.DataSource = Controller.Turniere;
            _turnierTable.DataBind();
        }
        private void OnCancelButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Controller.Turniere.RemoveAll(x => x.Id == 0);
            _turnierTable.DataSource = Controller.Turniere;
        }
        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Type ListDataType = _turnierTable.FallbackType ?? _turnierTable.ListDataType;
            foreach (int row in _turnierTable.RowsInEdit)
            {
                for (int i = 0; i < _turnierTable.DisplayFields.Count; i++)
                {
                    DisplayMetaInformation dmi = _turnierTable.DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                    if (dmi.Editable)
                    {
                        switch (dmi.ControlType)
                        {
                            case ControlType.ctEditText:
                                if (_turnierTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_turnierTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_turnierTable.DisplayFields[i].Name).SetValue(Controller.Turniere[row], Convert.ChangeType(value, _turnierTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDomain:
                                if (_turnierTable.Rows[row + 1].Cells[i].Controls[0] is DropDownList)
                                {
                                    int domainId = (_turnierTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                    ListDataType.GetProperty(_turnierTable.DisplayFields[i].Name).SetValue(Controller.Turniere[row], Convert.ChangeType(domainId, _turnierTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDate:
                                if (_turnierTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_turnierTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_turnierTable.DisplayFields[i].Name).SetValue(Controller.Turniere[row], Convert.ChangeType(value, _turnierTable.DisplayFields[i].PropertyType));
                                }
                                break;
                        }
                    }
                }
                if (Controller.Turniere[row].Id == 0)
                {
                    Controller.Turniere[row].Neuanlage();
                }
                else
                {
                    Controller.Turniere[row].Speichern();
                }
            }
            _turnierTable.DataSource = Controller.Turniere;
            _turnierTable.DataBind();
        }
        private void OnAddButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Turnier t = new Turnier();
            Controller.Turniere.Add(t);

            _turnierTable.DataSource = Controller.Turniere;
            _turnierTable.DataBind();
        }
        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            var tmp = Controller.Turniere.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (Controller.Turniere.SequenceEqual(tmp))
                Controller.Turniere = Controller.Turniere.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            else
                Controller.Turniere = tmp;

            _turnierTable.DataSource = Controller.Turniere;
            _turnierTable.DataBind();
        }
        private void OnTurnierDurchfuehren_Click(object sender, CommandEventArgs e)
        {
            //Turnier actTurnier = Controller.Turniere[Convert.ToInt32(e.CommandArgument) - 1];
            //actTurnier.ErzeugeSpiele(false);

            //pnl_spiele.Controls.Clear();

            //HtmlGenericControl headline = new HtmlGenericControl("h2")
            //{
            //    InnerText = $"Spiele von Turnier: {actTurnier.Turniername}",
            //    ID = "headlineMitgliederSwitch"
            //};
            //pnl_spiele.Controls.Add(headline);

            //Table spieleTable = new Table
            //{
            //    ID = $"tableSpiele",
            //    CssClass = "table table-bordered"
            //};

            //spieleTable.Rows.Add(GetHeaderRow(GetDisplayFields(actTurnier.Spiele[0].GetType())));            

            ////HACK: Gottlos wie kompliziert, deswegen so...
            //switch (System.Windows.Forms.MessageBox.Show("Wollen Sie das Turnier manuell durchführen?", "Bitte Auswählen", System.Windows.Forms.MessageBoxButtons.YesNo))
            //{
            //    case System.Windows.Forms.DialogResult.Yes:
            //        for (int memberNum = 0; memberNum < actTurnier.Spiele.Count; memberNum++)
            //        {
            //            spieleTable.Rows.Add(GetDataRow(actTurnier.Spiele[memberNum], GetDisplayFields(actTurnier.Spiele[0].GetType()), memberNum + 1, true));
            //        }
            //        break;
            //    case System.Windows.Forms.DialogResult.No:
            //        Random r = new Random();
            //        for (int memberNum = 0; memberNum < actTurnier.Spiele.Count; memberNum++)
            //        {
            //            actTurnier.Spiele[memberNum].PunkteTeilnehmer1 = r.Next(0, 15);
            //            actTurnier.Spiele[memberNum].PunkteTeilnehmer2 = r.Next(0, 15);
            //            spieleTable.Rows.Add(GetDataRow(actTurnier.Spiele[memberNum], GetDisplayFields(actTurnier.Spiele[0].GetType()), memberNum + 1, false));
            //        }
            //        break;
            //}
            //spieleTable.Rows.Add(GetFooterRow());
            //pnl_spiele.Controls.Add(spieleTable);
        }
        private void OnZeigeSpiele_Click(object sender, CommandEventArgs e)
        {

        }

        private void ResetSwitcher()
        {
            MoeglicheTeilnehmer = null;
            Teilnehmer = null;
            MitgliederAnzeige = -1;
            pnl_teilnehmer.Controls.Clear();
        }
        #endregion
    }
}