using DatabaseConverter.Command;
using DatabaseConverter.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DatabaseConverter.ViewModels
{
    public sealed class DatabaseDetailsViewModel:ViewModelBase
    {
        readonly ObservableCollection<string> _sqlserverList;
        readonly ObservableCollection<string> _sqldatabseList;
        string _serverName = string.Empty;
        bool _checkfornetworkservers = false;
        RelayCommand _connect;
        public DatabaseDetailsViewModel()
        {
            _sqlserverList = new ObservableCollection<string>();
            _sqldatabseList = new ObservableCollection<string>();
            IsWindowsAuthentication = true;
          
            _initialize();

        }
        public override void LoadPage() { }
        #region Properties
        public ObservableCollection<string> ServerList
        {
            get { return _sqlserverList; }
        }
        public ObservableCollection<string> DatabaseList
        {
            get { return _sqldatabseList; }
        }
        public string ServerName 
        {
            get { return App.Result.Server; }
            set
            {
                App.Result.Server = value;
                OnServerNameChanged();
            }
        }
        public string DatabaseName 
        {
            get 
            { 
                return App.Result.Database; 
            }
            set 
            {
                App.Result.Database = value;
            } 
        }
        public bool CheckForNetworkServers
        {
            get
            { 
                return _checkfornetworkservers;
            }
            set
            {
                _checkfornetworkservers = value;
                OnCheckfornetworkservers();
            }
        }
        public bool IsWindowsAuthentication 
        {
            get  
            {  
                return App.Result.IsWindowsAuthentication; 
            }
            set
            {
                App.Result.IsWindowsAuthentication = value;
            }
        }
      
        /// <summary>
        /// Returns the command which, when executed, cancels the order 
        /// and causes the Wizard to be removed from the user interface.
        /// </summary>
        public ICommand Connect
        {
            get
            {
                if (_connect == null)
                    _connect = new RelayCommand(() => this.ConnecttoSqlserver());
                return _connect;
            }
        }
        public string UserName
        {
            get
            { 
                return App.Result.UserName;
            }
            set
            {
                App.Result.UserName = value;
            }
        }
        public string Password
        {
            get
            { 
                return App.Result.Password;
            }
            set
            {
                App.Result.Password = value;
            }
        }
        #endregion
        #region Methods
        internal override bool IsValid()
        {
            return true;
        }
        #region DisplayName

        public override string DisplayName
        {
            get { return "Select Database"; }
        }

        #endregion // DisplayName
        #endregion // Methods

        #region PrivateProperties
        private void _initialize()
        {
           _getlocalhostSQLServerDetails();
        }
        private void _getlocalhostSQLServerDetails()
        {
            _sqlserverList.Clear();
            try
            {
                RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey key = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL");

                foreach (String element in key.GetValueNames())
                {
                    if (element == "MSSQLSERVER")
                        _sqlserverList.Add(System.Environment.MachineName);
                    else
                        _sqlserverList.Add(System.Environment.MachineName + @"\" + element);

                }
                key.Close();
                baseKey.Close();
                if (_sqlserverList.Count > 0)
                    ServerName = _sqlserverList[0];
            }
            catch (Exception exe)
            {
                throw (exe);
            }
        }
        private void OnServerNameChanged()
        {
            _sqldatabseList.Clear();
            IsWindowsAuthentication = true;
           
        }
        private void OnCheckfornetworkservers()
        {
            if (CheckForNetworkServers)
                _getnetworkservers();
            else
                _getlocalhostSQLServerDetails();
        }
        private void _getnetworkservers()
        {
            ServerList.Clear();
            List<SqlServerList> list = Utils.GetSQLInstanceNames();
            if(list == null)
               throw(new Exception("No Network servers found."));
           foreach (SqlServerList item in list)
           {
               if (item.InstanceName != string.Empty)
                   ServerList.Add(item.ServerName + @"\" + item.InstanceName);
               else
                   ServerList.Add(item.ServerName);
           }
        }
        private void _getlocalserverdatabasenames()
        {
            _sqldatabseList.Clear();
           // String conxString = "Data Source=AIS-163; Integrated Security=SSPI;";
          //  String conxString = "Server=AIS-163\\TestServer; user id=sa; Pwd=ais@2012";
           // String conxString = "Data Source=l\\PRDSERVER; Integrated Security=SSPI;";
           // String conxString = @"Data Source=AIS-163;Integrated Security=True";
            try
            {
                using (SqlConnection sqlConx = new SqlConnection(App.Result.ServerConnectionString()))
                {
                    sqlConx.Open();
                    DataTable tblDatabases = sqlConx.GetSchema("Databases");
                    sqlConx.Close();
                    foreach (DataRow row in tblDatabases.Rows)
                    {
                        _sqldatabseList.Add(row["database_name"].ToString());
                    }
                    if (_sqldatabseList.Count > 0)
                        DatabaseName = _sqldatabseList[0];
                }
            }
            catch (Exception exe)
            {
                throw (exe);
            }
        }
        private void ConnecttoSqlserver()
        {
            _getlocalserverdatabasenames();
        }
        
        #endregion
    }
}
