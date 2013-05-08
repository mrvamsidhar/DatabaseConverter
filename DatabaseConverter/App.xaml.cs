using DatabaseConverter.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DatabaseConverter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static DatabaseConverterDetails _selecteddbdetails;

         private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            if (DoHandle)
            {
                //Handling the exception within the UnhandledException handler.
                MessageBox.Show(e.Exception.Message, "Exception Caught",
                                        MessageBoxButton.OK, MessageBoxImage.Error);
                e.Handled = true;
            }
            else
            {
                //If you do not set e.Handled to true, the application will close due to crash.
                MessageBox.Show(e.Exception.Message, "Uncaught Exception");
                e.Handled = true;
            }
        }
        public bool DoHandle { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //var ex = e.ExceptionObject as Exception;
            //MessageBox.Show(ex.Message, "Exception");
            
        }

        public static DatabaseConverterDetails Result
        {
            get 
            {
                if (_selecteddbdetails == null)
                { 
                    _selecteddbdetails = new DatabaseConverterDetails(); 
                }
                return _selecteddbdetails;
            }
        }
    }
}
