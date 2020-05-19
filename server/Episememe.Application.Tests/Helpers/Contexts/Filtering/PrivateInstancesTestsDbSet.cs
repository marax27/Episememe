using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Episememe.Application.Tests.Helpers.Contexts.Filtering
{
    public class PrivateInstancesTestsDbSet : IDbSetTestContext<MediaInstance>
    {
        public DbSet<MediaInstance> Instances => CreateMediaInstancesDbSet();

        private DbSet<MediaInstance> CreateMediaInstancesDbSet()
        {
            var user1 = "user1";
            var user2 = "user2";

            var mediaInstance1 = new MediaInstance
            {
                Id = "1",
                IsPrivate = true,
                AuthorId = user1
            };
            var mediaInstance2 = new MediaInstance
            {
                Id = "2",
                IsPrivate = false,
                AuthorId = user1
            };
            var mediaInstance3 = new MediaInstance
            {
                Id = "3",
                IsPrivate = true,
                AuthorId = user2
            };
            var mediaInstance4 = new MediaInstance
            {
                Id = "4",
                IsPrivate = false,
                AuthorId = user2
            };
            var mediaInstance5 = new MediaInstance
            {
                Id = "5",
                IsPrivate = false
            };

            var instances = new HashSet<MediaInstance>()
            {
                mediaInstance1,
                mediaInstance2,
                mediaInstance3,
                mediaInstance4,
                mediaInstance5
            };

            return DbSetMockFactory.Create(instances).Object;
        }
    }
}
