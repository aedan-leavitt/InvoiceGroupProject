using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.OleDb;
using System.Reflection;

namespace groupProject.Main
{
    class clsMainSQL
    {
        


        public clsMainSQL() 
        {
            
        }





        /// <summary>
        /// SQL that saves the invoice to the database
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="invoiceDate"></param>
        /// <param name="totalCost"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string saveInvoiceInv(string invoiceNum, string invoiceDate, string totalCost)
        {
            try
            {
                string sSQL = "INSERT INTO Invoices (InvoiceNum, InvoiceDate, TotalCost) VALUES (" + invoiceNum + ", #" + invoiceDate + "#, " + totalCost + ")";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL that saves the invoice item link to the database
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string saveInvoiceLink(string invoiceNum, string Code, int LineItemNum)
        {
            try
            {
                string sSQL = "INSERT INTO LineItems (InvoiceNum, ItemCode, LineItemNum) VALUES (" + invoiceNum + ", '" + Code + "', " + LineItemNum + ")";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL that clears the invoice item link from the database
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string clearInvoiceLink(string invoiceNum)
        {
            try
            {
                string sSQL = "DELETE * FROM LineItems WHERE InvoiceNum = " + invoiceNum;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL that clears the invoice from the database
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string clearInvoice(string invoiceNum)
        {
            try
            {
                string sSQL = "DELETE * FROM Invoices WHERE InvoiceNum = " + invoiceNum;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL that gets all items from the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public String getAllItems()
        {
            try
            {
                string sSQL = "SELECT ItemCode, Cost, ItemDesc FROM ItemDesc";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }



        /// <summary>
        /// SQL that gets all items for a specific invoice
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string getItemsForInvoice(int invoiceNum)
        {
            try
            {
                string sSQL = "SELECT InvoiceNum, ItemCode FROM LineItems WHERE InvoiceNum = " + invoiceNum;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        // get the cost and description for an item
        public string getItemCostAndDescription(string itemCode)
        {
            try
            {
                string sSQL = "SELECT Cost, ItemDesc FROM ItemDesc WHERE ItemCode = '" + itemCode + "'";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
            
        }

        /// <summary>
        /// SQL that gets all invoices from the database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string getAllInvoices()
        {
            try
            {
                string sSQL = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// SQL that gets the invoice number from the ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        //public string getInvoiceNumFromID(int id)
        //{
        //    try
        //    {
        //        string sSQL = "SELECT InvoiceNum FROM Invoices WHERE ID = " + id;
        //        return sSQL;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
        //    }
        //}


        // get next LineItemNum for an invoice
        
        public string getNextLineItemNum(string invoiceNum)
        {
            try
            {
                string sSQL = "SELECT MAX(LineItemNum) FROM LineItems WHERE InvoiceNum = " + invoiceNum;
                return sSQL;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
