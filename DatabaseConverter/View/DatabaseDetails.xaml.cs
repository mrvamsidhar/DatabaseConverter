using DatabaseConverter.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseConverter.View
{
    /// <summary>
    /// Interaction logic for DatabaseDetails.xaml
    /// </summary>
    public partial class DatabaseDetails : UserControl
    {
        public DatabaseDetails()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            var databasedetailsviewmodel = this.DataContext as DatabaseDetailsViewModel;
            if (databasedetailsviewmodel != null)
                databasedetailsviewmodel.IsWindowsAuthentication = false;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            var databasedetailsviewmodel = this.DataContext as DatabaseDetailsViewModel;
            if (databasedetailsviewmodel != null)
                databasedetailsviewmodel.IsWindowsAuthentication = true;
        }
    }
}
