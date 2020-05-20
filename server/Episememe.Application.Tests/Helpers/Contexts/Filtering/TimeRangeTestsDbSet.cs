using System;
using System.Collections.Generic;
using System.Text;
using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Episememe.Application.Tests.Helpers.Contexts.Filtering
{
    public class TimeRangeTestsDbSet : IDbSetTestContext<MediaInstance>
    {
        public DbSet<MediaInstance> Instances => CreateMediaInstancesDbSet();

        private DbSet<MediaInstance> CreateMediaInstancesDbSet()
        {
            var mediaInstance1 = new MediaInstance
            {
                Id = "1",
                Timestamp = new DateTime(2009, 1, 1, 0, 0, 0)
            };
            var mediaInstance2 = new MediaInstance
            {
                Id = "2",
                Timestamp = new DateTime(2008, 1, 1, 0, 0, 0)
            };
            var mediaInstance3 = new MediaInstance
            {
                Id = "3",
                Timestamp = new DateTime(2008, 6, 1, 0, 0, 0)
            };

            var instances = new HashSet<MediaInstance>()
            {
                mediaInstance1,
                mediaInstance2,
                mediaInstance3
            };

            return DbSetMockFactory.Create(instances).Object;
        }
    }
}
