using System;

namespace Episememe.Application.TagGraph
{
    public class CycleException : Exception
    {
        public CycleException(string message)
            : base(message) { }
    }
}
