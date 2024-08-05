using System.Globalization;

namespace Hiwu.SpecificationPattern.Application.Exceptions
{
    public class TooManyRequestException : Exception
    {
        public TooManyRequestException() : base()
        {
        }

        public TooManyRequestException(string message) : base(message)
        {
        }

        public TooManyRequestException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
