using Episememe.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Episememe.Application.Tests.Helpers.Contexts.Filtering
{
    public class FavoriteMediaTestsDbSet : IDbSetTestContext<MediaInstance>
    {
        public DbSet<MediaInstance> Instances => CreateMediaInstancesDbSet();

        private DbSet<MediaInstance> CreateMediaInstancesDbSet()
        {
            var user1 = "user1";
            var user2 = "user2";

            var mediaInstance1 = new MediaInstance
            {
                Id = "1"
            };
            var mediaInstance2 = new MediaInstance
            {
                Id = "2"
            };
            var mediaInstance3 = new MediaInstance
            {
                Id = "3"
            };

            var favoriteMedia1 = CreateFavoriteMedia(user1, mediaInstance1);
            var favoriteMedia2 = CreateFavoriteMedia(user1, mediaInstance2);
            var favoriteMedia3 = CreateFavoriteMedia(user2, mediaInstance2);
            var favoriteMedia4 = CreateFavoriteMedia(user2, mediaInstance3);

            mediaInstance1.FavoriteMedia.Add(favoriteMedia1);
            mediaInstance2.FavoriteMedia.Add(favoriteMedia2);
            mediaInstance2.FavoriteMedia.Add(favoriteMedia3);
            mediaInstance3.FavoriteMedia.Add(favoriteMedia4);

            var instances = new HashSet<MediaInstance>()
            {
                mediaInstance1,
                mediaInstance2,
                mediaInstance3
            };

            return DbSetMockFactory.Create(instances).Object;
        }

        private FavoriteMedia CreateFavoriteMedia(string userId, MediaInstance mediaInstance)
        {
            var newFavoriteMedia = new FavoriteMedia
            {
                UserId = userId,
                MediaInstance = mediaInstance,
                MediaInstanceId = mediaInstance.Id
            };

            return newFavoriteMedia;
        }
    }
}
