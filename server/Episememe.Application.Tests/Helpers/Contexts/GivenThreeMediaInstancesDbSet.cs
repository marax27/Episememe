using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Episememe.Application.Tests.Helpers.Contexts
{
    public class GivenThreeMediaInstancesDbSet : IMediaInstanceDbSetTestContext
    {
        public DbSet<MediaInstance> MediaInstances => CreateMediaInstancesDbSet();

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

            var tag1 = new Tag
            {
                Id = 1,
                Name = "usa"
            };
            var tag2 = new Tag
            {
                Id = 2,
                Name = "germany"
            };
            var tag3 = new Tag
            {
                Id = 3,
                Name = "sport"
            };
            var tag4 = new Tag
            {
                Id = 4,
                Name = "university"
            };

            var mediaTag1 = CreateMediaTag(mediaInstance1, tag1);
            var mediaTag2 = CreateMediaTag(mediaInstance1, tag2);
            var mediaTag3 = CreateMediaTag(mediaInstance2, tag2);
            var mediaTag4 = CreateMediaTag(mediaInstance2, tag3);
            var mediaTag5 = CreateMediaTag(mediaInstance3, tag4);

            mediaInstance1.MediaTags.Add(mediaTag1);
            mediaInstance1.MediaTags.Add(mediaTag2);
            mediaInstance2.MediaTags.Add(mediaTag3);
            mediaInstance2.MediaTags.Add(mediaTag4);
            mediaInstance3.MediaTags.Add(mediaTag5);
            tag1.MediaTags.Add(mediaTag1);
            tag2.MediaTags.Add(mediaTag2);
            tag2.MediaTags.Add(mediaTag3);
            tag3.MediaTags.Add(mediaTag4);
            tag4.MediaTags.Add(mediaTag5);

            var instances = new HashSet<MediaInstance>()
            {
                mediaInstance1,
                mediaInstance2,
                mediaInstance3
            };

            return DbSetMockFactory.Create(instances).Object;
        }

        private MediaTag CreateMediaTag(MediaInstance instance, Tag tag)
        {
            var mediaTag = new MediaTag
            {
                MediaInstance = instance, 
                MediaInstanceId = instance.Id, 
                Tag = tag, 
                TagId = tag.Id
            };

            return mediaTag;
        }
    }
}
