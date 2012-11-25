using System;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveAtEndCommand : Command
    {
        #region Constructor

        public RemoveAtEndCommand()
            : base((string)App.Current.FindResource("RemoveAtEndCommandName"),
                    (string)App.Current.FindResource("RemoveAtEndCommandLabelFirst"),
                    string.Empty,
                    (string)App.Current.FindResource("RemoveAtEndCommandNameDescription"), 
                    true, false, false)
        { }

        #endregion

        #region Public method

        public override String Execute(string sourceText, CommandParameters parameters)
        {
            if (parameters.FirstBorder == string.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));
            int RemoveLength = 0;

            try
            {
                // Get count of symbols to remove.
                RemoveLength = Convert.ToInt32(parameters.FirstBorder);
                sourceText = sourceText.Remove(sourceText.Length - RemoveLength, RemoveLength);
            }
            catch (Exception)
            {
                throw new SimpleEditException((string)App.Current.FindResource("ErrorNumericParameter"));
            }
            return sourceText;
        }

        #endregion
    }
}
