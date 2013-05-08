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

namespace DatabaseConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly WizardPageViewModel _WizardViewModel;
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            _WizardViewModel = new WizardPageViewModel();
            _WizardViewModel.RequestClose += _WizardViewModel_RequestClose;
            base.DataContext = _WizardViewModel;
        }
        void _WizardViewModel_RequestClose(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
