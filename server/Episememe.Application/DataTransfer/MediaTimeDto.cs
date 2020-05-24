using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class MediaTimeDto
    {
        public IEnumerable<IEnumerable<long>> Dates {get; set;} = null!; 
    }
}