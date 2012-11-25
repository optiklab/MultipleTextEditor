using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class AddAfterCommand : Command
    {
        #region Constructor 

        public AddAfterCommand()
            : base((string)App.Current.FindResource("AddAfterCommandName"), 
                    (string)App.Current.FindResource("AddAfterCommandLabelFirst"),
                    string.Empty,
                    (string)App.Current.FindResource("AddAfterCommandLabelDescription"), 
                    true, false, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.TextToAppend) || string.IsNullOrEmpty(parameters.FirstBorder))
            {
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));
            }

            // Escape from special symbols.
            string TextToReplace = Regex.Escape(parameters.FirstBorder);
            Regex regText;
            // Case check
            if(parameters.IsCaseSensitive == false)
                regText = new Regex(TextToReplace, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regText = new Regex(TextToReplace, RegexOptions.Multiline);
            // Generate Match collection: find all occurrences of TextToReplase in sourceText.
            MatchCollection mc = regText.Matches(sourceText);
            if (mc.Count <= 0)
                throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));

            string TextToAppend = parameters.TextToAppend;
            int AppendLength = TextToAppend.Length;
            int SearchTemplateLength = TextToReplace.Length;
            // Add text in all occurrences.
            int Index = 0;
            int IndexAdd = 0; //every time we add text, indexes changes
            foreach (Match match in mc)
            {
                Index = match.Index + SearchTemplateLength + IndexAdd;
                sourceText = sourceText.Insert(Index, TextToAppend);
                IndexAdd += AppendLength;
            }

            return sourceText;
        }

        #endregion
    }
}
