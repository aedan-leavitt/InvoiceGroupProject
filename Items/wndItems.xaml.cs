using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using groupProject.Common;
using groupProject.Items.Items_Internal_Windows;

namespace groupProject.Items
{
    public partial class wndItems : Window
    {
        public List<clsInvoice> invoices;
        private clsItemsLogic itemsLogic;
        private List<clsItem> items;
        private List<clsItem> invoiceItems; // This will hold the items from the invoices
        private clsItemsSQL itemsSQL; // This will be used to add and update items in the database
        private clsDataAccess dataAccess; // This will be used to access the database


        /// <summary>
        /// Constructor for the wndItems class.
        /// </summary>
        /// <param name="invoices"></param>
        /// <param name="items"></param>
        public wndItems(List<clsInvoice> invoices)
        {
            InitializeComponent();
            this.invoices = invoices; //will be used to find the items in each invoice
            itemsSQL = new clsItemsSQL();
            itemsLogic = new clsItemsLogic();
            dataAccess = new clsDataAccess();

            string allItemsSQL = itemsSQL.getAllItems();
            int iRetVal = 0; // Variable to hold the number of rows returned
            DataSet ds = dataAccess.ExecuteSQLStatement(allItemsSQL, ref iRetVal);
            items = new List<clsItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                items.Add(new clsItem
                {
                    code = row["ItemCode"].ToString(),
                    cost = Convert.ToDouble(row["Cost"].ToString()),
                    description = row["ItemDesc"].ToString()
                });
            }

            
            LoadData();
        }


        /// <summary>
        ///     Loads the data from the database into the DataGrid.
        /// </summary>
        private void LoadData()
        {
            string invoiceItemsSQL = itemsSQL.getAllInvoiceItems();
            int iRetVal = 0; // Variable to hold the number of rows returned
            DataSet ds = dataAccess.ExecuteSQLStatement(invoiceItemsSQL, ref iRetVal);

            // Convert the DataSet to a List<clsItem>
            invoiceItems = new List<clsItem>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                invoiceItems.Add(new clsItem
                {
                    code = row["ItemCode"].ToString(),
                    cost = 10.5,
                    description = "Filler"
                });
            }

            ItemWindowDataGrid.ItemsSource = items; // These will later just use SQL rather than passing lists.
        }


        /// <summary>
        ///     Event handler for the Edit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is clsItem selectedItem)
                {
                    //MessageBox.Show($"Editing item: {selectedItem.code}");//message box is temporary
                    // Open an edit form or allow inline editing
                    UpdateItemWindow UpdateWindow = new UpdateItemWindow(selectedItem);
                    //this.Hide();
                    UpdateWindow.ShowDialog();
                    //this.Show();
                    ItemWindowDataGrid.ItemsSource = null;  // Refresh the DataGrid
                    ItemWindowDataGrid.ItemsSource = items;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        ///    Event handler for the Delete button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is clsItem selectedItem)
                {
                    if (MessageBox.Show($"Delete {selectedItem.code}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        // Check if the item is in any invoice
                        foreach (clsItem invoiceItem in invoiceItems)
                        {
                            if (invoiceItem.code == selectedItem.code)
                            {
                                MessageBox.Show("Cannot delete item, it is in an invoice.");
                                return;
                            }
                        }
                        //MessageBox.Show($"Deleting {selectedItem.code}");

                        // Remove the item from the list
                        items.Remove(selectedItem);

                        // Generate the SQL command
                        string deleteItemSQL = itemsSQL.deleteItem(selectedItem.code.ToString());

                        //MessageBox.Show(deleteItemSQL);
                        // Execute the SQL command

                        dataAccess.ExecuteNonQuery(deleteItemSQL);

                        // Refresh the DataGrid
                        ItemWindowDataGrid.ItemsSource = null;
                        ItemWindowDataGrid.ItemsSource = items;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }

        /// <summary>
        ///   Event handler for the Return button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close the current window and return to the previous window
        }

        /// <summary>
        ///   Event handler for the Add button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //add window
                addItemWindow addWindow = new addItemWindow();
                //this.Hide();
                addWindow.ShowDialog();
                //this.Show();
                ItemWindowDataGrid.ItemsSource = null;  // Refresh the DataGrid
                ItemWindowDataGrid.ItemsSource = items;
                //below was the testing without sql
                /*items.Add(new clsItem() // Add a new item to the list
                {
                    code = items.Count + 1, // Simple way to generate a unique code
                    cost = 0, // Default cost
                    description = "New Item" // Default description
                });*/
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + "->" + ex.Message);
            }
            
        }
    }
}
