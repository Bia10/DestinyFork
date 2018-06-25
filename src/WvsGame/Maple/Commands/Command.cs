using Destiny.Maple.Characters;

namespace Destiny.Maple.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract string Parameters { get; }
        public abstract bool IsRestricted { get; }

        public abstract void Execute(Character caller, string[] args);

        protected static string CombineArgs(string[] args, int start = 0)
        {
            string result = string.Empty;

            for (int i = start; i < args.Length; i++)
            {
                result += args[i] + ' ';
            }

            return result.Trim();
        }

        public string CombineArgs(string[] args, int start, int length)
        {
            string result = string.Empty;

            for (int i = start; i < length; i++)
            {
                result += args[i] + ' ';
            }

            return result.Trim();
        }

        protected void ShowSyntax(Character caller)
        {
            Character.Notify(caller, string.Format("[Syntax] {0}{1} {2}", Application.CommandIndicator, Name, Parameters.ClearFormatters()));
        }
    }
}
