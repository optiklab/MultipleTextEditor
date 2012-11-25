using System;
using System.Text;

namespace MultipleTextEditor
{
    internal sealed class ExecutorParameters
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ExecutorParameters()
        {
            _source = CommandSource.workWithFolders;
            _format = Encoding.Default;
        }

        #endregion

        #region Public property

        /// <summary>
        /// Source for text: files, folders, UI text field
        /// </summary>
        public CommandSource Source
        {
            set
            {
                _source = value;
            }
            get
            {
                return _source;
            }
        }

        /// <summary>
        /// Folder path.
        /// </summary>
        public String SourcePath
        {
            set
            {
                _sourcePath = value;
            }
            get
            {
                return _sourcePath;
            }
        }

        /// <summary>
        /// File path.
        /// </summary>
        public String DestinyPath
        {
            set
            {
                _destinyPath = value;
            }
            get
            {
                return _destinyPath;
            }
        }

        /// <summary>
        /// Mask.
        /// </summary>
        public String Mask
        {
            set
            {
                _mask = value;
            }
            get
            {
                return _mask;
            }

        }

        /// <summary>
        /// Text.
        /// </summary>
        public String Text
        {
            set
            {
                _text = value;
            }
            get
            {
                return _text;
            }

        }

        /// <summary>
        /// Format.
        /// </summary>
        public Encoding Format
        {
            set
            {
                _format = value;
            }
            get
            {
                return _format;
            }

        }

        #endregion

        #region Private fields

        /// <summary>
        /// Command source.
        /// </summary>
        private CommandSource _source;

        /// <summary>
        /// Folder path.
        /// </summary>
        private String _sourcePath;

        /// <summary>
        /// File path.
        /// </summary>
        private String _destinyPath;

        /// <summary>
        /// Mask for files searching.
        /// </summary>
        private String _mask;

        /// <summary>
        /// Text for work in case of CommandSource == Work With Text
        /// </summary>
        private String _text;

        /// <summary>
        /// Type of text format.
        /// </summary>
        private Encoding _format;

        #endregion
    }
}
