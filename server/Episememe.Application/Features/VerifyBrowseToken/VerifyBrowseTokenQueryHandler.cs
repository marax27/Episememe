using System.Linq;
using Episememe.Application.Interfaces;
using MediatR;

namespace Episememe.Application.Features.VerifyBrowseToken
{
    public class VerifyBrowseTokenQueryHandler : RequestHandler<VerifyBrowseTokenQuery, bool>
    {
        private readonly IAuthorizationContext _context;
        private readonly ITimeProvider _timeProvider;

        public VerifyBrowseTokenQueryHandler(IAuthorizationContext context, ITimeProvider timeProvider)
        {
            _context = context;
            _timeProvider = timeProvider;
        }

        protected override bool Handle(VerifyBrowseTokenQuery request)
        {
            var timeNow = _timeProvider.GetUtc();
            return _context.BrowseTokens
                .Any(token => token.Id == request.TokenValue && token.ExpirationTime >= timeNow);
        }
    }
}
