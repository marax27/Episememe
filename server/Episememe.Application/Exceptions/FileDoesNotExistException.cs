using System;

namespace Episememe.Application.Exceptions
{
    public class FileDoesNotExistException : Exception
    {
        public FileDoesNotExistException(string message)
            : base(message) { }
    }
}
