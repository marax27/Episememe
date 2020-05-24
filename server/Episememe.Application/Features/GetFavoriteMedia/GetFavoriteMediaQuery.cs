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
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return new GetFavoriteMediaQuery(userId);
        }
    }
}
