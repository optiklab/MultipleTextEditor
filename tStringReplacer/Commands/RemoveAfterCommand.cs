using System;
using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveAfterCommand : Command
    {
        #region Constructor

        public RemoveAfterCommand()
            : base((string)App.Current.FindResource("RemoveAfterCommandName"),
                    (string)App.Current.FindResource("RemoveAfterCommandLabelFirst"),
                    (string)App.Current.FindResource("RemoveAfterCommandLabelSecond"),
                    (string)App.Current.FindResource("RemoveAfterCommandNameDescription"), 
                    true, true, false)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (parameters.SecondBorder == string.Empty || parameters.FirstBorder == string.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            // Escape from special symbols.
            string TextRemoveAfter = Regex.Escape(parameters.FirstBorder);
            Regex regText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regText = new Regex(TextRemoveAfter, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regText = new Regex(TextRemoveAfter, RegexOptions.Multiline);

            string TextRemove = parameters.SecondBorder;

            if (TextRemove == (string)App.Current.FindResource("TagAllSmall") ||
                TextRemove == (string)App.Current.FindResource("TagAllBig") ||
                TextRemove == (string)App.Current.FindResource("TagAllMiddle"))
            {
                Match match = regText.Match(sourceText);

                if (match.Success)
                {
                    int Index = match.Index + TextRemoveAfter.Length;
                    sourceText = sourceText.Remove(Index, sourceText.Length - Index);
                }
                else
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
            }
            else
            {
                // Generate Match collection: find all occurrences of TextToReplase in sourceText.
                MatchCollection mc = regText.Matches(sourceText);
                int RemoveLength = 0;

                if (mc.Count <= 0)
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));

                try
                {
                    // Get count of symbols to remove.
                    RemoveLength = Convert.ToInt32(TextRemove);
                    int SearchTemplateLength = TextRemoveAfter.Length;
                    // Remove text in all occurrences.
                    int Index = 0;
                    int IndexAdd = 0; //every time remove text, indexes changes
                    // Remove RemoveLength symbols after every occurrence of TextRemoveAfter
                    foreach (Match match in mc)
                    {
                        Index = match.Index + SearchTemplateLength + IndexAdd;
                        sourceText = sourceText.Remove(Index, RemoveLength);
                        IndexAdd -= RemoveLength;
                    }
                }
                catch (Exception)
                {
                    // Can't convert to digit,  find first occurrence of the TextRemove after TextRemoveAfter .
                    string TextToRemove = Regex.Escape(TextRemove);
                    Regex regSubText;
                    // Case check
                    if (parameters.IsCaseSensitive == false)
                        regSubText = new Regex(TextToRemove, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    else
                        regSubText = new Regex(TextToRemove, RegexOptions.Multiline);
                    Match match = regSubText.Match(sourceText, mc[0].Index);
                    sourceText = sourceText.Remove(match.Index, TextRemove.Length);
                }
            }

            return sourceText;
        }

        #endregion
    }
}
