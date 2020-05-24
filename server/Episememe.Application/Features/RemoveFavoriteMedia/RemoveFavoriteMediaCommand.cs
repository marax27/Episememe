using MediatR;
using System;

namespace Episememe.Application.Features.RemoveFavoriteMedia
{
    public class RemoveFavoriteMediaCommand : IRequest
    {
        public string MediaInstanceId { get; set; }
        public string UserId { get; set; }

        private RemoveFavoriteMediaCommand(string mediaInstanceId, string userId)
        {
            MediaInstanceId = mediaInstanceId;
            UserId = userId;
        }

        public static RemoveFavoriteMediaCommand Create(string mediaInstanceId, string userId)
        {
            if (mediaInstanceId == null)
                throw new ArgumentNullException(nameof(mediaInstanceId));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return new RemoveFavoriteMediaCommand(mediaInstanceId, userId);
        }
    }
}
