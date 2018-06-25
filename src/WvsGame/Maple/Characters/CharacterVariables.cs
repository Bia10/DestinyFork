using System.Collections.ObjectModel;

using Destiny.Data;

namespace Destiny.Maple.Characters
{
    public sealed class CharacterVariables : KeyedCollection<string, Variable>
    {
        public Character Parent { get; private set; }

        public CharacterVariables(Character parent)
        {
            Parent = parent;
        }

        public void Load()
        {
            foreach (Datum datum in new Datums("variables").Populate("CharacterID = {0}", Parent.ID))
            {
                Add(new Variable(datum));
            }
        }

        public void Save()
        {
            Database.Delete("variables", "CharacterID = {0}", Parent.ID);

            foreach (Variable variable in this)
            {
                Datum datum = new Datum("variables")
                {
                    ["CharacterID"] = Parent.ID,
                    ["Key"] = variable.Key,
                    ["Value"] = variable.Value
                };

                datum.Insert();
            }
        }

        protected override string GetKeyForItem(Variable item)
        {
            return item.Key;
        }
    }
}
