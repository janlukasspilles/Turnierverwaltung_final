using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.CustomControls;
using TVModeLib.Model.TeilnehmerNS;
using TVModelLib;
using TVModelLib.Extensions;

namespace Turnierverwaltung.View.Pages
{
    public partial class Mannschaften : Page
    {
        private Controller _controller;
        private CustomTable<Teilnehmer> _curTable;
        private CustomSwitchPanel _customSwitchPanel;

        public List<Teilnehmer> Mitglieder
        {
            get
            {
                if (ViewState["Mitglieder"] != null)
                    return (List<Teilnehmer>)ViewState["Mitglieder"];
                return null;
            }
            set => ViewState["Mitglieder"] = value;
        }

        public List<Teilnehmer> MoeglicheMitglieder
        {
            get
            {
                if (ViewState["MoeglicheMitglieder"] != null)
                    return (List<Teilnehmer>)ViewState["MoeglicheMitglieder"];
                return null;
            }
            set => ViewState["MoeglicheMitglieder"] = value;
        }

        public Controller Controller { get => _controller; set => _controller = value; }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            _curTable = new CustomTable<Teilnehmer>(Type.GetType($"Turnierverwaltung.Model.TeilnehmerNS.Mannschaft"));
            _curTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
            _curTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
            _curTable.AddButton_ClickCommand += OnAddButton_Click;
            _curTable.SubmitButton_ClickCommand += OnSubmitButton_Click;
            _curTable.CancelButton_ClickCommand += OnCancelButton_Click;
            _curTable.AdditionalRowButtons = GetAdditionalRowButtons();
            if (!IsPostBack)
            {
                Controller.GetAlleMannschaften();
            }
            _curTable.DataSource = Controller.Teilnehmer;
            _curTable.DataBind();

            pnl_tbl.Controls.Add(_curTable);
            if (MitgliederAnzeige != -1)
            {
                _customSwitchPanel = new CustomSwitchPanel
                {
                    HeadlineText = $"Mitgliederliste für {Controller.Teilnehmer[MitgliederAnzeige]}",
                    Ds1 = Mitglieder,
                    Ds2 = MoeglicheMitglieder
                };
                _customSwitchPanel.LeftButton_ClickCommand += OnLeftButton_Click;
                _customSwitchPanel.RightButton_ClickCommand += OnRightButton_Click;
                _customSwitchPanel.CancelButton_ClickCommand += OnCancelMitgliederButton_Click;
                _customSwitchPanel.SubmitButton_ClickCommand += OnSubmitMitgliederButton_Click;
                _customSwitchPanel.DataBind();
                pnl_mitglieder.Controls.Add(_customSwitchPanel);
            }

        }

        private List<Tuple<string, string, string, Action<object, CommandEventArgs>>> GetAdditionalRowButtons()
        {
            return new List<Tuple<string, string, string, Action<object, CommandEventArgs>>>
            {
                Tuple.Create("btnZeigeMitglieder", "Zeige Mitglieder", "btn btn-secondary", new Action<object, CommandEventArgs>((o, e) => OnZeigeMitglieder_Click(o, e)))
            };
        }

        private void OnZeigeMitglieder_Click(object sender, CommandEventArgs e)
        {
            MitgliederAnzeige = Convert.ToInt32(e.CommandArgument) - 1;
            Mitglieder = (Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Mitglieder;
            MoeglicheMitglieder = Controller.GetMoeglicheMitglieder((Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Id);
            _customSwitchPanel = new CustomSwitchPanel()
            {
                HeadlineText = $"Mitgliederliste für {Controller.Teilnehmer[MitgliederAnzeige]}"
            };
            _customSwitchPanel.Ds1 = Mitglieder;
            _customSwitchPanel.Ds2 = MoeglicheMitglieder;
            _customSwitchPanel.LeftButton_ClickCommand += OnLeftButton_Click;
            _customSwitchPanel.RightButton_ClickCommand += OnRightButton_Click;
            _customSwitchPanel.CancelButton_ClickCommand += OnCancelMitgliederButton_Click;
            _customSwitchPanel.SubmitButton_ClickCommand += OnSubmitMitgliederButton_Click;
            _customSwitchPanel.DataBind();
            pnl_mitglieder.Controls.Clear();
            pnl_mitglieder.Controls.Add(_customSwitchPanel);
        }

        private void OnSubmitMitgliederButton_Click(object sender, EventArgs e)
        {
            (Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Mitglieder = Mitglieder;
            (Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Speichern();
        }

        private void OnCancelMitgliederButton_Click(object sender, EventArgs e)
        {
            Mitglieder = (Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Mitglieder;
            MoeglicheMitglieder = Controller.GetMoeglicheMitglieder((Controller.Teilnehmer[MitgliederAnzeige] as Mannschaft).Id);
            ((ListBox)_customSwitchPanel.FindControlRecursive("lbListe1")).DataSource = Mitglieder;
            ((ListBox)_customSwitchPanel.FindControlRecursive("lbListe2")).DataSource = MoeglicheMitglieder;
            ((ListBox)_customSwitchPanel.FindControlRecursive("lbListe2")).DataBind();
            ((ListBox)_customSwitchPanel.FindControlRecursive("lbListe1")).DataBind();
        }

        private void OnRightButton_Click(object sender, EventArgs e)
        {
            //Entfernen
            ListBox lbMoegliche = (ListBox)_customSwitchPanel.FindControlRecursive("lbListe2");
            ListBox lbTatsaechliche = (ListBox)_customSwitchPanel.FindControlRecursive("lbListe1");
            if (lbTatsaechliche.SelectedIndex != -1)
                Mitglieder.MoveTo(Mitglieder[lbTatsaechliche.SelectedIndex], MoeglicheMitglieder);
            lbMoegliche.DataBind();
            lbTatsaechliche.DataBind();
        }

        private void OnLeftButton_Click(object sender, EventArgs e)
        {
            //Hinzufügen
            ListBox lbMoegliche = (ListBox)_customSwitchPanel.FindControlRecursive("lbListe2");
            ListBox lbTatsaechliche = (ListBox)_customSwitchPanel.FindControlRecursive("lbListe1");
            if (lbMoegliche.SelectedIndex != -1)
                MoeglicheMitglieder.MoveTo(MoeglicheMitglieder[lbMoegliche.SelectedIndex], Mitglieder);

            lbMoegliche.DataBind();
            lbTatsaechliche.DataBind();
        }

        private void OnCancelButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Controller.Teilnehmer.RemoveAll(x => x.Id == 0);
            _curTable.DataSource = Controller.Teilnehmer;
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Mannschaft m = new Mannschaft();
            Controller.Teilnehmer.Add(m);

            _curTable.DataSource = Controller.Teilnehmer;
            _curTable.DataBind();
        }

        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            var tmp = Controller.Teilnehmer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (Controller.Teilnehmer.SequenceEqual(tmp))
                Controller.Teilnehmer = Controller.Teilnehmer.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            else
                Controller.Teilnehmer = tmp;

            _curTable.DataSource = Controller.Teilnehmer;
            _curTable.DataBind();
        }

        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Type ListDataType = _curTable.FallbackType ?? _curTable.ListDataType;
            foreach (int row in _curTable.RowsInEdit)
            {
                for (int i = 0; i < _curTable.DisplayFields.Count; i++)
                {
                    DisplayMetaInformation dmi = _curTable.DisplayFields[i].GetCustomAttribute(typeof(DisplayMetaInformation), true) as DisplayMetaInformation;
                    if (dmi.Editable)
                    {
                        switch (dmi.ControlType)
                        {
                            case ControlType.ctEditText:
                                if (_curTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_curTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(value, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDomain:
                                if (_curTable.Rows[row + 1].Cells[i].Controls[0] is DropDownList)
                                {
                                    int domainId = (_curTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(domainId, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDate:
                                if (_curTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_curTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Teilnehmer[row], Convert.ChangeType(value, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                        }
                    }
                }
                if (Controller.Teilnehmer[row].Id == 0)
                {
                    Controller.Teilnehmer[row].Neuanlage();
                }
                else
                {
                    Controller.Teilnehmer[row].Speichern();
                }
            }
            _curTable.DataSource = Controller.Teilnehmer;
            _curTable.DataBind();
        }
        private void ResetSwitcher()
        {
            MoeglicheMitglieder = null;
            Mitglieder = null;
            MitgliederAnzeige = -1;
            pnl_mitglieder.Controls.Clear();
        }

        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            ResetSwitcher();
            Queue<Teilnehmer> deletequeue = new Queue<Teilnehmer>();
            foreach (TableRow tmp in _curTable.Rows)
            {
                if (!(tmp is TableHeaderRow) && !(tmp is TableFooterRow))
                {
                    if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{_curTable.Rows.GetRowIndex(tmp)}") is CheckBox cb)
                    {
                        if (cb.Checked)
                        {
                            Teilnehmer t = Controller.Teilnehmer[_curTable.Rows.GetRowIndex(tmp) - 1];
                            t.Loeschen();
                            deletequeue.Enqueue(t);
                        }
                    }
                }
            }
            foreach (Teilnehmer t in deletequeue)
            {
                Controller.Teilnehmer.Remove(t);
            }
            _curTable.DataSource = Controller.Teilnehmer;
            _curTable.DataBind();
        }
    }
}