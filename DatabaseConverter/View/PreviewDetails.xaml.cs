using DatabaseConverter.Common;
using DatabaseConverter.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseConverter.View
{
    /// <summary>
    /// Interaction logic for PreviewDetails.xaml
    /// </summary>
    public partial class PreviewDetails : UserControl
    {
        public PreviewDetails()
        {
            InitializeComponent();
           
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (((ComboBox)sender).Items.Count > 0)
                ((ComboBox)sender).SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataContext = this.DataContext as PreviewDetailsViewModel;
            if (e.AddedItems.Count <= 0)
                return;
            dataContext.SelectedFeature = e.AddedItems[0] as FeatureListItem;
            dataContext.UpdatePreview();
            Updatebrowsercontrol();
        }
        private void Updatebrowsercontrol()
        {
            string strPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            strPath = strPath + "\\Preview.xml";
          //  byte[] attachment = System.IO.File.ReadAllBytes(strPath);
          //  MemoryStream mStream = new MemoryStream(attachment);
          ////  XMLBrowserctrl.NavigateToStream((Stream)mStream);
            XMLBrowserctrl.Navigate(strPath);
        }
    }
}
