
namespace MultipleTextEditor
{
    /// <summary>
    /// 
    /// </summary>
    internal class Command
    {
        #region Constructor

        public Command(string commandTitle, string firstLabelText, string secondLabelText, string description, bool IsFirstField, bool IsSecondField, bool IsMainField)
        {
            _commandTitle = commandTitle;
            _firstLabelText = firstLabelText;
            _secondLabelText = secondLabelText;
            _isFirstFieldNeeded = IsFirstField;
            _isSecondFieldNeeded = IsSecondField;
            _isMainFieldNeeded = IsMainField;
            _description = description;
            _changed = 0;
            _found = 0;
        }

        public Command(string commandTitle) :
            this(commandTitle, string.Empty, string.Empty, string.Empty, false, false, true)
        {  }

        #endregion

        #region Public method

        public virtual string Execute(string sourceText, CommandParameters parameters)
        {
            System.Windows.MessageBox.Show("Main Command class!");
            return sourceText;
        }

        #endregion

        #region Public property

        /// <summary>
        /// 
        /// </summary>
        public string CommandTitle
        {
            set
            {
                _commandTitle = value;
            }
            get
            {
                return _commandTitle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FirstLabelText
        {
            get
            {
                return _firstLabelText;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string SecondLabelText
        {
            get
            {
                return _secondLabelText;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFirstFieldNeeded
        {
            get
            {
                return _isFirstFieldNeeded;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public bool IsMainFieldNeeded
        {
            get
            {
                return _isMainFieldNeeded;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSecondFieldNeeded
        {
            get
            {
                return _isSecondFieldNeeded;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ResultReport
        {
            get
            {
                return string.Format(@"{0} elements found, {1} changed", _found, _changed);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// 
        /// </summary>
        private string _commandTitle;

        /// <summary>
        /// 
        /// </summary>
        private string _firstLabelText;

        /// <summary>
        /// 
        /// </summary>
        private string _secondLabelText;

        /// <summary>
        /// 
        /// </summary>
        private bool _isFirstFieldNeeded;

        /// <summary>
        /// 
        /// </summary>
        private bool _isSecondFieldNeeded;

        /// <summary>
        /// 
        /// </summary>
        private bool _isMainFieldNeeded;

        /// <summary>
        /// 
        /// </summary>
        private int _changed;

        /// <summary>
        /// 
        /// </summary>
        private int _found;

        /// 
        /// </summary>
        private string _description;

        #endregion
    }
}
