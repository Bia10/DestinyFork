using Destiny.Maple.Characters;
using Destiny.Maple.Data;

namespace Destiny.Maple.Commands.Implementation
{
    public sealed class ItemCommand : Command
    {
        public override string Name
        {
            get
            {
                return "item";
            }
        }

        public override string Parameters
        {
            get
            {
                return "{ id } [ quantity ]";
            }
        }

        public override bool IsRestricted
        {
            get
            {
                return true;
            }
        }

        public override void Execute(Character caller, string[] args)
        {
            if (args.Length < 1)
            {
                ShowSyntax(caller);
            }
            else
            {
                short quantity = 0;

                if (args.Length > 1)
                {
                    short.TryParse(args[args.Length - 1], out quantity);
                }

                if (quantity < 1)
                {
                    quantity = 1;
                }

                int itemID = int.Parse(args[0]);

                if (DataProvider.Items.Contains(itemID))
                {
                    caller.Items.AddItemToInventory(new Item(itemID, quantity));
                }
                else
                {
                    Character.Notify(caller, "[Command] Invalid item.");
                }
            }
        }
    }
}