using System;
using Episememe.Application.Interfaces;

namespace Episememe.Infrastructure.Time
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetUtc()
            => DateTime.UtcNow;
    }
}
