using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using groupProject.Common;

namespace groupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        /// <summary>
        /// Instantiation of our clsInvoices
        /// </summary>
        clsInvoice clsInvoice = new clsInvoice();
        /// <summary>
        /// Instantiation of our searchlogic class
        /// </summary>
        clsSearchLogic clsSearchLogic = new clsSearchLogic();
        /// <summary>
        /// Bool to keep track if the application is updating information
        /// </summary>
        private bool isUpdating = false;
        /// <summary>
        /// Create an invoice list to use within the whole window
        /// </summary>
        private List<clsInvoice> invoiceList;
        /// <summary>
        /// Holds the selected Invoice object so that it can be accessed from other windows
        /// </summary>
        ///
        public clsInvoice selectedInvoice { get; private set; }

        public wndSearch()
        {
            InitializeComponent();

            LoadData();

            populateFilters();
        }

        /// <summary>
        /// Method that loads the data in our data grid and combo box properly
        /// </summary>
        private void LoadData()
        {
            invoiceList = clsSearchLogic.invoiceList();

            dgInvoiceList.ItemsSource = invoiceList;

            cbInvoiceSelected.ItemsSource = invoiceList;
        }


        /// <summary>
        /// Method that adds our applicable filter options to the combo boxes
        /// </summary>
        private void populateFilters()
        {
            cbDateFilter.Items.Add("");
            foreach (var date in clsSearchLogic.getDates())
            {
                cbDateFilter.Items.Add(date);  // adds the dates from our list to the combo box
            }

            cbCostFilter.Items.Add("");
            foreach (var cost in clsSearchLogic.getCost()) // adds the costs from out list to the combo box
            {
                cbCostFilter.Items.Add(cost);
            }

            cbInvoiceNumFilter.Items.Add("");
            foreach (var num in clsSearchLogic.getNums()) // adds the invoice numbers from our list to the combo box
            {
                cbInvoiceNumFilter.Items.Add(num);
            }
        }

        /// <summary>
        /// Method that applies our filters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            string selectedDate = cbDateFilter.SelectedItem?.ToString(); // takes the selected item if selection isn't null
            string selectedCost = cbCostFilter.SelectedItem?.ToString();
            string selectedInvoiceNum = cbInvoiceNumFilter.SelectedItem?.ToString();

            var filteredList = clsSearchLogic.ApplyFilters(selectedDate, selectedCost, selectedInvoiceNum); // passes in the selected items in the drop down, which will allow us to use LINQ operations

            dgInvoiceList.ItemsSource = filteredList;
            cbInvoiceSelected.ItemsSource = filteredList;

            dgInvoiceList.SelectedItem = null;
            cmdSelectInvoice.IsEnabled = false;
        }

        /// <summary>
        /// Method that resets selected filters and selected invoices back to default
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cbDateFilter.SelectedIndex = 0;
            cbCostFilter.SelectedIndex = 0;
            cbInvoiceNumFilter.SelectedIndex = 0;

            LoadData();

            dgInvoiceList.SelectedItem = null;
            cbInvoiceSelected.SelectedItem = null;
            cmdSelectInvoice.IsEnabled = false;
        }


        /// <summary>
        /// Handles the button click of the Cancel Search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Method that handles selection change in the Invoice combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating) return;

            try
            {
                isUpdating = true;
                if (cbInvoiceSelected.SelectedItem is clsInvoice selectedInvoice)
                {
                    // Find and select the corresponding item in the DataGrid
                    dgInvoiceList.SelectedItem = selectedInvoice;
                    dgInvoiceList.ScrollIntoView(selectedInvoice);
                    cmdSelectInvoice.IsEnabled = true; // Enable the select button
                }
            }
            finally
            {
                isUpdating = false;
            }
        }

        /// <summary>
        /// Method that handles a change selection in the data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgInvoiceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isUpdating) return;

            try
            {
                isUpdating = true;
                if (dgInvoiceList.SelectedItem is clsInvoice selectedInvoice)
                {
                    // Set the ComboBox selected item to match the DataGrid selection
                    cbInvoiceSelected.SelectedItem = selectedInvoice;
                    cmdSelectInvoice.IsEnabled = true; // Enable the select button
                }
                else
                {
                    cbInvoiceSelected.SelectedItem = null;
                    cmdSelectInvoice.IsEnabled = false; // Disable the select button
                }
            }
            finally
            {
                isUpdating = false;
            }
        }


        /// <summary>
        /// Error handling method
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

        /// <summary>
        /// Handles the action of selecting an invoice, stores the selected invoice in selectedInvoice variable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdSelectInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (dgInvoiceList.SelectedItem is clsInvoice selectedInvoice)
            {
                selectedInvoice = selectedInvoice;
                this.DialogResult = true;
                this.Close();
            }
        }
    }
}
