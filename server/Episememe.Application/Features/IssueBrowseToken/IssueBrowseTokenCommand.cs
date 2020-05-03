using System;
using MediatR;

namespace Episememe.Application.Features.IssueBrowseToken
{
    public class IssueBrowseTokenCommand : IRequest
    {
        public string TokenValue { get; }

        private IssueBrowseTokenCommand(string token)
            => TokenValue = token;

        public static IssueBrowseTokenCommand Create(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token is null or empty.", nameof(token));
            }

            return new IssueBrowseTokenCommand(token);
        }
    }
}
