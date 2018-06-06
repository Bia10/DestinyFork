using System;

namespace Destiny.Maple
{
    public class NoGameAccountException : Exception
    {
        public override string Message
        {
            get
            {
                return "The specified account does not exist.";
            }
        }
    }
}