using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.Interfaces
{
    public interface IMediaIdProvider
    {
        string GenerateUniqueBase32Id(IReadOnlyCollection<string> existingIds);
    }
}
