using System;

namespace Episememe.Application.Exceptions
{
    public class MediaDoesNotBelongToUserException : Exception
    {
        public MediaDoesNotBelongToUserException(string message)
            : base(message) { }
    }
}
