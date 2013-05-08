using DatabaseConverter.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        #region _Fields
        bool _isCurrentPage;
        #endregion
        public ViewModelBase() 
        { 
            
        }


        public abstract string DisplayName { get; }
        public abstract void LoadPage();
        public bool IsCurrentPage
        {
            get { return _isCurrentPage; }
            set
            {
                if (value == _isCurrentPage)
                    return;

                _isCurrentPage = value;
                this.NotifyChanged("IsCurrentPage");
            }
        }

        #region Methods

        /// <summary>
        /// Returns true if the user has filled in this page properly
        /// and the wizard should allow the user to progress to the 
        /// next page in the workflow.
        /// </summary>
        internal abstract bool IsValid();

        #endregion // Methods

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyChanged(string propertyName)   
        {   
            if (string.IsNullOrEmpty(propertyName))   
                throw new ArgumentNullException("propertyName");   
 
            if (PropertyChanged != null)   
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));   
        }

        #endregion
    }
}