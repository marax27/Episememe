using System;
using System.Collections.Generic;
using System.Linq;
using Episememe.Domain.Entities;
using Episememe.Domain.HelperEntities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Episememe.Application.Tests.Helpers
{
    public static class DbSetMockFactory
    {
        public static Mock<DbSet<T>> Create<T>(IEnumerable<T> elements) where T : class
        {
            var elementsAsQueryable = elements.AsQueryable();
            var result = new Mock<DbSet<T>>();

            result.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            result.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            result.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            result.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return result;
        }

        public static DbSet<MediaInstance> SampleMediaInstancesDbSet()
        {
            var mediaInstance1 = new MediaInstance();
            mediaInstance1.Id = "1";
            mediaInstance1.RevisionCount = 0;
            mediaInstance1.Timestamp = DateTime.Today.AddYears(-1);
            var mediaInstance2 = new MediaInstance();
            mediaInstance2.Id = "2";
            mediaInstance2.RevisionCount = 0;
            mediaInstance2.Timestamp = DateTime.Today.AddYears(-2);
            var mediaInstance3 = new MediaInstance();
            mediaInstance3.Id = "3";
            mediaInstance2.RevisionCount = 0;
            mediaInstance3.Timestamp = DateTime.Today.AddYears(-1).AddMonths(-6);

            var tag1 = new Tag();
            tag1.Id = 1;
            tag1.Name = "usa";
            var tag2 = new Tag();
            tag2.Id = 2;
            tag2.Name = "germany";
            var tag3 = new Tag();
            tag3.Id = 3;
            tag3.Name = "sport";
            var tag4 = new Tag();
            tag4.Id = 4;
            tag4.Name = "university";

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

            var instances = new List<MediaInstance>()
            {
                mediaInstance1,
                mediaInstance2,
                mediaInstance3
            };

            return DbSetMockFactory.Create(instances).Object;
        }

        private static MediaTag GetMediaTag(MediaInstance instance, Tag tag)
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
