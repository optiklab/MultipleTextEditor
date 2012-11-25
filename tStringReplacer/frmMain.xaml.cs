using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;

namespace MultipleTextEditor
{
    public enum Formats
    {
        Default = 0, 
        Unicode = 1,
        BigEndianUnicode = 2,
        ASCII = 3,
        UTF32 = 4,
        UTF7 = 5,
        UTF8 = 6
    }

    /// <summary>
    /// Main UI class for the Application.
    /// Contain logic of update, enable, disable controls, executing main operations.
    /// </summary>
    public partial class frmMain : Window
    {
        #region Contructor

        /// <summary>
        /// Contructor
        /// </summary>
        public frmMain()
        {
            InitializeComponent();
            selectedOperation = CommandSource.workWithFolders;
            _commandParams = new CommandParameters();
            _executorParams = new ExecutorParameters();
            _executorParams.Source = selectedOperation;
            _commands = new CommandsCollection();
            _formats.Add("По умолчанию");
            _formats.Add("Unicode");
            _formats.Add("Big Endian Unicode");
            _formats.Add("ASCII");
            _formats.Add("UTF32");
            _formats.Add("UTF7");
            _formats.Add("UTF8");
        }

        #endregion

        #region Event handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutDialog dlg = new AboutDialog();
            dlg.ShowDialog();
        }

        /// <summary>
        /// Combo box SelectionChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOperationsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = AddOperationsComboBox.SelectedIndex;
            ChangeUI(selected);
        }

        private void ReplaceButton_Checked(object sender, RoutedEventArgs e)
        {
            RemoveButton.IsChecked = false;
            AddButton.IsChecked = false;
            ReplaceButton.IsChecked = true;
            _UpdateComboBox(CommandType.Replace);
        }

        private void AddButton_Checked(object sender, RoutedEventArgs e)
        {
            ReplaceButton.IsChecked = false;
            RemoveButton.IsChecked = false;
            AddButton.IsChecked = true;
            _UpdateComboBox(CommandType.Add);
        }

        private void RemoveButton_Checked(object sender, RoutedEventArgs e)
        {
            ReplaceButton.IsChecked = false;
            AddButton.IsChecked = false;
            RemoveButton.IsChecked = true;
            _UpdateComboBox(CommandType.Remove);
        }

        private void ReplaceButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _SetControlsToDefault();
        }

        private void AddButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _SetControlsToDefault();
        }

        private void RemoveButton_Unchecked(object sender, RoutedEventArgs e)
        {
            _SetControlsToDefault();
        }

        private void WorkWithFoldersButton_Checked(object sender, RoutedEventArgs e)
        {
            WorkWithFileButton.IsChecked = false;
            WorkWithTextButton.IsChecked = false;
            WorkWithFoldersGrid.Visibility = Visibility.Visible;
            WorkWithFilesGrid.Visibility = Visibility.Collapsed;
            WorkWithTextGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            _executorParams.Source = CommandSource.workWithFolders;
        }

        private void WorkWithFileButton_Checked(object sender, RoutedEventArgs e)
        {
            WorkWithFoldersButton.IsChecked = false;
            WorkWithTextButton.IsChecked = false;
            WorkWithFoldersGrid.Visibility = Visibility.Collapsed;
            WorkWithFilesGrid.Visibility = Visibility.Visible;
            WorkWithTextGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Collapsed;
            _executorParams.Source = CommandSource.workWithFiles;
        }

        private void WorkWithTextButton_Checked(object sender, RoutedEventArgs e)
        {
            WorkWithFileButton.IsChecked = false;
            WorkWithFoldersButton.IsChecked = false;
            WorkWithFoldersGrid.Visibility = Visibility.Collapsed;
            WorkWithFilesGrid.Visibility = Visibility.Collapsed;
            WorkWithTextGrid.Visibility = Visibility.Visible;
            SettingsGrid.Visibility = Visibility.Collapsed;
            _executorParams.Source = CommandSource.workWithText;
        }

        /// <summary>
        /// Source Folder text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSourceFolderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.SourcePath = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Mask text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpecifyMaskTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.Mask = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Destination Folder text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDestinationFolderTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.DestinyPath = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Source File text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSourceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.SourcePath = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Destination File text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDestinationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.DestinyPath = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Source Text text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PutTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _executorParams.Text = ((System.Windows.Controls.TextBox)sender).Text;
            //_commandParams.TextToOperate = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// FirstBorder (Floating) text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstBorder_TextChanged(object sender, TextChangedEventArgs e)
        {
            _commandParams.FirstBorder = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// SecondBorder (Floating) text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SecondBorder_TextChanged(object sender, TextChangedEventArgs e)
        {
            _commandParams.SecondBorder = ((System.Windows.Controls.TextBox)sender).Text;
        }

        /// <summary>
        /// Text To Append text field TextChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextField_TextChanged(object sender, TextChangedEventArgs e)
        {
            _commandParams.TextToAppend = ((System.Windows.Controls.TextBox)sender).Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Selection empty 
            ChangeUI(-1);
            AddOperationsComboBox.SelectedIndex = -1;
            SpecifyMaskTextBox.Text = @"*.*";
            PutTextBox.Text = @"Вставьте Ваш текст сюда";
            _SetControlsToDefault();
            WorkWithFoldersButton.IsChecked = true;
            MultipleTextEditor.Properties.Settings.Default.Reload();
        }

        /// <summary>
        /// Source Browse button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSourceBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SelectSourceTextBox.Text = OnOpenFile();
        }

        /// <summary>
        /// Destination Browse button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDestinationBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SelectDestinationTextBox.Text = OnOpenFile();
        }

        /// <summary>
        /// Source Folder Browse button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectSourceFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SelectSourceFolderTextBox.Text = OnOpenFolder();
        }

        /// <summary>
        /// Select Destination Folder button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDestinationFolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            SelectDestinationFolderTextBox.Text = OnOpenFolder();
        }

        /// <summary>
        /// Start button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkOnIt_Click(object sender, RoutedEventArgs e)
        {
            // Define type of source: folder, file, text...
            //_executorParams.Source = (CommandSource)StringReplacerTabs.SelectedIndex;
            StringBuilder result = new StringBuilder();

            if (AddOperationsComboBox.SelectedIndex != -1)
            {
                Thread newWindowThread = new Thread(new ThreadStart(ShowProgressBar));

                try
                {
                    Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                    // Create command execution class.
                    CommandExecuter oper = new CommandExecuter(_executorParams, _commandParams);

                    // Execute appropriate command. Number of selected command appropriate to its number in the Command collection.
                    var cmd = _commands.GetCommand(AddOperationsComboBox.SelectedIndex);

                    // Show progress bar in another thread.
                    newWindowThread.SetApartmentState(ApartmentState.STA);
                    newWindowThread.IsBackground = true;
                    newWindowThread.Start();

                    // Start operation in main thread.
                    result = oper.ExecuteCommand(cmd);
                    newWindowThread.Abort();

                    frmResults frm = new frmResults(result);
                    frm.Show();
                }
                catch (SimpleEditException ex)
                {
                    newWindowThread.Abort();
                    System.Windows.MessageBox.Show(ex.Message);
                }
                catch (Exception)
                {
                    newWindowThread.Abort();
                    System.Windows.MessageBox.Show((string)App.Current.FindResource("TextNotFoundError"));
                }

                Mouse.OverrideCursor = null;
            }
            else
                System.Windows.MessageBox.Show((string)App.Current.FindResource("ErrorFillParameters"));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowProgressBar()
        {
            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.ShowInTaskbar = false;
            progressWindow.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelIt_Click(object sender, RoutedEventArgs e)
        {
            //TODO. Command executor in other process.  .NET 4?
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeIt_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            WorkWithFoldersGrid.Visibility = Visibility.Collapsed;
            WorkWithFilesGrid.Visibility = Visibility.Collapsed;
            WorkWithTextGrid.Visibility = Visibility.Collapsed;
            SettingsGrid.Visibility = Visibility.Visible;
            FormatsComboBox.Items.Clear();
            FormatsComboBox.Items.Add(@"Default");
            FormatsComboBox.Items.Add(@"Unicode");
            FormatsComboBox.Items.Add(@"Big Endian Unicode");
            FormatsComboBox.Items.Add(@"ASCII");
            FormatsComboBox.Items.Add(@"UTF32");
            FormatsComboBox.Items.Add(@"UTF7");
            FormatsComboBox.Items.Add(@"UTF8");
            FormatsComboBox.SelectedIndex = 0;
            WorkWithFoldersButton.IsChecked = false;
            WorkWithFileButton.IsChecked = false;
            WorkWithTextButton.IsChecked = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormatsComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Initialize collection of formats
            switch (FormatsComboBox.SelectedIndex)
            {
                case (int)Formats.Default:
                    _executorParams.Format = Encoding.Default;
                    break;
                case (int)Formats.Unicode:
                    _executorParams.Format = Encoding.Unicode;
                    break;
                case (int)Formats.BigEndianUnicode:
                    _executorParams.Format = Encoding.BigEndianUnicode;
                    break;
                case (int)Formats.ASCII:
                    _executorParams.Format = Encoding.ASCII;
                    break;
                case (int)Formats.UTF32:
                    _executorParams.Format = Encoding.UTF32;
                    break;
                case (int)Formats.UTF7:
                    _executorParams.Format = Encoding.UTF7;
                    break;
                case (int)Formats.UTF8:
                    _executorParams.Format = Encoding.UTF8;
                    break;
                default:
                    break;
            }

            // Initialize check boxes
            IncludeParametersCheckBox.IsChecked = _commandParams.IncludeParameters;
            CheckRegisterCheckBox.IsChecked = _commandParams.IsCaseSensitive;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IncludeParametersCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (IncludeParametersCheckBox.IsChecked == true)
            {
                _commandParams.IncludeParameters = true;
            }
            else
            {
                _commandParams.IncludeParameters = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckRegisterCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRegisterCheckBox.IsChecked == true)
            {
                _commandParams.IsCaseSensitive = true;
            }
            else
            {
                _commandParams.IsCaseSensitive = false;
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Method shows to user Open Folder dialog and returns path to the folder.
        /// </summary>
        /// <returns>Selected path in the Open Folder dialog</returns>
        private String OnOpenFolder()
        {
            String textboxString = String.Empty;
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textboxString = fbd.SelectedPath;
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

            return textboxString;
        }

        /// <summary>
        /// Method shows to user OpenFileDialog and returns path to the file.
        /// </summary>
        /// <returns>Selected path in the Open File dialog</returns>
        private String OnOpenFile()
        {
            String textboxString = String.Empty;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = (string)App.Current.FindResource("FileDialogSettings");

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (ofd.CheckFileExists == true)
                    {
                        textboxString = ofd.FileName;
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

            return textboxString;
        }

        /// <summary>
        /// Method changes visibility of Floating Text Fields appropriate to selected command options.
        /// </summary>
        /// <param name="SelectedItem">Number of selected command from the Combo Box.</param>
        private void ChangeUI(int SelectedItem)
        {
            if (SelectedItem >= 0 && SelectedItem < _commands.GetCount())
            {
                if (_commands.GetCommand(SelectedItem) != null)
                {
                    // First text block.
                    if (_commands.GetCommand(SelectedItem).IsFirstFieldNeeded == true)
                    {
                        FirstBorder.Visibility = Visibility.Visible;
                        firstBorderLabel.Visibility = Visibility.Visible;
                        firstBorderLabel.Content = _commands.GetCommand(SelectedItem).FirstLabelText;
                    }
                    else
                    {
                        FirstBorder.Visibility = Visibility.Collapsed;
                        firstBorderLabel.Visibility = Visibility.Collapsed;
                    }
                    // Second text block.
                    if (_commands.GetCommand(SelectedItem).IsSecondFieldNeeded == true)
                    {
                        SecondBorder.Visibility = Visibility.Visible;
                        secondBorderLabel.Visibility = Visibility.Visible;
                        secondBorderLabel.Content = _commands.GetCommand(SelectedItem).SecondLabelText;
                    }
                    else
                    {
                        SecondBorder.Visibility = Visibility.Collapsed;
                        secondBorderLabel.Visibility = Visibility.Collapsed;
                    }
                    // Main text block.
                    if (_commands.GetCommand(SelectedItem).IsMainFieldNeeded == true)
                    {
                        TextField.Visibility = Visibility.Visible;
                        mainTextLabel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        TextField.Visibility = Visibility.Collapsed;
                        mainTextLabel.Visibility = Visibility.Collapsed;
                    }
                    AddOperationLabel.Text = _commands.GetCommand(SelectedItem).Description;
                    // Update.
                    UpdateLayout();
                }
            }
            else if (SelectedItem == -1)
            {
                FirstBorder.Visibility = Visibility.Collapsed;
                firstBorderLabel.Visibility = Visibility.Collapsed;
                SecondBorder.Visibility = Visibility.Collapsed;
                secondBorderLabel.Visibility = Visibility.Collapsed;
                TextField.Visibility = Visibility.Visible;
                mainTextLabel.Visibility = Visibility.Visible;
                AddOperationLabel.Text = (string)App.Current.FindResource("AddOperationLabelHint");
            }
        }

        /// <summary>
        /// Update combo box items collection.
        /// </summary>
        /// <param name="type">Type of commands which should be loaded to Combo box items collection.</param>
        private void _UpdateComboBox(CommandType type)
        {
            if (type == CommandType.None)
                AddOperationsComboBox.IsEnabled = false;
            else
                AddOperationsComboBox.IsEnabled = true;
            //Clear combo box
            AddOperationsComboBox.SelectedIndex = -1;
            AddOperationsComboBox.Items.Clear();
            //Update collection for combo box
            _commands.UpdateCollection(type); 
            //Fill combo box items collection
            for (int i = 0; i < _commands.GetCount(); i++)
            {
                Command cmd = _commands.GetCommand(i);
                AddOperationsComboBox.Items.Add(cmd.CommandTitle);
            }      
        }

        /// <summary>
        /// Method set buttons to unchecked and combo box to disabled states 
        /// </summary>
        private void _SetControlsToDefault()
        {
            ReplaceButton.IsChecked = false;
            RemoveButton.IsChecked = false;
            AddButton.IsChecked = false;
            _UpdateComboBox(CommandType.None);
        }

        #endregion   

        #region Private fields

        /// <summary>
        /// Parameters for current command.
        /// </summary>
        private CommandParameters _commandParams;

        /// <summary>
        /// Current collection of commands.
        /// </summary>
        private CommandsCollection _commands;

        /// <summary>
        /// Execution parameters of current command.
        /// </summary>
        private ExecutorParameters _executorParams;

        /// <summary>
        /// Selected type of operation.
        /// </summary>
        private CommandSource selectedOperation;

        /// <summary>
        /// Collection of available text formats.
        /// </summary>
        private List<string> _formats = new List<string>();

        #endregion
    }
}
