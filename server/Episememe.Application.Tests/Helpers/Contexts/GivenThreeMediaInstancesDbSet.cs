using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Episememe.Application.Tests.Helpers.Contexts
{
    public class GivenThreeMediaInstancesDbSet : IMediaInstanceDbSetTestContext
    {
        public DbSet<MediaInstance> MediaInstances { get => GetMediaInstancesDbSet(); }

        private DbSet<MediaInstance> GetMediaInstancesDbSet()
        {
            var mediaInstance1 = new MediaInstance
            {
                Id = "1",
                RevisionCount = 0,
                Timestamp = DateTime.Today.AddYears(-1)
            };
            var mediaInstance2 = new MediaInstance
            {
                Id = "2",
                RevisionCount = 0,
                Timestamp = DateTime.Today.AddYears(-2)
            };
            var mediaInstance3 = new MediaInstance
            {
                Id = "3",
                RevisionCount = 0,
                Timestamp = DateTime.Today.AddYears(-1).AddMonths(-6)
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

            var mediaTag1 = GetMediaTag(mediaInstance1, tag1);
            var mediaTag2 = GetMediaTag(mediaInstance1, tag2);
            var mediaTag3 = GetMediaTag(mediaInstance2, tag2);
            var mediaTag4 = GetMediaTag(mediaInstance2, tag3);
            var mediaTag5 = GetMediaTag(mediaInstance3, tag4);

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

        private MediaTag GetMediaTag(MediaInstance instance, Tag tag)
        {
            var mediaTag = new MediaTag();
            mediaTag.MediaInstance = instance;
            mediaTag.MediaInstanceId = instance.Id;
            mediaTag.Tag = tag;
            mediaTag.TagId = tag.Id;

            return mediaTag;
        }
    }
}
