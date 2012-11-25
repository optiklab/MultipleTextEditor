using System.Collections.Generic;
using MultipleTextEditor.Commands;

namespace MultipleTextEditor
{
    internal sealed class CommandsCollection
    {
        #region Constructor

        public CommandsCollection()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Method return command if it is exist in the current collection.
        /// Otherwise return null.
        /// </summary>
        /// <param name="Number">Number in the current collection of commands</param>
        /// <returns>command or null</returns>
        public Command GetCommand(int Number)
        {
            if (Number >= 0 && Number < _commands.Count)
                return _commands[Number];
            else
                return null;
        }

        public int GetCount()
        {
            return _commands.Count;
        }

        public void UpdateCollection(CommandType type)
        {
            _commands.Clear();
            switch (type)
            {
                case CommandType.Add:
                    _commands.Add(new AddBeforeCommand());
                    _commands.Add(new AddAfterCommand());
                    _commands.Add(new AddBetweenCommand());
                    _commands.Add(new AddAtEndCommand());
                    _commands.Add(new AddAtStartCommand());
                    break;
                case CommandType.Remove:
                    _commands.Add(new RemoveBeforeCommand());
                    _commands.Add(new RemoveAfterCommand());
                    _commands.Add(new RemoveBetweenCommand());
                    _commands.Add(new RemoveAtEndCommand());
                    _commands.Add(new RemoveAtStartCommand());
                    _commands.Add(new RemoveThatCommand());
                    break;
                case CommandType.Replace:
                    _commands.Add(new ReplaceBetweenCommand());
                    _commands.Add(new ReplaceThatCommand());
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Private fields

        private List<Command> _commands = new List<Command>();

        #endregion

    }
}
