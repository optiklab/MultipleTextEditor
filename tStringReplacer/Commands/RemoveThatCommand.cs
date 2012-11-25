using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveThatCommand : Command
    {
        #region Constructor

        public RemoveThatCommand()
            : base((string)App.Current.FindResource("RemoveThatCommandName"),
                    string.Empty,
                    string.Empty,
                    (string)App.Current.FindResource("RemoveThatCommandNameDescription"), 
                    false, false, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.TextToAppend))
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            // Escape from special symbols.
            string TextToRemove = Regex.Escape(parameters.TextToAppend);
            Regex regText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regText = new Regex(TextToRemove, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regText = new Regex(TextToRemove, RegexOptions.Multiline);

            // Generate Match collection: find all occurrences of TextToRemove in sourceText.
            MatchCollection mc = regText.Matches(sourceText);
            if (mc.Count <= 0)
                throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));

            int SearchTemplateLength = TextToRemove.Length;
            // Remove text in all occurrences.
            int Index = 0;
            int IndexAdd = 0; //every time we remove text, indexes changes
            foreach (Match match in mc)
            {
                Index = match.Index + IndexAdd;
                sourceText = sourceText.Remove(Index, SearchTemplateLength);
                IndexAdd -= SearchTemplateLength;
            }

            return sourceText;
        }

        #endregion
    }
}
