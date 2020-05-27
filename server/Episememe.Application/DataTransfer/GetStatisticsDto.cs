using System.Collections.Generic;

namespace Episememe.Application.DataTransfer
{
    public class GetStatisticsDto
    {
        public IEnumerable<IEnumerable<long>> Data {get; set;} = null!; 
    }
}