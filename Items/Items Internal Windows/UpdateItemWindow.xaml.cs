using System;
using System.Collections.Generic;
using System.Linq;
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

namespace groupProject.Items.Items_Internal_Windows
{
    /// <summary>
    /// Interaction logic for UpdateItemWindow.xaml
    /// </summary>
    public partial class UpdateItemWindow : Window
    {
        private clsItem selectedItem;
        /// <summary>
        ///    Constructor for the UpdateItemWindow class.  This will set the text boxes to the selected item
        /// </summary>
        /// <param name="selectedItem"></param>
        public UpdateItemWindow(clsItem selectedItem)
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Event handler for the window loaded event.  This will set the text boxes to the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // add sql to update the data
            clsItemsSQL sql = new clsItemsSQL();
            string insertItemSQL = sql.UpdateItemData(selectedItem.code, DescriptionTextBox.Text, CostTextBox.Text);

            // Execute the insert item code
            clsDataAccess dataAccess = new clsDataAccess();
            dataAccess.ExecuteNonQuery(insertItemSQL);
            this.Close();
        }

        /// <summary>
        /// Event handler for the Cancel button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
