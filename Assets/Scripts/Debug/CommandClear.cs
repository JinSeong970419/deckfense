namespace Deckfense
{
    public class CommandClear : ConsoleCommand
    {
        public override string Name { get; protected set; }
        public override string Command { get; protected set; }
        public override string Description { get; protected set; }

        public CommandClear()
        {
            Name = "Clear";
            Command = "clear";
            Description = $"{Name} - console text Erases";

            AddCommandToConsole();
        }

        public override void RunCommand(string[] args)
        {
            PopupDebugConsole.Instance.ClearConsole();
        }

        public static CommandClear CreateCommand()
        {
            return new CommandClear();
        }
    }
}
