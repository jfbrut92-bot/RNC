using System;

namespace Utac.Rnc2.Infrastructure.CustomExceptions
{
    public class ConcurrencyException : ApplicationException
    {

        public ConcurrencyException()
            : base()
        {
        }

        public ConcurrencyException(string message)
            : base(message)
        {
        }

        public ConcurrencyException(string message, Exception innner)
            : base(message, innner)
        {
        }

    }
}
