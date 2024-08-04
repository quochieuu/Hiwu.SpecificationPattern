using System.Globalization;

namespace Hiwu.SpecificationPattern.Core.Exceptions
{
    public class AccountException : Exception
    {
        public AccountException() : base()
        {
        }

        public AccountException(string message) : base(message)
        {
        }

        public AccountException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
