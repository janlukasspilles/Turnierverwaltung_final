using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.CustomControls;
using TVModelLib.Model.TurniereNS;
using TVModelLib.Extensions;
using System.Linq;
using System.Collections.Generic;
using TVModelLib;
using System.Reflection;
using TVModelLib.Database;
using TVModelLib.Model.TeilnehmerNS;
using System.Collections;
// Es gibt eine Tabelle mit allen Mannschaften analog zur Personenansicht
// Es gibt einen neuen Button "Spieler hinzufügen" in der Tabelle für jede Mannschaft
// Wird dieser Button gedrückt öffnen sich zwei Felder
// Feld1 sind die Mitglieder der entsprechenden Mannschaft
// Feld2 zeigt alle Personen, die Mitglieder dieser Mannschaft werden können
// Spieler in den jeweiligen Feldern lassen sich auswählen
// Zwei Buttons über die Spieler hinzugefügt oder entfernt werden können
namespace Turnierverwaltung.View.Pages
{
    public partial class Spieleverwaltung : Page
    {
        #region Attributes
        private Controller _controller;
        private CustomTable<Spiel> _spielTable;
        #endregion
        #region Properties
        public Controller Controller { get => _controller; set => _controller = value; }
        public Turnier AusgewaehltesTurnier
        {
            get
            {
                if (ViewState["AusgewaehltesTurnier"] != null)
                    return (Turnier)ViewState["AusgewaehltesTurnier"];
                return null;
            }
            set
            {
                ViewState["AusgewaehltesTurnier"] = value;
            }
        }
        #endregion
        #region Constructors
        #endregion
        #region Methods
        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            if (!IsPostBack)
            {
                Controller.GetAlleTurniere();
                if (Controller.Turniere != null && Controller.Turniere.Count > 0)
                    AusgewaehltesTurnier = Controller.Turniere[0];

                ddlTurnierauswahl.DataSource = Controller.Turniere;
                ddlTurnierauswahl.DataBind();
            }
            BuildTable();
        }

        private void BuildTable()
        {
            _spielTable = new CustomTable<Spiel>(TypeExtensions.GetAssemType($"TVModelLib.Model.TurniereNS.Spiel"), "SpieleTable");
            _spielTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
            _spielTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
            _spielTable.AddButton_ClickCommand += OnAddButton_Click;
            _spielTable.SubmitButton_ClickCommand += OnSubmitButton_Click;

            if (AusgewaehltesTurnier != null)
            {
                _spielTable.DomainDictionary.Add("TURNIERTEILNEHMER", AusgewaehltesTurnier.TurnierTeilnehmer);
                _spielTable.DataSource = AusgewaehltesTurnier.Spiele;
            }
            else
            {
                _spielTable.DataSource = Controller.Turniere[ddlTurnierauswahl.SelectedIndex].Spiele;
            }            
            pnl_tbl.Controls.Add(_spielTable);
            _spielTable.DataBind();
        }

        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            Type ListDataType = _spielTable.FallbackType ?? _spielTable.ListDataType;
            foreach (int row in _spielTable.RowsInEdit)
            {
                for (int i = 0; i < _spielTable.DisplayFields.Count; i++)
                {
                    DisplayMetaInformation dmi = _spielTable.DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                    if (dmi.Editable)
                    {
                        switch (dmi.ControlType)
                        {
                            case ControlType.ctEditText:
                                if (_spielTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_spielTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_spielTable.DisplayFields[i].Name).SetValue(AusgewaehltesTurnier.Spiele[row], Convert.ChangeType(value, _spielTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDomain:
                                if (_spielTable.Rows[row + 1].Cells[i].Controls[0] is DropDownList)
                                {
                                    int domainId = (_spielTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                    if (ListDataType.GetProperty(_spielTable.DisplayFields[i].Name).IsNumericType())
                                    {
                                        ListDataType.GetProperty(_spielTable.DisplayFields[i].Name).SetValue(AusgewaehltesTurnier.Spiele[row], Convert.ChangeType(domainId, _spielTable.DisplayFields[i].PropertyType));
                                    }
                                    else
                                    {
                                        if (_spielTable.DomainDictionary.TryGetValue(dmi.DomainName, out IList domainList))
                                        {
                                            ListDataType.GetProperty(_spielTable.DisplayFields[i].Name).SetValue(AusgewaehltesTurnier.Spiele[row], domainList[domainId - 1]);
                                        }
                                        else
                                        {
                                            throw new Exception("Keine Domainliste hinterlegt");
                                        }
                                    }
                                }
                                break;
                            case ControlType.ctDate:
                                if (_spielTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_spielTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_spielTable.DisplayFields[i].Name).SetValue(AusgewaehltesTurnier.Spiele[row], Convert.ChangeType(value, _spielTable.DisplayFields[i].PropertyType));
                                }
                                break;
                        }
                    }
                }
                if (AusgewaehltesTurnier.Spiele[row].Id == 0)
                {
                    AusgewaehltesTurnier.Spiele[row].Neuanlage(DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", AusgewaehltesTurnier.Turnierart).ToString());
                }
                else
                {
                    AusgewaehltesTurnier.Spiele[row].Speichern(DatabaseHelper.ReturnSingleValue("Bezeichnung", "turnierart", AusgewaehltesTurnier.Turnierart).ToString());
                }
            }
            _spielTable.DataSource = AusgewaehltesTurnier.Spiele;
            _spielTable.DataBind();
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Spiel s = new Spiel((int)AusgewaehltesTurnier.Id);
            AusgewaehltesTurnier.Spiele.Add(s);

            _spielTable.DataSource = AusgewaehltesTurnier.Spiele;
            _spielTable.DataBind();
        }

        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            Queue<Spiel> deletequeue = new Queue<Spiel>();
            foreach (TableRow tmp in _spielTable.Rows)
            {
                if (!(tmp is TableHeaderRow) && !(tmp is TableFooterRow))
                {
                    if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{_spielTable.Rows.GetRowIndex(tmp)}") is CheckBox cb)
                    {
                        if (cb.Checked)
                        {
                            Spiel s = AusgewaehltesTurnier.Spiele[_spielTable.Rows.GetRowIndex(tmp) - 1];
                            s.Loeschen();
                            deletequeue.Enqueue(s);
                        }
                    }
                }
            }
            foreach (Spiel s in deletequeue)
            {
                AusgewaehltesTurnier.Spiele.Remove(s);
            }
            _spielTable.DataSource = AusgewaehltesTurnier.Spiele;
            _spielTable.DataBind();
        }

        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            var tmp = AusgewaehltesTurnier.Spiele.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (AusgewaehltesTurnier.Spiele.SequenceEqual(tmp))
                AusgewaehltesTurnier.Spiele = AusgewaehltesTurnier.Spiele.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            else
                AusgewaehltesTurnier.Spiele = tmp;

            _spielTable.DataSource = AusgewaehltesTurnier.Spiele;
            _spielTable.DataBind();
        }
        #endregion

        protected void ddlTurnierauswahl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controller.GetAlleTurniere();
            AusgewaehltesTurnier = Controller.Turniere[ddlTurnierauswahl.SelectedIndex];
            pnl_tbl.Controls.Remove(pnl_tbl.FindControl("SpieleTable"));
            BuildTable();
        }
    }
}