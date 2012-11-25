
namespace MultipleTextEditor.Commands
{
    internal sealed class AddAtEndCommand : Command
    {
        #region Constructor

        public AddAtEndCommand()
            : base((string)App.Current.FindResource("AddAtEndCommandName"), 
                    string.Empty, 
                    string.Empty,
                    (string)App.Current.FindResource("AddAtEndCommandNameDescription"), 
                    false, false, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            //create regular expression object from Text
            string resultText = string.Empty;

            if (parameters.TextToAppend != string.Empty)
                resultText = sourceText + parameters.TextToAppend;
            else
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            return resultText;
        }

        #endregion
    }
}
