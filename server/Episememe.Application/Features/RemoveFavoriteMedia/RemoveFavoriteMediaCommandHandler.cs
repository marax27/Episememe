using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var mediaInstance = await _context.MediaInstances
                .Include(mi => mi.FavoriteMedia)
                .SingleAsync(mi => mi.Id == request.MediaInstanceId, cancellationToken);

            var favoriteMedia = mediaInstance.FavoriteMedia
                .Single(fm => fm.UserId == request.UserId && fm.MediaInstanceId == mediaInstance.Id);

            mediaInstance.FavoriteMedia.Remove(favoriteMedia);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
