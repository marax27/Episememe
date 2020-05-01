using System;

namespace Episememe.Application.Interfaces
{
    public interface ITimeProvider
    {
        DateTime GetUtc();
    }
}
