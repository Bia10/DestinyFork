using Destiny.Data;

namespace Destiny.Maple
{
    public sealed class Variable
    {
        public string Key { get; }
        public string Value { get; }

        public Variable(string key, object value)
        {
            Key = key;
            Value = value.ToString();
        }

        public Variable(Datum datum)
        {
            Key = (string)datum["key"];
            Value = (string)datum["Value"];
        }
    }
}
