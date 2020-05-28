using Episememe.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Episememe.Application.Features.RemoveFavoriteMedia
{
    public class RemoveFavoriteMediaCommandHandler : IRequestHandler<RemoveFavoriteMediaCommand>
    {
        private readonly IWritableApplicationContext _context;

        public RemoveFavoriteMediaCommandHandler(IWritableApplicationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(RemoveFavoriteMediaCommand request, CancellationToken cancellationToken)
        {
            var favoriteMedia = await _context.FavoriteMedia.FindAsync(request.MediaInstanceId, request.UserId);

            if (favoriteMedia != null)
            {
                _context.FavoriteMedia.Remove(favoriteMedia);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
