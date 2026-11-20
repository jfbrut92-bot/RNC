using System;

namespace Utac.Rnc2.Infrastructure.CustomExceptions
{
    public class ValueObjectValidationException : ApplicationException
    {

        public ValueObjectValidationException()
            : base()
        {
        }

        public ValueObjectValidationException(string message)
            : base(message)
        {
        }

        public ValueObjectValidationException(string message, Exception innner)
            : base(message, innner)
        {
        }

    }
}
