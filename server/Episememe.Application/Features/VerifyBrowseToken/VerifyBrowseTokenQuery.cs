using System;
using MediatR;

namespace Episememe.Application.Features.VerifyBrowseToken
{
    public class VerifyBrowseTokenQuery : IRequest<bool>
    {
        public string TokenValue { get; }

        private VerifyBrowseTokenQuery(string token)
            => TokenValue = token;

        public static VerifyBrowseTokenQuery Create(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Token is null or empty.", nameof(token));

            return new VerifyBrowseTokenQuery(token);
        }
    }
}
