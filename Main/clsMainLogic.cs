using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupProject.Common;
using groupProject.Items;
using groupProject.Items.Items_Internal_Windows;
using groupProject.Main;
using groupProject.Search;
using System.Windows;
using System.Data;
using System.Reflection;



namespace groupProject.Main
{
    class clsMainLogic
    {

        /// <summary>
        /// List of items, this is all the items in the database
        /// </summary>
        public List<clsItem> itemList = new List<clsItem>();

        /// <summary>
        /// clsMainSQL object
        /// </summary>
        public clsMainSQL mainSQL = new clsMainSQL();

        /// <summary>
        /// clsDataAccess object
        /// </summary>
        public clsDataAccess dataAccess = new clsDataAccess();

        /// <summary>
        /// Constructor for the clsMainLogic class
        /// </summary>
        public clsMainLogic() // Constructor for the clsMainLogic class
        {
            // Any initialization if needed
        }



        /// <summary>
        /// Method that gets the list of items for an invoice by the invoice number
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsItem> getItemsList(int invoiceNum)
        {
            
            try
            {
                // will get the list of items for an invoice from invoice link table
                int iret = 0;

                string sSQL;
                DataSet ds = new DataSet();


                sSQL = mainSQL.getItemsForInvoice(invoiceNum);
                ds = dataAccess.ExecuteSQLStatement(sSQL, ref iret);
                // turn ds to list
                itemList = new List<clsItem>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    clsItem item = new clsItem();

                    item.code = dr[1].ToString();
                    itemList.Add(item);
                }

                // now that each item with its code has been added to itemlist, I need to go through Itemlist and add the cost and description to each item based on the items table
                foreach (clsItem item in itemList)
                {
                    // get the cost and description for each item
                    sSQL = mainSQL.getItemCostAndDescription(item.code);
                    ds = dataAccess.ExecuteSQLStatement(sSQL, ref iret);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        item.cost = Convert.ToDouble(dr[0].ToString());
                        item.description = dr[1].ToString();
                    }
                }
                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Method that gets all items from the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsItem> getAllItems()
        {
            
            try
            {
                // will get the list of all items from the database
                int iret = 0;
                string sSQL = mainSQL.getAllItems();
                DataSet ds = new DataSet();
                ds = dataAccess.ExecuteSQLStatement(sSQL, ref iret);

            // turn ds to list
            itemList = new List<clsItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                clsItem item = new clsItem();
                //item.Code = dr[0].ToString();
                item.code = dr[0].ToString();
                item.cost = Convert.ToDouble(dr[1].ToString());
                item.description = dr[2].ToString();
                itemList.Add(item);
            }

                return itemList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


        /// <summary>
        /// Method that saves the invoice to the database, first clears things, then saves the new things
        /// </summary>
        /// <param name="invoice"></param>
        /// <exception cref="Exception"></exception>
        public void saveInvoice(clsInvoice invoice)
        {
            
            try
            {
                // will save the invoice to the database, if there is already an invoice with the same invoice number, it will delete the old invoice and save the new one, will do the same for the invoice item link
                int iret = 0;
                DataSet ds = new DataSet();
                string sSQL = "";

                // first clear the invoice link from the database
                sSQL = mainSQL.clearInvoiceLink(invoice.InvoiceNum);
                iret = dataAccess.ExecuteNonQuery(sSQL);

                // then clear the invoice from the database
                sSQL = mainSQL.clearInvoice(invoice.InvoiceNum);
                iret = dataAccess.ExecuteNonQuery(sSQL);


                // then save the invoice to the invoice table
                sSQL = mainSQL.saveInvoiceInv(invoice.InvoiceNum.ToString(), invoice.InvoiceDate.ToString(), invoice.TotalCost.ToString());
                iret = dataAccess.ExecuteNonQuery(sSQL);

                // then save the invoice link to the invoice item link table for each item in the invoice, will need to get the nexe LineItemNum for each item for the invoice
                
                foreach (clsItem item in invoice.itemList)
                {
                    int nLineItemNum = 0;
                    string sLineItemNum = "0";
                    // get the next LineItemNum for each item
                    sSQL = mainSQL.getNextLineItemNum(invoice.InvoiceNum.ToString());
                    sLineItemNum = dataAccess.ExecuteScalarSQL(sSQL);
                    if (sLineItemNum == "")
                    {
                        sLineItemNum = "0";
                    }
                    nLineItemNum = Convert.ToInt32(sLineItemNum) + 1;
                    sSQL = mainSQL.saveInvoiceLink(invoice.InvoiceNum.ToString(), item.code.ToString(), nLineItemNum);
                    iret = dataAccess.ExecuteNonQuery(sSQL);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }

        }


        /// <summary>
        /// Method that gets all invoices from the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<clsInvoice> getAllInvoices()
        {
            
            try
            {
                // will get all invoices from the database
                int iret = 0;
                string sSQL = mainSQL.getAllInvoices();
                DataSet ds = new DataSet();
                ds = dataAccess.ExecuteSQLStatement(sSQL, ref iret);

                // turn ds to list
                List<clsInvoice> invoiceList = new List<clsInvoice>();
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    clsInvoice invoice = new clsInvoice();
                    invoice.InvoiceNum = dr[0].ToString();
                    invoice.InvoiceDate = dr[1].ToString();
                    invoice.TotalCost = dr[2].ToString();
                    invoiceList.Add(invoice);
                }
                return invoiceList;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }


    }
}
