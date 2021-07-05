using System.Web.UI;

namespace TVModelLib.Extensions
{
    public static class ControlExtensions
    {
        public static Control FindControlRecursive(this Control value, string name)
        {
            if (value != null && value.ID == name)
                return value;
            foreach (Control c in value.Controls)
            {
                Control tmp = FindControlRecursive(c, name);
                if (tmp != null)
                    return tmp;
            }
            return null;
        }
    }
}
