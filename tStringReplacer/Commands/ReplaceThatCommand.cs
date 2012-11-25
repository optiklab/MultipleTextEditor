using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class ReplaceThatCommand : Command
    {
        #region Constructor

        public ReplaceThatCommand()
            : base((string)App.Current.FindResource("ReplaceThatCommandName"),
                    (string)App.Current.FindResource("ReplaceThatCommandLabelFirst"),
                    string.Empty,
                    (string)App.Current.FindResource("ReplaceThatCommandNameDescription"), 
                    true, false, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            //create regular expression object from Text
            string resultText = string.Empty;

            if (parameters.TextToAppend != string.Empty && parameters.FirstBorder != string.Empty)
            {
                //escape from special symbols
                string TextToReplace = Regex.Escape(parameters.FirstBorder);

                Regex regText;
                // Case check
                if (parameters.IsCaseSensitive == false)
                    regText = new Regex(TextToReplace, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                else
                    regText = new Regex(TextToReplace, RegexOptions.Multiline);

                resultText = regText.Replace(sourceText, parameters.TextToAppend);

                if(sourceText.CompareTo(resultText) == 0)
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
            }
            else
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            return resultText;
        }

        #endregion
    }
}
