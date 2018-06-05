using System;
using System.Collections.ObjectModel;

namespace Destiny.Maple
{
    public sealed class Migrations : KeyedCollection<string, Migration>
    {
        public int Validate(string host, int characterID)
        {
            foreach (Migration migration in this)
            {
                if (migration.Host != host || migration.CharacterID != characterID) continue;

                Remove(migration);

                return (DateTime.Now - migration.Expiry).TotalSeconds > 30 ? 0 : migration.AccountID;
            }

            return 0;
        }

        protected override string GetKeyForItem(Migration item)
        {
            return item.Host;
        }
    }
}
