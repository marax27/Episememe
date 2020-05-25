using Episememe.Application.DataTransfer;
using Episememe.Application.Interfaces;
using MediatR;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Episememe.Application.Features.GetStatistic
{
    public class GetStatisticsQueryHandler : RequestHandler<GetStatisticsQuery, GetStatisticsDto>
    {
        private readonly IApplicationContext _context;
        private readonly ITimeProvider _timeProvider;
        public GetStatisticsQueryHandler(IApplicationContext context, ITimeProvider timeProvider)
        {
            _context = context;
            _timeProvider = timeProvider;
        }
        protected override GetStatisticsDto Handle(GetStatisticsQuery request)
        {
            var dates = _context.MediaInstances.Select(x => x.Timestamp).ToList();
            if (!dates.Any())
            {
                long convertedDate = (long) (_timeProvider.GetUtc().Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                return new GetStatisticsDto {Data = new List<List<long>> {new List<long> {convertedDate, (long) 0}}};
            }
            var start = dates.Min();
            var end = _timeProvider.GetUtc();
            var nbOfDailyInstances = dates.GroupBy(x => x.Date)
                .Select(x => new 
                {
                    Value = x.Count(),
                    Day = (DateTime) x.Key
                }).ToList();
            List<List<long>> statistics = new List<List<long>>();
            long count = 0;
            for(DateTime date = start; date.Date <= end.Date; date = date.AddDays(1))
            {
                var instance = nbOfDailyInstances.FirstOrDefault(x => x.Day == date.Date);
                if (instance != null) count += instance.Value;
                long convertedDate = (long) (date.Date - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                statistics.Add(new List<long> {convertedDate, count});
            }
            return new GetStatisticsDto {Data = statistics};
        }
    }
}