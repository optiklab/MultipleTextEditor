using System.Text.RegularExpressions;

namespace MultipleTextEditor.Commands
{
    internal sealed class RemoveBetweenCommand : Command
    {
        #region Constructor

        public RemoveBetweenCommand()
            : base((string)App.Current.FindResource("RemoveBetweenCommandName"),
                    (string)App.Current.FindResource("RemoveBetweenCommandLabelFirst"),
                    (string)App.Current.FindResource("RemoveBetweenCommandLabelSecond"),
                    (string)App.Current.FindResource("RemoveBetweenCommandNameDescription"),
                    true, true, false)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.FirstBorder) || string.IsNullOrEmpty(parameters.SecondBorder))
            {
                return sourceText;
            }

            int LastFromIndex = 0;
            int LastToIndex = 0;
            int NextIndex = 0;
            string From = parameters.FirstBorder;
            string To = parameters.SecondBorder;
            int FromLength = From.Length;
            int ToLength = To.Length;
            bool IsChanged = false;

            do
            {
                Match matchFrom = FindNextMatch(From, sourceText, NextIndex, parameters);

                if (matchFrom.Success)
                    LastFromIndex = matchFrom.Index;
                else
                    break;

                Match matchTo = FindNextMatch(To, sourceText, LastFromIndex + FromLength, parameters);

                if (matchTo.Success)
                    LastToIndex = matchTo.Index;
                else
                    break;

                // Is need to include parameters to remove
                if (parameters.IncludeParameters == true)
                {
                    sourceText = RemoveSubstring(sourceText, LastFromIndex, LastToIndex + ToLength);
                    NextIndex = LastFromIndex;
                }
                else
                {
                    sourceText = RemoveSubstring(sourceText, LastFromIndex + FromLength, LastToIndex);
                    NextIndex = LastFromIndex + FromLength + ToLength;
                }

                IsChanged = true;
            } while (true);

            if (IsChanged == false)
                throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));

            return sourceText;
        }

        #endregion

        #region Private method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        private Match FindNextMatch(string search, string source, int start, CommandParameters parameters)
        {
            string Text = Regex.Escape(search);
            Regex regToText;
            // Case check
            if (parameters.IsCaseSensitive == false)
                regToText = new Regex(Text, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            else
                regToText = new Regex(Text, RegexOptions.Multiline);
            // Find first occurrence of To in sourceText.
            Match match = regToText.Match(source, start);
            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private string RemoveSubstring(string source, int start, int end)
        {
            if (start >= 0 && end > 0 && start < source.Length && end <= source.Length)
                source = source.Remove(start, end - start);
            return source;
        }

        #endregion
    }
}
