using System;

namespace Episememe.Application.Graphs.Exceptions
{
    public class CycleException : Exception
    {
        public CycleException(string message)
            : base(message) { }
    }
}
