using System;

namespace Episememe.Application.Exceptions
{
    public class MediaDoesNotBelongToUser : Exception
    {
        public MediaDoesNotBelongToUser(string message)
            : base(message) { }
    }
}
