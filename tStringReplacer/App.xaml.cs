using System;
using System.Windows;
using System.Text;

namespace MultipleTextEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {            
            StringBuilder exMessage = new StringBuilder();

            // Add info
            exMessage.Append(DateTime.Now);
            exMessage.Append(System.Environment.NewLine);
            exMessage.Append("Source: \n");
            exMessage.Append(e.Exception.Source);
            exMessage.Append("\nException message: \n");
            exMessage.Append(e.Exception.Message);
            exMessage.Append("\nStack trace: \n");
            exMessage.Append(e.Exception.StackTrace);
            // Log to file
            FileWorker filewriter = new FileWorker();

            filewriter.WriteToFile(@"C:\MultiTextEditorLog.txt", exMessage.ToString(), true);

            MessageBox.Show(@"Exception occurs during work with the application. Please, send C:\MultiTextEditorLog.txt log about the error to developer!");
        }
    }
}
