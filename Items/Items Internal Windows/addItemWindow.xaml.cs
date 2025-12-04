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

namespace groupProject.Items.Items_Internal_Windows
{
    /// <summary>
    /// Interaction logic for addItemWindow.xaml
    /// </summary>
    public partial class addItemWindow : Window
    {
        /// <summary>
        /// Constructor for the addItemWindow class.
        /// </summary>
        public addItemWindow()
        {
            InitializeComponent();
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

        /// <summary>
        /// Event handler for the Submit button click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            // Add SQL to add the data
            clsItemsSQL sql = new clsItemsSQL();
            string insertItemSQL = sql.insertItem(DescriptionTextBox.Text, CostTextBox.Text);

            // Execute the insert item code
            clsDataAccess dataAccess = new clsDataAccess();
            int iRetVal = 0;
            dataAccess.ExecuteNonQuery(insertItemSQL);

            this.Close();
        }
    }
}
