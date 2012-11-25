using System.Text.RegularExpressions;
using System;

namespace MultipleTextEditor.Commands
{
    internal sealed class ReplaceBetweenCommand : Command
    {
        #region Constructor

        public ReplaceBetweenCommand()
            : base((string)App.Current.FindResource("ReplaceBetweenCommandName"),
                    (string)App.Current.FindResource("ReplaceBetweenCommandLabelFirst"),
                    (string)App.Current.FindResource("ReplaceBetweenCommandLabelSecond"),
                    (string)App.Current.FindResource("ReplaceBetweenCommandNameDescription"), 
                    true, true, true)
        { }

        #endregion

        #region Public method

        public override string Execute(string sourceText, CommandParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.FirstBorder) || string.IsNullOrEmpty(parameters.SecondBorder))
            {
                throw new SimpleEditException((string)App.Current.FindResource("ErrorCommandParametersEmpty"));
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
                // FROM
                Match matchFrom = FindNextMatch(From, sourceText, NextIndex, parameters);

                if (matchFrom.Success)
                    LastFromIndex = matchFrom.Index;
                else
                    break;

                // TO
                Match matchTo = FindNextMatch(To, sourceText, LastFromIndex + FromLength, parameters);

                if (matchTo.Success)
                    LastToIndex = matchTo.Index;
                else
                    break;

                // Is need to include parameters to replace
                if (parameters.IncludeParameters == true)
                {
                    sourceText = ReplaceSubstring(sourceText, parameters.TextToAppend, LastFromIndex, LastToIndex + ToLength);
                    NextIndex = LastFromIndex + parameters.TextToAppend.Length;
                }
                else
                {
                    sourceText = ReplaceSubstring(sourceText, parameters.TextToAppend, LastFromIndex + FromLength, LastToIndex);
                    NextIndex = LastFromIndex + parameters.TextToAppend.Length + FromLength + ToLength;
                }
                IsChanged = true;
                _loopsCounter++;

                if (_loopsCounter > 1000000 && !IsChanged)
                {
                    throw new SimpleEditException((string)App.Current.FindResource("TextNotFoundError"));
                }

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
        private string ReplaceSubstring(string source, string append, int start, int end)
        {
            if (start >= 0 && end > 0 && start < source.Length && end <= source.Length)
            {
                source = source.Remove(start, end - start);
                source = source.Insert(start, append);
            }
            return source;
        }

        #endregion

        #region Private fields

        private int _loopsCounter = 0;

        #endregion
    }
}
