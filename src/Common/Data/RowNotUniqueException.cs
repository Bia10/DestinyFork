using System.Data;

namespace Destiny.Data
{
    public class RowNotUniqueException : DataException
    {
        public override string Message
        {
            get
            {
                return "Obtained row is not unique.";
            }
        }
    }
}