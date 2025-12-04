using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;

namespace groupProject.Common
{
    public class clsItem
    {
        public string code { get; set; }
        public double cost { get; set; }
        //public string itemName { get; set; }
        public string description { get; set; }

        public clsItem(string code, double cost, string description)
        {
            this.code = code;
            this.cost = cost;
            this.description = description;
        }

        public clsItem()
        {
            // Default constructor
        }

        // return code, then description, then cost
        public override string ToString()
        {
            return "Item #" + code + " " + description + " $" + cost;

        }
    }
}
