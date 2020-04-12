using System;

namespace Episememe.Application.Exceptions
{
    public class FileExistsException : Exception
    {
        public FileExistsException(string message)
            : base(message) { }
    }
}
