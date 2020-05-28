using Episememe.Application.DataTransfer;
using MediatR;
using System;
using System.Collections.Generic;

namespace Episememe.Application.Features.GetFavoriteMedia
{
    public class GetFavoriteMediaQuery : IRequest<IEnumerable<MediaInstanceDto>>
    {
        public string UserId { get; }

        private GetFavoriteMediaQuery(string userId)
        {
            UserId = userId;
        }

        public static GetFavoriteMediaQuery Create(string userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            return new GetFavoriteMediaQuery(userId);
        }
    }
}
