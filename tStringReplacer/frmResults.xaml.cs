using System;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace MultipleTextEditor
{
    /// <summary>
    /// Interaction logic for frmResults.xaml
    /// </summary>
    public partial class frmResults : Window
    {
        #region Constructor

        public frmResults(StringBuilder data)
        {
            InitializeComponent();
            _data = data;
        }

        #endregion

        #region Event handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_data.Length == 0)
                TextResults.Text = (string)App.Current.FindResource("DefaultResults");
            else
                TextResults.Text = _data.ToString();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Find path.
            String filePath = String.Empty;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = (string)App.Current.FindResource("FileDialogSettings");

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (ofd.CheckFileExists == true)
                    {
                        filePath = ofd.FileName;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                System.Windows.Forms.MessageBox.Show((string)App.Current.FindResource("ErrorOutMemory"));
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show((string)App.Current.FindResource("ErrorCantOpenDialog"));
            }

            // Write.
            FileWorker fw = new FileWorker();
            try
            {
                fw.WriteToFile(filePath, _data.ToString(), false);
            }
            catch (SimpleEditException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Public properties

        public StringBuilder Data
        {
            set
            {
                _data = value;
            }
            get
            {
                return _data;
            }
        }

        #endregion

        #region Private field

        private StringBuilder _data;

        #endregion
    }
}
