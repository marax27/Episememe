using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using MediatR;

namespace Episememe.Application.Features.IssueBrowseToken
{
    public class IssueBrowseTokenCommandHandler : IRequestHandler<IssueBrowseTokenCommand>
    {
        private readonly IAuthorizationContext _context;
        private readonly ITimeProvider _timeProvider;
        private readonly TimeSpan _timeToExpiration;

        public IssueBrowseTokenCommandHandler(IAuthorizationContext context, ITimeProvider timeProvider)
        {
            _context = context;
            _timeProvider = timeProvider;
            _timeToExpiration = TimeSpan.FromHours(2.0);
        }

        public async Task<Unit> Handle(IssueBrowseTokenCommand request, CancellationToken cancellationToken)
        {
            RemoveExpiredTokens();
            AddNewToken(request.TokenValue);
            await _context.SaveChangesAsync(CancellationToken.None);
            return Unit.Value;
        }

        private void RemoveExpiredTokens()
        {
            var expiredTokens = _context.BrowseTokens
                .Where(token => token.ExpirationTime <= _timeProvider.GetUtc());
            _context.BrowseTokens.RemoveRange(expiredTokens);
        }

        private void AddNewToken(string token)
        {
            var browseToken = new BrowseToken
            {
                Id = token,
                ExpirationTime = _timeProvider.GetUtc() + _timeToExpiration
            };
            _context.BrowseTokens.Add(browseToken);
        }
    }
}
