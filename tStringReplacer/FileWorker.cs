using System;
using System.Text;
using System.IO;

namespace MultipleTextEditor
{
    internal sealed class FileWorker
    {
        #region Public method

        /// <summary>
        /// Method read data from file.
        /// </summary>
        /// <param name="filePath">Path to file to read.</param>
        /// <returns>String of read data.</returns>
        public String ReadFromFile(String filePath)
        {
            String readText = String.Empty;

            if (filePath == String.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorSourcePathIsEmpty"));
            
            try
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                try
                {
                    readText = sr.ReadToEnd();
                }
                catch (OutOfMemoryException)
                {
                    readText = String.Empty;
                    throw new SimpleEditException((string)App.Current.FindResource("ErrorOutMemory"));
                }
                finally
                {
                    sr.Close();
                }
            }
            catch(Exception)
            {
                readText = String.Empty;
                String result = String.Format((string)App.Current.FindResource("ErrorCantReadFile"), filePath);
                throw new SimpleEditException(result);
            }
            return readText;
        }

        /// <summary>
        /// Method writes data to specified file.
        /// </summary>
        /// <param name="filePath">File to write.</param>
        /// <param name="textToWrite">Text to write.</param>
        /// <param name="format">Encoding format.</param>
        public void WriteToFile(String filePath, String textToWrite, Encoding format)
        {
            if (filePath == String.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorDestinyPathIsEmpty"));
            try
            {
                StreamWriter writetofile = new StreamWriter(filePath, false, format, textToWrite.Length);
                try
                {
                    writetofile.Write(textToWrite);
                }
                catch (OutOfMemoryException)
                {
                    throw new SimpleEditException((string)App.Current.FindResource("ErrorOutMemory"));
                }
                finally
                {
                    writetofile.Close();
                }
            }
            catch (Exception)
            {
                String result = String.Format((string)App.Current.FindResource("ErrorCantWriteFile"), filePath);
                throw new SimpleEditException(result);
            }
        }

        /// <summary>
        /// Method writes data to specified file.
        /// </summary>
        /// <param name="filePath">File to write.</param>
        /// <param name="textToWrite">Text to write.</param>
        /// <param name="append">Is need to append text?</param>
        public void WriteToFile(String filePath, String textToWrite, bool append)
        {
            if (filePath == String.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorDestinyPathIsEmpty"));

            try
            {
                StreamWriter writetofile = new StreamWriter(filePath, append, Encoding.Default, textToWrite.Length);
                try
                {
                    writetofile.Write(textToWrite);
                }
                catch (OutOfMemoryException)
                {
                    throw new SimpleEditException((string)App.Current.FindResource("ErrorOutMemory"));
                }
                finally
                {
                    writetofile.Close();
                }
            }
            catch (Exception)
            {
                String result = String.Format((string)App.Current.FindResource("ErrorCantWriteFile"), filePath);
                throw new SimpleEditException(result);
            }
        }

        #endregion
    }
}
