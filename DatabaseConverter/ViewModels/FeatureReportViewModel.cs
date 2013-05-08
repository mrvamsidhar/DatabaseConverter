using DatabaseConverter.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.ViewModels
{
    public sealed class FeatureReportViewModel:ViewModelBase
    {
        ReportConverter _converter = new ReportConverter();
        ObservableCollection<FeatureListItem> _selectedfeatures;
        public FeatureReportViewModel() { }

        #region overridden methods
        internal override bool IsValid()
        {
            return true;
        }
        public override string DisplayName
        {
            get { return "View Report"; }
        }
        public override void LoadPage()
        {
            _loadselectedfeatures();
            _converter.Convert(SelectedFeatures,true);
        }
        #endregion
        #region Properties
        public ObservableCollection<FeatureListItem> SelectedFeatures
        {
            get { return _selectedfeatures; }
        }
        public ReportConverter ReportConverter
        { get { return _converter; } }
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
        #endregion
        #region Private Methods
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
        #endregion
    }
}
