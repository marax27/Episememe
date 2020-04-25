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
    }
}
