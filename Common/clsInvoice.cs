using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace groupProject.Common
{
    public class clsInvoice
    {
        /// <summary>
        /// Invoice number
        /// </summary>
        public string InvoiceNum { get; set; }
        /// <summary>
        /// Invoice date
        /// </summary>
        public string InvoiceDate { get; set; }
        /// <summary>
        /// Invoice total cost
        /// </summary>
        public string TotalCost { get; set; }
        public int id { get; set; }
        /// <summary>
        /// Holds the list of our invoices
        /// </summary>
        public List<clsItem> itemList { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemList"></param>
        public clsInvoice(int id, List<clsItem> itemList)
        {
            this.id = id;
            this.itemList = itemList;
        }

        public clsInvoice()
        {
        }

        /// <summary>
        /// Overrides tostring method
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override string ToString()
        {
            try
            {
                return "Invoice #" + InvoiceNum + " " + InvoiceDate;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }
    }
}
