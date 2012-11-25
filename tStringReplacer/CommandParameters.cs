using System;

namespace MultipleTextEditor
{
    /// <summary>
    /// Types of operations
    /// </summary>
    enum CommandType
    {
        None = 0,
        Add = 1,
        Remove = 2,
        Replace = 3
    };

    /// <summary>
    /// Possible sources for work
    /// </summary>
    enum CommandSource
    {
        workWithFolders = 0,
        workWithFiles = 1,
        workWithText = 2
    };

    /// <summary>
    /// Class describe parameters for specified command.
    /// </summary>
    internal sealed class CommandParameters
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CommandParameters()
        {
            _isCaseSensitive = true;
            _includeParameters = false;
        }

        #endregion

        #region Public property

        /// <summary>
        /// First border.
        /// </summary>
        public String FirstBorder
        {
            set
            {
                _firstBorder = value;
            }
            get
            {
                return _firstBorder;
            }
        }

        /// <summary>
        /// Second border.
        /// </summary>
        public String SecondBorder
        {
            set
            {
                _secondBorder = value;
            }
            get
            {
                return _secondBorder;
            }
        }
        
        /// <summary>
        /// Text to append.
        /// </summary>
        public String TextToAppend
        {
            set
            {
                _textToAppend = value;
            }
            get
            {
                return _textToAppend;
            }
        }

        /// <summary>
        /// Is case sensitive.
        /// </summary>
        public bool IsCaseSensitive
        {
            set
            {
                _isCaseSensitive = value;
            }
            get
            {
                return _isCaseSensitive;
            }
        }

        /// <summary>
        /// Include parameters of searching to operation.
        /// </summary>
        public bool IncludeParameters
        {
            set
            {
                _includeParameters = value;
            }
            get
            {
                return _includeParameters;
            }
        }
        
        #endregion

        #region Private fields

        /// <summary>
        /// Text for first border: "from", "count", etc.
        /// </summary>
        private String _firstBorder;

        /// <summary>
        /// Text for seondary border: "to" field.
        /// </summary>
        private String _secondBorder;

        /// <summary>
        /// Text which will be append in operations of Add, Replace.
        /// </summary>
        private String _textToAppend;

        /// <summary>
        /// Is case sensitive.
        /// </summary>
        private bool _isCaseSensitive;

        /// <summary>
        /// Include parameters of searching to operation.
        /// </summary>
        private bool _includeParameters;

        #endregion
    }
}
