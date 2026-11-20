using System;

namespace Utac.Rnc2.Infrastructure.CustomExceptions
{
    public class InvalidAddressException : ApplicationException
    {

        public InvalidAddressException()
            : base()
        {
        }

        public InvalidAddressException(string message)
            : base(message)
        {
        }

        public InvalidAddressException(string message, Exception innner)
            : base(message, innner)
        {
        }
    }
}
