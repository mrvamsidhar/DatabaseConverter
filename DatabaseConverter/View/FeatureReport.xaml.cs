using DatabaseConverter.Common;
using DatabaseConverter.ViewModels;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Xps.Packaging;
//sing System.Windows.Xps.Packaging;

namespace DatabaseConverter.View
{
    /// <summary>
    /// Interaction logic for FeatureReport.xaml
    /// </summary>
    public partial class FeatureReport : UserControl
    {
        public FeatureReport()
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
             var dataContext = this.DataContext as FeatureReportViewModel;
            if (e.AddedItems.Count <= 0)
                return;
            dataContext.SelectedFeature = e.AddedItems[0] as FeatureListItem;
            Updatebrowsercontrol();
        }

        private void Updatebrowsercontrol()
        {
            var dataContext = this.DataContext as FeatureReportViewModel;
            ObservableCollection<FeatureListItem> selecteditems = null;

            bool bIsAllSelected = false;
            if (dataContext.SelectedFeature.Name == "All")
            {
                bIsAllSelected = true;
                selecteditems = dataContext.SelectedFeatures;
            }
            else
            {
                selecteditems = new ObservableCollection<FeatureListItem>();
                selecteditems.Add(dataContext.SelectedFeature);
            }
            dataContext.ReportConverter.Convert(selecteditems, bIsAllSelected);
            string strFilePath =  System.Reflection.Assembly.GetExecutingAssembly().Location;
            string wordDocument = System.IO.Path.GetDirectoryName(strFilePath);
            wordDocument =    wordDocument +    "\\Sample.doc";
            if (string.IsNullOrEmpty(wordDocument) || !File.Exists(wordDocument))
            {
                MessageBox.Show("The file is invalid. Please select an existing file again.");
            }
            else
            {
                string convertedXpsDoc = string.Concat(System.IO.Path.GetTempPath(), "\\", Guid.NewGuid().ToString(), ".xps");
                XpsDocument xpsDocument = ConvertWordToXps(wordDocument, convertedXpsDoc);
                if (xpsDocument == null)
                {
                    return;
                }

                documentviewWord.Document = xpsDocument.GetFixedDocumentSequence();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as FeatureReportViewModel;
            ObservableCollection<FeatureListItem> selecteditems = null;

            bool bIsAllSelected = false;
            if (dataContext.SelectedFeature.Name == "All")
            {
                bIsAllSelected = true;
                selecteditems = dataContext.SelectedFeatures;
            }
            else
            {
                selecteditems = new ObservableCollection<FeatureListItem>();
                selecteditems.Add(dataContext.SelectedFeature);
            }
            dataContext.ReportConverter.Convert(selecteditems, bIsAllSelected);
             Updatebrowsercontrol();
            ////Message box confirmation to view the created document.
            //if (MessageBox.Show("Do you want to view the MS Word document?", "Document has been created", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            //{
            //    //Launching the MS Word file using the default Application.[MS Word Or Free WordViewer]
            //    System.Diagnostics.Process.Start("Sample.doc");
            //    //Exit
            //    // this.Close();
            //}
        }

        /// <summary>
        ///  Convert the word document to xps document
        /// </summary>
        /// <param name="wordFilename">Word document Path</param>
        /// <param name="xpsFilename">Xps document Path</param>
        /// <returns></returns>
        private XpsDocument ConvertWordToXps(string wordFilename, string xpsFilename)
        {
            // Create a WordApplication and host word document
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                wordApp.Documents.Open(wordFilename);

                // To Invisible the word document
                wordApp.Application.Visible = false;

                // Minimize the opened word document
                wordApp.WindowState = WdWindowState.wdWindowStateMinimize;

                Document doc = wordApp.ActiveDocument;

                doc.SaveAs(xpsFilename, WdSaveFormat.wdFormatXPS);

                XpsDocument xpsDocument = new XpsDocument(xpsFilename, FileAccess.Read);
                return xpsDocument;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurs, The error message is  " + ex.ToString());
                return null;
            }
            finally
            {
                wordApp.Documents.Close();
                ((_Application)wordApp).Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }
    }
}
