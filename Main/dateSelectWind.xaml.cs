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

namespace groupProject.Main
{
    /// <summary>
    /// Interaction logic for dateSelectWind.xaml
    /// </summary>
    public partial class dateSelectWind : Window
    {
        public dateSelectWind()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if the date is selected
            //if (datePicker.SelectedDate == null)
            //{
            //    MessageBox.Show("Please select a date.");
            //    return;
            //}
            this.DialogResult = true;
        }
    }
}
