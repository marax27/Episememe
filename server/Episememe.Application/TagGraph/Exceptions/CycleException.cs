using System;

namespace Episememe.Application.TagGraph.Exceptions
{
    public class CycleException : Exception
    {
        public CycleException(string message)
            : base(message) { }
    }
}
