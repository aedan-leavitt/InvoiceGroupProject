using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using groupProject.Common;


namespace groupProject.Items
{
    class clsItemsSQL
    {
        // <summary>

        /// This SQL gets all data on an invoice for a given InvoiceID.

        /// </summary>

        /// <param name="sInvoiceID">The InvoiceID for the invoice to retrieve all data.</param>

        /// <returns>All data for the given invoice.</returns>

        public string SelectInvoiceData(string sInvoiceID)

        {

            string sSQL = "SELECT * FROM Invoices WHERE InvoiceNum = " + sInvoiceID;

            return sSQL;

        }

        public string SelectItemData(string sItemID)

        {

            string sSQL = "SELECT * FROM ItemDesc WHERE ItemNum = " + sItemID;

            return sSQL;

        }

        //- Update ItemDesc Set ItemDesc = 'abcdef', Cost = 123 where ItemCode = 'A'
        public string UpdateItemData(string sItemID, string sItemDesc, string sCost)
        {
            string sSQL = "UPDATE ItemDesc SET ItemDesc = '" + sItemDesc + "', Cost = " + sCost + " WHERE ItemNum = " + sItemID;
            return sSQL;
        }

        //- Insert into ItemDesc(ItemCode, ItemDesc, Cost) Values('ABC', 'blah', 321)
        public string insertItem(string sItemDesc, string sCost)
        {
            string sSQL = "INSERT INTO ItemDesc (ItemCode, ItemDesc, Cost) VALUES ('" + sItemDesc + "', '" + sItemDesc + "', " + sCost + ")";
            return sSQL;
        }

        //- Delete from ItemDesc Where ItemCode = 'ABC'
        public string deleteItem(string sItemID)
        {
            string sSQL = $"DELETE FROM ItemDesc WHERE ItemCode = '{sItemID}'";
            return sSQL;
        }

        public string getAllInvoiceItems()
        {
            string sSql = "SELECT * FROM LineItems";
            return sSql;
        }

        public string getAllItems()

        {

            string sSQL = "SELECT * FROM ItemDesc";

            return sSQL;

        }
    }
}
