using System;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveAtStartCommand : Command
    {
        #region Constructor

        public RemoveAtStartCommand()
            : base((string)App.Current.FindResource("RemoveAtStartCommandName"),
                    (string)App.Current.FindResource("RemoveAtStartCommandLabelFirst"),                    
                    string.Empty,
                    (string)App.Current.FindResource("RemoveAtStartCommandNameDescription"), 
                    true, false, false)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (parameters.FirstBorder == string.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            int RemoveLength = 0;
            try
            {
                // Get count of symbols to remove.
                RemoveLength = Convert.ToInt32(parameters.FirstBorder);
                sourceText = sourceText.Remove(BEGIN, RemoveLength);
            }
            catch (Exception)
            {
                throw new SimpleEditException((string)App.Current.FindResource("ErrorNumericParameter"));
            }
            
            return sourceText;
        }

        #endregion

        #region Private field

        private const int BEGIN = 0;

        #endregion
    }
}
