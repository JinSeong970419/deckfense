using UnityEngine;

namespace Deckfense
{
    public abstract class ConsoleCommand
    {
        public abstract string Name { get; protected set; }
        public abstract string Command { get; protected set; }
        public abstract string Description { get; protected set; }

        public void AddCommandToConsole()
        {
            string addMessage = "Add Command";

            PopupDebugConsole.AddCommandsToConsole(Command, this);
            Debug.Log(Name + addMessage);
        }

        public abstract void RunCommand(string[] args);
    }
}
