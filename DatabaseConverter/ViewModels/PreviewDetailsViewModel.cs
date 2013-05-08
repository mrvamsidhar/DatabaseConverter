using DatabaseConverter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml;

namespace DatabaseConverter.ViewModels
{
    public sealed class PreviewDetailsViewModel:ViewModelBase
    {
        public ObservableCollection<FeatureListItem> _selectedfeatures;
        public PreviewDetailsViewModel() 
        {
            _selectedfeatures = new ObservableCollection<FeatureListItem>();
        }
        public override void LoadPage() 
        {
            _loadselectedfeatures();
            UpdatePreview();
        }
        public ObservableCollection<FeatureListItem> SelectedFeatures
        {
            get { return _selectedfeatures; }
        }
        private FeatureListItem _selectedFeatureItem;
        public FeatureListItem SelectedFeature
        {
            get { return _selectedFeatureItem; }
            set
            {
                _selectedFeatureItem = value;
                base.NotifyChanged("SelectedFeature");
            }
        }
        #region Methods
        internal override bool IsValid()
        {
            return true;
        }
        public override string DisplayName
        {
            get { return "Preview"; }
        }
        #endregion
        #region PrivateMethods
        private void _loadselectedfeatures()
        {
            _selectedfeatures = new ObservableCollection<FeatureListItem>();
            _selectedfeatures.Add(new FeatureListItem() { IsSelected = true, Name = "All" });
            var selectitems = App.Result.Features.Where(item => item.IsSelected == true).ToList();
            foreach (var item in selectitems)
            {
                _selectedfeatures.Add(item);
            }
            SelectedFeature = _selectedfeatures[0];
        }
        public string GetPreviewinXML(FeatureListItem feature)
        {
            string strXML = "";
            using (SqlConnection con = new SqlConnection())
            {
               // con.ConnectionString = App.Result.ConnectionString;
                con.ConnectionString = App.Result.DatabaseConnectionString();//con.ConnectionString + ";Database=" + App.Result.Database + ";";
                using (SqlCommand com = con.CreateCommand())
                {

                    com.CommandType = CommandType.Text;
                    com.CommandText = feature.Query + " FOR XML AUTO";// "SELECT * from Company FOR XML AUTO";
                    con.Open();
                    using (XmlReader reader = com.ExecuteXmlReader())
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(reader);
                        ds.DataSetName = feature.Name;
                        //ds.WriteXml("D:\\Test1.xml");
                        strXML = ds.GetXml();
                        con.Close();
                    }
                }
            }
            return strXML;
        
        }
        public void UpdatePreview()
        {
            string strPath = System.IO.Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location);
            strPath = strPath + "\\Preview.xml";
            XmlDocument xDoc = new XmlDocument();
            string strXMLData = "";
            strXMLData = "<Preview>";
            if (SelectedFeature.Name == "All")
            {
                foreach (FeatureListItem feature in SelectedFeatures)
                {
                    if(feature.Name != "All")
                        strXMLData = strXMLData + GetPreviewinXML(feature);
                }
            }
            else
            {
                strXMLData = strXMLData + GetPreviewinXML(SelectedFeature);
            }
            strXMLData = strXMLData + "</Preview>";
            xDoc.LoadXml(strXMLData);
            xDoc.Save(strPath);
        }
        #endregion
    }
}
