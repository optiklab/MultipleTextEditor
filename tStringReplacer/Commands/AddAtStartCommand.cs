
namespace MultipleTextEditor.Commands
{
    internal sealed class AddAtStartCommand : Command
    {
        #region Constructor

        public AddAtStartCommand()
            : base((string)App.Current.FindResource("AddAtStartCommandName"),
                    string.Empty,
                    string.Empty,
                    (string)App.Current.FindResource("AddAtStartCommandNameDescription"), 
                    false, false, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            //create regular expression object from Text
            string resultText = string.Empty;

            if (parameters.TextToAppend != string.Empty)
                resultText = sourceText.Insert(0, parameters.TextToAppend);
            else
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            return resultText;
        }

        #endregion
    }
}
