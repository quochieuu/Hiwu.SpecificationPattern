using System.Globalization;

namespace Hiwu.SpecificationPattern.Application.Exceptions
{
    public class UnprocessableException : Exception
    {
        public UnprocessableException() : base()
        {
        }

        public UnprocessableException(string message) : base(message)
        {
        }

        public UnprocessableException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
