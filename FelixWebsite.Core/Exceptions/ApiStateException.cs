using System;

namespace FelixWebsite.Core.Exceptions
{
    public class ApiStateException:Exception
    {
        public ApiStateException()
        {
            
        }

        public ApiStateException(string message):base(message)
        {
            
        }

        public ApiStateException(string message, Exception inner):base(message,inner)
        {
            
        }
    }
}
