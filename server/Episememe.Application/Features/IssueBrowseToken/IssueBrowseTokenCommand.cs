using System;
using MediatR;

namespace Episememe.Application.Features.IssueBrowseToken
{
    public class IssueBrowseTokenCommand : IRequest
    {
        public string TokenValue { get; }
        public string? UserId { get; }

        private IssueBrowseTokenCommand(string token, string? userId)
        {
            TokenValue = token;
            UserId = userId;
        }

        public static IssueBrowseTokenCommand Create(string token, string? userId)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty.", nameof(token));
            }

            return new IssueBrowseTokenCommand(token, userId);
        }
    }
}
