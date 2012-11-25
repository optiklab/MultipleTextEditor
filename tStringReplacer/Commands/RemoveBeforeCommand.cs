using System;
using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveBeforeCommand : Command
    {
        #region Constructor

        public RemoveBeforeCommand()
            : base((string)App.Current.FindResource("RemoveBeforeCommandName"),
                    (string)App.Current.FindResource("RemoveBeforeCommandLabelFirst"),
                    (string)App.Current.FindResource("RemoveBeforeCommandLabelSecond"),
                    (string)App.Current.FindResource("RemoveBeforeCommandNameDescription"),
                    true, true, false)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (parameters.SecondBorder == string.Empty || parameters.FirstBorder == string.Empty)
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));

            // Escape from special symbols.
            string TextRemoveBefore = Regex.Escape(parameters.FirstBorder);
            Regex regText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regText = new Regex(TextRemoveBefore, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regText = new Regex(TextRemoveBefore, RegexOptions.Multiline);

            string TextRemove = parameters.SecondBorder;

            if (TextRemove == (string)App.Current.FindResource("TagAllSmall") ||
                TextRemove == (string)App.Current.FindResource("TagAllBig") ||
                TextRemove == (string)App.Current.FindResource("TagAllMiddle"))
            {
                // Find first occurrence of text and remove all from the beginning to its index.
                Match match = regText.Match(sourceText);
                if (match.Success == true)
                {
                    int Index = match.Index;
                    sourceText = sourceText.Remove(BEGIN, Index);
                }
                else
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
            }
            else
            {
                // Generate Match collection: find all occurrences of TextToReplase in sourceText.
                MatchCollection mc = regText.Matches(sourceText);
                int RemoveLength = 0;

                if (mc.Count > 0)
                {
                    bool IsNumeric = true;
                    try
                    {
                        // Get count of symbols to remove.
                        RemoveLength = Convert.ToInt32(TextRemove);
                        IsNumeric = true;
                    }
                    catch (Exception)
                    {
                        // Can't convert to digit
                        IsNumeric = false;
                    }

                    if( IsNumeric == true )
                        sourceText = RemoveSymbolsBeforeAllTexts(sourceText, mc, RemoveLength);
                    else
                        sourceText = RemoveStringBeforeFirstText(sourceText, mc[0], TextRemove, parameters);
                }
            }

            return sourceText;
        }

        #endregion

        #region Private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private string RemoveSymbolsBeforeAllTexts(string source, MatchCollection mc, int count)
        {
            // Remove text in all occurrences.
            int Index = 0;
            int IndexAdd = 0; //every time remove text, indexes changes
            // Remove RemoveLength symbols before every occurrence of TextRemoveAfter
            foreach (Match match in mc)
            {
                Index = match.Index + IndexAdd;
                source = source.Remove(Index - count, count);
                IndexAdd -= count;
            }

            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToRemove"></param>
        /// <returns></returns>
        private string RemoveStringBeforeFirstText(string source, Match TextRemoveBeforeMatch, string textRemove, CommandParameters parameters)
        {
            // Find first occurrence of the textRemove before TextRemoveBeforeMatch.
            string TextToRemove = Regex.Escape(textRemove);
            Regex regSubText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regSubText = new Regex(TextToRemove, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regSubText = new Regex(TextToRemove, RegexOptions.Multiline);

            MatchCollection matches = regSubText.Matches(source, BEGIN);
            if (matches.Count > 0)
            {
                // Find the nearest occurrence of TextRemove to first occurrence of TextRemoveAfter.
                int mindif = TextRemoveBeforeMatch.Index;
                Match minmatch = matches[0];
                bool IsFound = false;
                foreach (Match match in matches)
                {
                    if (match.Index < TextRemoveBeforeMatch.Index)
                    {
                        int dif = TextRemoveBeforeMatch.Index - match.Index;
                        if (dif <= mindif)
                        {
                            mindif = dif;
                            minmatch = match;
                            IsFound = true;
                        }
                    }
                }
                // Before TextRemoveBeforeMatch we found textRemove occurrence, and remove it
                if (IsFound == true)
                {
                    source = source.Remove(minmatch.Index, textRemove.Length);
                }
                else
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
            }      
            else
                throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
            return source;
        }

        #endregion

        #region Private field

        private const int BEGIN = 0;

        #endregion
    }
}
