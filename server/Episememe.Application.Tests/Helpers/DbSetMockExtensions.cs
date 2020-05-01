using System.Collections.Generic;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Episememe.Application.Tests.Helpers
{
    public static class DbSetMockExtensions
    {
        public static void StoreAddedEntitiesIn<T>(this Mock<DbSet<T>> mock, ISet<T> addedEntities)
            where T : class
        {
            mock.Setup(x => x.Add(It.IsAny<T>()))
                .Callback((T entity) => addedEntities.Add(entity));
            mock.Setup(x => x.AddRange(It.IsAny<IEnumerable<T>>()))
                .Callback((IEnumerable<T> entities) =>
                {
                    foreach (var entity in entities)
                        addedEntities.Add(entity);
                });

            mock.Setup(x => x.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Callback((T entity, CancellationToken _) => addedEntities.Add(entity));
            mock.Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<T>>(), It.IsAny<CancellationToken>()))
                .Callback((IEnumerable<T> entities, CancellationToken _) =>
                {
                    foreach (var entity in entities)
                        addedEntities.Add(entity);
                });
        }

        public static void StoreRemovedEntitiesIn<T>(this Mock<DbSet<T>> mock, ISet<T> removedEntities)
            where T : class
        {
            mock.Setup(x => x.Remove(It.IsAny<T>()))
                .Callback((T entity) => removedEntities.Add(entity));
            mock.Setup(x => x.RemoveRange(It.IsAny<IEnumerable<T>>()))
                .Callback((IEnumerable<T> entities) =>
                {
                    foreach (var entity in entities)
                        removedEntities.Add(entity);
                });
        }
    }
}
