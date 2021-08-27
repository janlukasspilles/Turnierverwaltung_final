using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turnierverwaltung.ControllerNS;
using Turnierverwaltung.CustomControls;
using TVModelLib;
using TVModelLib.Model.Benutzeranmeldung;

namespace Turnierverwaltung.View.Pages
{
    public partial class Benutzerverwaltung : Page
    {
        private Controller _controller;
        private CustomTable<Benutzer> _curTable;

        public Controller Controller { get => _controller; set => _controller = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Controller = Global.Controller;
            _curTable = new CustomTable<Benutzer>(Type.GetType($"TVModelLib.Model.Benutzeranmeldung.Benutzer"), "Benutzertabelle");
            _curTable.OnHeaderButton_ClickCommand += OnHeaderButton_Click;
            _curTable.DeleteButton_ClickCommand += OnDeleteButton_Click;
            _curTable.AddButton_ClickCommand += OnAddButton_Click;
            _curTable.SubmitButton_ClickCommand += OnSubmitButton_Click;
            _curTable.CancelButton_ClickCommand += OnCancelButton_Click;
            if (!IsPostBack)
            {
                Controller.GetAlleBenutzer();
            }
            _curTable.DataSource = Controller.Benutzer;
            _curTable.DataBind();

            pnl_tbl.Controls.Add(_curTable);
        }
        
        
        private void OnCancelButton_Click(object sender, EventArgs e)
        {
            Controller.Benutzer.RemoveAll(x => x.Id == 0);
            _curTable.DataSource = Controller.Benutzer;
        }

        private void OnAddButton_Click(object sender, EventArgs e)
        {
            Benutzer m = new Benutzer();
            Controller.Benutzer.Add(m);

            _curTable.DataSource = Controller.Benutzer;
            _curTable.DataBind();
        }

        private void OnHeaderButton_Click(object sender, CommandEventArgs e)
        {
            var tmp = Controller.Benutzer.OrderBy(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            if (Controller.Benutzer.SequenceEqual(tmp))
                Controller.Benutzer = Controller.Benutzer.OrderByDescending(o => o.GetType().GetProperty(e.CommandArgument.ToString()).GetValue(o)).ToList();
            else
                Controller.Benutzer = tmp;

            _curTable.DataSource = Controller.Benutzer;
            _curTable.DataBind();
        }

        private void OnSubmitButton_Click(object sender, EventArgs e)
        {
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
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Benutzer[row], Convert.ChangeType(value, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDomain:
                                if (_curTable.Rows[row + 1].Cells[i].Controls[0] is DropDownList)
                                {
                                    int domainId = (_curTable.Rows[row + 1].Cells[i].Controls[0] as DropDownList).SelectedIndex + 1;
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Benutzer[row], Convert.ChangeType(domainId, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                            case ControlType.ctDate:
                                if (_curTable.Rows[row + 1].Cells[i].Controls[0] is TextBox)
                                {
                                    string value = (_curTable.Rows[row + 1].Cells[i].Controls[0] as TextBox).Text;
                                    ListDataType.GetProperty(_curTable.DisplayFields[i].Name).SetValue(Controller.Benutzer[row], Convert.ChangeType(value, _curTable.DisplayFields[i].PropertyType));
                                }
                                break;
                        }
                    }
                }
                if (Controller.Benutzer[row].Id == 0)
                {
                    Controller.Benutzer[row].Neuanlage();
                }
                else
                {
                    Controller.Benutzer[row].Speichern();
                }
            }
            _curTable.DataSource = Controller.Benutzer;
            _curTable.DataBind();
        }

        private void OnDeleteButton_Click(object sender, EventArgs e)
        {
            Queue<Benutzer> deletequeue = new Queue<Benutzer>();
            foreach (TableRow tmp in _curTable.Rows)
            {
                if (!(tmp is TableHeaderRow) && !(tmp is TableFooterRow))
                {
                    if (tmp.Cells[tmp.Cells.Count - 1].FindControl($"cbSelected{_curTable.Rows.GetRowIndex(tmp)}") is CheckBox cb)
                    {
                        if (cb.Checked)
                        {
                            Benutzer t = Controller.Benutzer[_curTable.Rows.GetRowIndex(tmp) - 1];
                            t.Loeschen();
                            deletequeue.Enqueue(t);
                        }
                    }
                }
            }
            foreach (Benutzer t in deletequeue)
            {
                Controller.Benutzer.Remove(t);
            }
            _curTable.DataSource = Controller.Benutzer;
            _curTable.DataBind();
        }
    }
}