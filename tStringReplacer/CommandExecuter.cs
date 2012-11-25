using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace MultipleTextEditor
{
    /// <summary>
    /// CommandExecuter class.
    /// Execute command, make all operations for command, build report.
    /// </summary>
    internal sealed class CommandExecuter
    {
        #region Constuctor

        /// <summary>
        /// Constructor.
        /// </summary>
        public CommandExecuter(ExecutorParameters executorParams, CommandParameters commandParams)
        {
            _report = new StringBuilder();
            _executorParams = executorParams;
            _commandParams = commandParams;
        }

        #endregion

        #region Public method

        /// <summary>
        /// Method execute appropriate command with parameters initialized during craetion of instance.
        /// </summary>
        /// <param name="cmd">Command which has to be executed.</param>
        public StringBuilder ExecuteCommand(Command cmd)
        {
            _cmd = cmd;
            switch (_executorParams.Source)
            {
                case CommandSource.workWithFolders:
                    _ExecuteOperationForFolders();
                    break;
                case CommandSource.workWithFiles:
                    _ExecuteOperationForFiles(_executorParams.SourcePath, _executorParams.DestinyPath);
                    break;
                case CommandSource.workWithText:
                    String outText = _ExecuteCommand(_executorParams.Text);
                    //TO LOG
                    _report.Append(outText);
                    break;
                default:
                    break;
            }
            return _report;
        }

        #endregion

        #region Private method

        /// <summary>
        /// Function find directory and text in files under this directory
        /// </summary>
        /// <param name="cmd"></param>
        private void _ExecuteOperationForFolders()
        {
            String Mask = _MaskConstraint(_executorParams.Mask);
            //create regular expression object from Mask            
            Regex regMask = new Regex(Mask, RegexOptions.IgnoreCase);
            
            ulong Matches = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(_executorParams.SourcePath);
                // Format result string.
                String Line = String.Format((string)App.Current.FindResource("ResultFolder"), di.FullName);
                _report.AppendLine(Line + Environment.NewLine);
                Matches = _FindFilesInFolder(di, regMask);
            }
            catch (PathTooLongException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorFilePath"));
            }
            catch (IOException)
            {
                //to LOG:
                String result = String.Format((string)App.Current.FindResource("ErrorDirectoryAccess"), _executorParams.SourcePath);
                _report.AppendLine(result);
            }
            catch (UnauthorizedAccessException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorSecurity"));
            }
            catch (OutOfMemoryException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorOutMemory"));
            }
            catch (ArgumentException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorFilePath"));
            }
            catch (System.Security.SecurityException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorSecurity"));
            }


            //TO LOG
            String temp = String.Format((string)App.Current.FindResource("NFilesMatch"), Matches);
            temp += Environment.NewLine;
            //_report.AppendLine(temp);
            _report.Insert(0, temp);
            //show status in Log textbox
            //txbLog.Text = Matches.ToString() + " files found!" + Environment.NewLine + txbLog.Text;          
        }

        /// <summary>
        /// Method reads data from file.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <returns></returns>
        private String _ReadDataFromFile(String filePath)
        {
            String Text = String.Empty;
            //check file info
            FileInfo fi = new FileInfo(filePath);
            
            if(fi.Exists == true)
            {

                    FileWorker fw = new FileWorker();
                    Text = fw.ReadFromFile(filePath);
            }
            return Text;
        }

        /// <summary>
        /// Method writes data to file.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="data">Data to write.</param>
        private void _WriteDataToFile(String filePath, String data, Encoding format)
        {
            //check file info
            FileInfo fi = new FileInfo(filePath);

            FileWorker fw = new FileWorker();
            fw.WriteToFile(filePath, data, format);
        }

        /// <summary>
        /// Recurrence function  search for Files which specified by Mask in appropriate Folder and all subfolders with recursion call.
        /// </summary>
        /// <param name="di">Path to the searhing folder.</param>
        /// <param name="regMask">Mask of file to search.</param>
        /// <returns></returns>
        private ulong _FindFilesInFolder(DirectoryInfo di, Regex regMask)
        {
            ulong CountOfMatchFiles = 0;
            FileInfo[] fi = null;

            try
            {
                fi = di.GetFiles();
                // Search in list of files.
                foreach (FileInfo f in fi)
                {
                    // File is appropriate to Mask.
                    if (regMask.IsMatch(f.Name)) 
                    {
                        ++CountOfMatchFiles;
                        String sourceFilePath = di.FullName + @"\" + f.Name;
                        String destFilePath = _executorParams.DestinyPath;
                        // DestinyPath is non obligatory field. If it is empty, text should be written to the same file.
                        if (destFilePath == String.Empty || destFilePath == null)
                            destFilePath = sourceFilePath;
                        else
                            destFilePath += @"\" + f.Name;
                        //
                        _ExecuteOperationForFiles(sourceFilePath, destFilePath);
                    }
                }
                // Get subfolder.
                DirectoryInfo[] diSub = di.GetDirectories();  // list of subfolders
                foreach (DirectoryInfo diSubDir in diSub) // Recursion for every subfolder
                    CountOfMatchFiles += _FindFilesInFolder(diSubDir, regMask);
            }
            catch (DirectoryNotFoundException)
            {
                //to LOG:
                String result = String.Format((string)App.Current.FindResource("ErrorDirectoryAccess"), di.Name);
                _report.AppendLine(result);
            }
            catch (IOException)
            {
                //to LOG:
                String result = String.Format((string)App.Current.FindResource("ErrorDirectoryAccess"), di.Name);
                _report.AppendLine(result);
            }
            catch (UnauthorizedAccessException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorSecurity"));
            }
            catch(ArgumentNullException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorFilePath"));
            }
            catch (System.Security.SecurityException)
            {
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ErrorSecurity"));
            }
            return CountOfMatchFiles;
        }

        /// <summary>
        /// Method execute operation for appropriate file.
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destinyPath"></param>
        private void _ExecuteOperationForFiles(String sourcePath, String destinyPath)
        {
            try
            {
                String infoFromFile = _ReadDataFromFile(sourcePath);
                if (infoFromFile != String.Empty && _commandParams != null)
                {
                    //results
                    _report.AppendLine(@"Файл, найденный по маске: " + sourcePath);
                    String outText = _ExecuteCommand(infoFromFile);
                    // DestinyPath is non obligatory field. If it is empty, text should be written to the same file.
                    if (destinyPath == String.Empty)
                        _WriteDataToFile(sourcePath, outText, _executorParams.Format);
                    else
                        _WriteDataToFile(destinyPath, outText, _executorParams.Format);
                    _report.AppendLine((string)App.Current.FindResource("ResultOfOperation") +
                                        (string)App.Current.FindResource("ResultOperationCompleted") +
                                        Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                //to System Message:
                //throw new SimpleEditException(OptikLaboratory.MultipleTextEditor.Properties.Resources.ErrorSystem);
                
                //to LOG:
                _report.AppendLine((string)App.Current.FindResource("ResultOfOperation") +
                                    ex.Message + 
                                    Environment.NewLine);
            } 
        }

        /// <summary>
        /// Method executes the command for user text (Data) and return its results
        /// </summary>
        /// <param name="Data">User text (from file, text field)</param>
        /// <returns>Changed text from the Command</returns>
        private String _ExecuteCommand(String Data)
        {
            return _cmd.Execute(Data, _commandParams);
        }

        /// <summary>
        /// Method exclude special symbols from the mask
        /// </summary>
        /// <param name="Mask">Mask string which was entered by user</param>
        /// <returns>Mask for the RegEx class</returns>
        private String _MaskConstraint(String Mask)
        {
            try
            {
                // Replace . to \.
                Mask = Mask.Replace(".", @"\."); /* (".", "\\.") */
                // Replace ? to .
                Mask = Mask.Replace("?", ".");
                // Replace * to .*
                Mask = Mask.Replace("*", ".*");
                // We need to find string exact to mask:
                Mask = "^" + Mask + "$";
            }
            catch (Exception)
            {
                //to System Message:
                throw new SimpleEditException((string)App.Current.FindResource("ErrorMaskWrong"));
            }
            return Mask;
        }

        #endregion

        #region Private field

        /// <summary>
        /// Report which builds during the operation.
        /// </summary>
        private StringBuilder _report;

        /// <summary>
        /// Execution parameters.
        /// </summary>
        private ExecutorParameters _executorParams;

        /// <summary>
        /// Command parameters.
        /// </summary>
        private CommandParameters _commandParams;

        /// <summary>
        /// Command which will be executed.
        /// </summary>
        private Command _cmd;

        #endregion
    }
}
