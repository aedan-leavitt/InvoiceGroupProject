using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using groupProject.Common;

namespace groupProject.Search
{
    class clsSearchLogic
    {
        /// <summary>
        /// Holds our item list
        /// </summary>
        List<clsInvoice> itemList;
        /// <summary>
        /// Holds a copy of our item list that we can manipulate as we apply filters
        /// </summary>
        List<clsInvoice> fullItemList;

        /// <summary>
        /// Method that generates the list of our invoices
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> invoiceList()
        {
            try { 
                clsDataAccess db = new clsDataAccess();
                DataSet ds = new DataSet();
                int iRet = 0;
                itemList = new List<clsInvoice>();

                ds = db.ExecuteSQLStatement(Search.clsSearchSQL.getAllInvoices(), ref iRet);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    clsInvoice clsInvoice = new clsInvoice();
                    clsInvoice.InvoiceNum = dr[0].ToString();
                    clsInvoice.InvoiceDate = dr[1].ToString();
                    clsInvoice.TotalCost = dr[2].ToString();
                    itemList.Add(clsInvoice);
                }

                fullItemList = itemList;

                return itemList;

        }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method that takes distinct dates to use for our combo box
        /// </summary>
        /// <returns></returns>
        public List<string> getDates()
        {
            if (fullItemList == null) invoiceList();
            return fullItemList.Select(i => i.InvoiceDate).Distinct().OrderBy(d => d).ToList(); // takes distinct dates from our list of invoices and returns them so we can add them to our list in the corresponding combo box
        }

        /// <summary>
        /// Method that takes distinct costs to use for our combo box
        /// </summary>
        /// <returns></returns>
        public List<string> getCost()
        {
            if (fullItemList == null) invoiceList();
            return fullItemList.Select(i => i.TotalCost).Distinct().OrderBy(c => c).ToList();
        }

        /// <summary>
        /// Method that takes distinct numbers to use for our combo box
        /// </summary>
        /// <returns></returns>
        public List<string> getNums()
        {
            if (fullItemList == null) invoiceList();
            return fullItemList.Select(i => i.InvoiceNum).Distinct().OrderBy(n => n).ToList();
        }

        /// <summary>
        /// Method that takes in the selected filter values and filters our copied fullItemList accordingly using LINQ operations
        /// </summary>
        /// <param name="selectedDate"></param>
        /// <param name="selectedCost"></param>
        /// <param name="selectedInvoiceNum"></param>
        /// <returns></returns>
        public List<clsInvoice> ApplyFilters(string selectedDate, string selectedCost, string selectedInvoiceNum)
        {
            // I did a lot of research and found information online to understand these LINQ operations, so I will leave detailed comments explaining each step in this filtering process to show my understanding of how this works
           
            if (fullItemList == null) invoiceList(); // Check to make sure our item list actually has data, otherwise populate it here

            var filtered = fullItemList.AsEnumerable();  // Temporary filter that will hold our item list as an iEnumerable type, which allows us to step through each element one at a time

            if (!string.IsNullOrEmpty(selectedDate)) // checks to make sure the user selection isn't the empty string or null
                filtered = filtered.Where(i => i.InvoiceDate == selectedDate); // this line only keeps the invoices in our list that meet the criteria using the LINQ operation, so if the invoice date of this invoice matches the one the user chose, it will keep it in the filtered list. otherwise it will be removed

            if (!string.IsNullOrEmpty(selectedCost))
                filtered = filtered.Where(i => i.TotalCost == selectedCost); // cumulative filtering based on the cost

            if (!string.IsNullOrEmpty(selectedInvoiceNum))
                filtered = filtered.Where(i => i.InvoiceNum == selectedInvoiceNum); // cumulative filtering based on the invoice number

            return filtered.ToList(); // returns our narrowed down iEnumerable list back to us in a list form.
        }

    }
}
