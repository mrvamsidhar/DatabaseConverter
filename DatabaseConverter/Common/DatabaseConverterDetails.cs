using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConverter.Common
{
    public class DatabaseConverterDetails
    {
        public DatabaseConverterDetails() 
        {
            Features = new ObservableCollection<FeatureListItem>();
          //  SelectedFeatures = new ObservableCollection<FeatureListItem>();
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Server { set; get; }
        public string Database { set; get; }
        public bool IsWindowsAuthentication { get; set; }
        public ObservableCollection<FeatureListItem> Features { get; set; }
       // public ObservableCollection<FeatureListItem> SelectedFeatures { get; set; }
        public string ServerConnectionString()
        {
            string strConnectionString = string.Empty;
            if (IsWindowsAuthentication)
                strConnectionString = string.Format(@"Data Source={0};Integrated Security=True", Server);
            else
                strConnectionString = string.Format("Server={0}; user id={1}; Pwd={2}", Server, UserName, Password);// AIS-163\\TestServer,sa,ais@2012);

            return strConnectionString;
        }
        public string DatabaseConnectionString()
        {
            string strConnectionString = string.Empty;
            if (IsWindowsAuthentication)
                strConnectionString = string.Format(@"Data Source={0};Integrated Security=True", Server);
            else
                strConnectionString = string.Format("Server={0}; user id={1}; Pwd={2};Database={3};", Server, UserName, Password, Database);// AIS-163\\TestServer,sa,ais@2012);

            return strConnectionString;
        }
    }

    
}
