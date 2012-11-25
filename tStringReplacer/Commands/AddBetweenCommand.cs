using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class AddBetweenCommand : Command
    {
        #region Constructor

        public AddBetweenCommand()
            : base((string)App.Current.FindResource("AddBetweenCommandName"),
                    (string)App.Current.FindResource("AddBetweenCommandLabelFirst"),
                    (string)App.Current.FindResource("AddBetweenCommandLabelSecond"),
                    (string)App.Current.FindResource("AddBetweenCommandNameDescription"), 
                    true, true, true)
        { }            

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.TextToAppend) || string.IsNullOrEmpty(parameters.FirstBorder) ||
                string.IsNullOrEmpty(parameters.SecondBorder))
            {
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));
            }

            string SearchTemplate = parameters.FirstBorder + parameters.SecondBorder;
            // Escape from special symbols.
            string TextToReplace = Regex.Escape(SearchTemplate);
            Regex regText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regText = new Regex(TextToReplace, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regText = new Regex(TextToReplace, RegexOptions.Multiline);

            // Generate Match collection: find all occurrences of TextToReplase in sourceText.
            MatchCollection mc = regText.Matches(sourceText);
            if (mc.Count <= 0)
                throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));

            string TextToAppend = parameters.TextToAppend;
            int AppendLength = TextToAppend.Length;
            // Add text between all occurrences From and TO.
            int Index = 0;

            int Step = parameters.FirstBorder.Length; // append After From, but Before To
            int IndexAdd = Step; //every time we add text, indexes changes
            foreach (Match match in mc)
            {
                Index = match.Index + IndexAdd;
                sourceText = sourceText.Insert(Index, TextToAppend);
                IndexAdd += AppendLength;
            }

            return sourceText;
        }

        #endregion
    }
}
