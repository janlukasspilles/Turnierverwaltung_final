using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Turnierverwaltung_final.Model
{
    public class SportartDetailTabelle
    {
        #region Attributes
        private List<Tuple<string, string, int>> _detailColumns;
        private string _tablename;
        #endregion
        #region Properties
        public List<Tuple<string, string, int>> DetailColumns { get => _detailColumns; set => _detailColumns = value; }
        public string Tablename { get => _tablename; set => _tablename = value; }
        #endregion
        #region Constructors
        public SportartDetailTabelle(string bezeichnungSportart)
        {
            Tablename = bezeichnungSportart + "_details";
        }
        #endregion
        #region Methods
        public void ErzeugeNeueTabelle()
        {
            string createTable = $"CREATE TABLE {Tablename} (\r\n{GetColumnsPrepared()});";
        }

        private string GetColumnsPrepared()
        {
            string res = "";
            foreach (var detailColumn in DetailColumns)
            {
                res += $"{detailColumn.Item1} {detailColumn.Item2}({detailColumn.Item3}) NOT NULL,\r\n";
            }
            return res;
        }
        #endregion
    }
}