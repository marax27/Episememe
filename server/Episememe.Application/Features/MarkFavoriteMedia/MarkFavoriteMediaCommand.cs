using MediatR;
using System;

namespace Episememe.Application.Features.MarkFavoriteMedia
{
    public class MarkFavoriteMediaCommand : IRequest
    {
        public string MediaInstanceId { get; }
        public string UserId { get; }

        private MarkFavoriteMediaCommand(string mediaInstanceId, string userId)
        {
            MediaInstanceId = mediaInstanceId;
            UserId = userId;
        }

        public static MarkFavoriteMediaCommand Create(string mediaInstanceId, string userId)
        {
            if (mediaInstanceId == null)
                throw new ArgumentNullException(nameof(mediaInstanceId));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return new MarkFavoriteMediaCommand(mediaInstanceId, userId);
        }
    }
}
