using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using groupProject.Common;
using groupProject.Items;
using groupProject.Items.Items_Internal_Windows;
using groupProject.Main;
using groupProject.Search;
using static System.Formats.Asn1.AsnWriter;

namespace groupProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// the list of all invoices- I dont think I actually use this
        /// </summary>
        List<clsInvoice> invoices = new List<clsInvoice>();

        /// <summary>
        /// the list of all items this is used to populate the combo box
        /// </summary>
        List<clsItem> items = new List<clsItem>();

        /// <summary>
        /// Instantiation of our clsMainLogic class
        /// </summary>
        clsMainLogic mainLogic = new clsMainLogic();

        /// <summary>
        /// Instantiation of our clsMainSQL class
        /// </summary>
        clsMainSQL mainSQL = new clsMainSQL();

        /// <summary>
        /// Instantiation of our clsDataAccess class
        /// </summary>
        clsDataAccess dataAccess = new clsDataAccess();

        /// <summary>
        /// Instantiation of our clsInvoice class that will be used to hold the current invoice
        /// </summary>
        public clsInvoice currentInvoice = new clsInvoice();

        /// <summary>
        /// Instantiation of our clsItem class that will be used to hold the current item inside the combo box
        /// </summary>
        public clsItem currentItem = new clsItem();

        /// <summary>
        /// Constructor for the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //temporary intialized data for testing
            
            try
            {
                //initializeData();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }






        //    newInvoice();
        //}

        public void initializeData()//this function is temproarily giving some data for testing purposes.
        {//later to be updated using the sql commands
            invoices = new List<clsInvoice>();
            items = new List<clsItem>();

            items.Add(new clsItem()
            {
                code = "1",
                cost = 2,
                description = "first item"
            });
            items.Add(new clsItem()
            {
                code = "2",
                cost = 1,
                description = "first item"
            });
            items.Add(new clsItem()
            {
                code = "3",
                cost = 5,
                description = "third item"
            });

            invoices.Add(new clsInvoice()
            {
                itemList = new List<clsItem>()
            });
            invoices[0].itemList.Add(items[0]);
            invoices[0].itemList.Add(items[1]);

        }

        private void itemBtn_Click(object sender, RoutedEventArgs e)
        {
            try { 
            wndItems ItemsWindow = new wndItems(invoices);
            this.Hide();
            ItemsWindow.ShowDialog();
            this.Show();

                // get new list of all items from the database
                items = mainLogic.getAllItems();
                // re link the itembox to items
                itemBox.ItemsSource = null;
                itemBox.ItemsSource = items;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the search button click event. This will open the search window and refresh the items combo box and Invoice while also getting the new invoice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                wndSearch SearchWindow = new wndSearch();
                this.Hide();
                SearchWindow.ShowDialog();
                this.Show();
                // get the selected invoice from the search window and set it to the current invoice
                currentInvoice = SearchWindow.selectedInvoice; // currently the search window does not actually change the selectedInvoice ever
                currentInvoice = SearchWindow.dgInvoiceList.SelectedItem as clsInvoice; // this is for while the search window is not implemented correctly

                // make the selected item from combo box no longer be selected
                itemBox.SelectedItem = null;

                // get the items from the database
                items = mainLogic.getAllItems();

                newInvoice();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the add item button click event. This will add the selected item from the combo box to the invoice and refresh the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                // only works if there is an item from the combo box selected
                if (itemBox.SelectedItem != null)
                {
                    // get the selected item from the combo box
                    clsItem selectedItem = itemBox.SelectedItem as clsItem;

                    //currentInvoice = currentInvoice;

                    // add the selected item to the invoice
                    currentInvoice.itemList.Add(selectedItem);
                    // refresh the data grid
                    itemDataGrid.ItemsSource = null;
                    itemDataGrid.ItemsSource = currentInvoice.itemList;
                }
                // refresh the invoice info
                refreshInvoiceInfo();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the edit button click event. This will enable the buttons and refresh the items combo box and invoice data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                itemBox.IsEnabled = true;
                addItemBtn.IsEnabled = true;
                removeItemBtn.IsEnabled = true;
                editBtn.IsEnabled = false;

                // get new list of all items from the database
                items = mainLogic.getAllItems();
                // re link the itembox to items
                itemBox.ItemsSource = null;
                itemBox.ItemsSource = items;

                // if the selected item is null then create a new invoice
                if (currentInvoice == null || currentInvoice.itemList == null)
                {
                    currentInvoice = new clsInvoice();
                    currentInvoice.itemList = new List<clsItem>();
                }

                // refresh the UI for invoice information- date cost and invoice number
                refreshInvoiceInfo();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Method that will create a new invoice and refresh the data grid and combo box if there is no invoice selected, otherwise just refreshes things
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void newInvoice()
        {
            
            try
            {
                // re disables buttons
                itemBox.IsEnabled = false;
                addItemBtn.IsEnabled = false;
                removeItemBtn.IsEnabled = false;
                editBtn.IsEnabled = true;

                //checks if there was a selected invoice from the search window
                if (currentInvoice == null)
                {
                    currentInvoice = new clsInvoice();
                    currentInvoice.itemList = new List<clsItem>();
                }

                // re link the invoice to the datagrid
                itemDataGrid.ItemsSource = null;
                itemDataGrid.ItemsSource = currentInvoice.itemList;

                // while the itemslist is not implemented correctly instead use the database to get itemslist for an invoice
                int invoiceNum = Convert.ToInt32(currentInvoice.InvoiceNum);
                itemDataGrid.ItemsSource = mainLogic.getItemsList(invoiceNum);
                currentInvoice.itemList = mainLogic.getItemsList(invoiceNum);


                // now the combo box needs to be populated with the items from the database
                itemBox.ItemsSource = null;
                itemBox.ItemsSource = mainLogic.getAllItems();

                refreshInvoiceInfo();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the remove item button click event. This will remove the selected item from the invoice and refresh the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeItemBtn_Click(object sender, RoutedEventArgs e)
        {
            

            
            try
            {
                // Only works if there is an item selected from the data grid
                if (itemBox.SelectedItem != null)
                {
                    clsItem selectedItem = itemBox.SelectedItem as clsItem;

                    // Find the matching item by a unique identifier (e.g., code)
                    clsItem itemToRemove = currentInvoice.itemList.FirstOrDefault(item => item.code == selectedItem.code);

                    if (itemToRemove != null)
                    {
                        currentInvoice.itemList.Remove(itemToRemove);

                        // Refresh the data grid
                        itemDataGrid.ItemsSource = null;
                        itemDataGrid.ItemsSource = currentInvoice.itemList;
                    }
                }
                refreshInvoiceInfo();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }


        /// <summary>
        /// Method that will refresh the invoice information displayed in the UI
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void refreshInvoiceInfo()
        {
            
            try
            {
                // make sure to have the total cost be correct
                currentInvoice.TotalCost = "0";
                foreach (clsItem item in currentInvoice.itemList)
                {
                    currentInvoice.TotalCost = (Convert.ToDouble(currentInvoice.TotalCost) + Convert.ToDouble(item.cost)).ToString();
                }

                // Refresh the invoice information displayed in the UI
                // make sure to have date first
                if (currentInvoice.InvoiceDate == null)
                {
                    currentInvoice.InvoiceDate = DateTime.Now.ToString();
                }
                disDate.Content = currentInvoice.InvoiceDate.ToString();

                // if there is no invoice num get a new one that is the highest invoice number + 1
                // get invoices from the database
                invoices = mainLogic.getAllInvoices();
                if (currentInvoice.InvoiceNum == null || currentInvoice.InvoiceNum == "0")
                {
                    int maxInvoiceNum = 0;
                    foreach (clsInvoice invoice in invoices)
                    {
                        int invoiceNum = Convert.ToInt32(invoice.InvoiceNum);
                        if (invoiceNum > maxInvoiceNum)
                        {
                            maxInvoiceNum = invoiceNum;
                        }
                    }
                    currentInvoice.InvoiceNum = (maxInvoiceNum + 1).ToString();
                }

                disNum.Content = currentInvoice.InvoiceNum.ToString();




                discost.Content = "$" + currentInvoice.TotalCost.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." + MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the save button click event. This will save the current invoice to the database and refresh the data grid, will get default values if something is null, will also prompt the user for a date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                // save the current invoice, will remove the invoice from the database and then save it again, will remove the invoice item links then readd the invoice item links

                // get the user to input a date with the dateSelectWindow
                dateSelectWind dateSelectWindow = new dateSelectWind();
                this.Hide();
                dateSelectWindow.ShowDialog();
                this.Show();
                // get the date from the dateSelectWindow if there is a selected date
                string date = dateSelectWindow.datePicker.Text;
                if (date == null || date == "")
                {
                    // if its null then either use the date from invoice or the current date
                    if (currentInvoice.InvoiceDate != null)
                    {
                        date = currentInvoice.InvoiceDate;
                    }
                    else
                    {
                        date = DateTime.Now.ToString();
                    }
                }


                // set the date to the current invoice
                currentInvoice.InvoiceDate = date;

                // make sure each part of the invoice is not null
                if (currentInvoice.InvoiceNum == null || currentInvoice.InvoiceNum == "0")
                {
                    // get automatic new invoice number
                    int maxInvoiceNum = 0;
                    foreach (clsInvoice invoice in invoices)
                    {
                        int invoiceNum = Convert.ToInt32(invoice.InvoiceNum);
                        if (invoiceNum > maxInvoiceNum)
                        {
                            maxInvoiceNum = invoiceNum;
                        }
                    }
                }
                if (currentInvoice.InvoiceDate == null)
                {
                    currentInvoice.InvoiceDate = DateTime.Now.ToString();
                }
                if (currentInvoice.TotalCost == null)
                {
                    currentInvoice.TotalCost = "0";
                }
                if (currentInvoice.itemList == null)
                {
                    currentInvoice.itemList = new List<clsItem>();
                }
                // save the invoice to the database
                mainLogic.saveInvoice(currentInvoice);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for the new button click event. This will create a new invoice and refresh the data grid and combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                // create a new invoice
                currentInvoice = new clsInvoice();
                currentInvoice.itemList = new List<clsItem>();

                refreshInvoiceInfo();
                // re disable buttons
                itemBox.IsEnabled = false;
                addItemBtn.IsEnabled = false;
                removeItemBtn.IsEnabled = false;
                editBtn.IsEnabled = true;

                // re link the invoice to the datagrid
                itemDataGrid.ItemsSource = null;
                itemDataGrid.ItemsSource = currentInvoice.itemList;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// Exception handler that shows the error
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}