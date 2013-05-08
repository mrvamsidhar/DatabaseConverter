using DatabaseConverter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.ViewModels
{
    public sealed class FeaturesDetailsViewModel:ViewModelBase
    {
       
        public FeaturesDetailsViewModel() 
        {
           // Features = new ObservableCollection<FeatureListItem>();
            _loadFeatures();
        }
        public override void LoadPage() { }
        #region Methods

        internal override bool IsValid()
        {
            return true;
        }
        #region DisplayName

        public override string DisplayName
        {
            get { return "Select Features"; }
        }
        public ObservableCollection<FeatureListItem> Features 
        {
            get { return App.Result.Features; }
            set { App.Result.Features = value; } 
        }
        private void _loadFeatures()
        {
            Features.Clear();
           // _featureslist.Clear();
            foreach (string strKey in ConfigurationManager.AppSettings.AllKeys)
            {
                string strVal = ConfigurationManager.AppSettings[strKey];
                FeatureListItem item = new FeatureListItem() { Name = strKey, Query = strVal };
                Features.Add(item);
                item.PropertyChanged += item_PropertyChanged;
             }
        }

        void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           
        }
        #endregion // DisplayName
        #endregion // Methods


    }
}
