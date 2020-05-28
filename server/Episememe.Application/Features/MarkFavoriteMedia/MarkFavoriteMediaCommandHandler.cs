using Episememe.Application.Interfaces;
using Episememe.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Episememe.Application.Features.MarkFavoriteMedia
{
    public class MarkFavoriteMediaCommandHandler : IRequestHandler<MarkFavoriteMediaCommand>
    {
        private readonly IWritableApplicationContext _context;

        public MarkFavoriteMediaCommandHandler(IWritableApplicationContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(MarkFavoriteMediaCommand request, CancellationToken cancellationToken)
        {
            if (await _context.FavoriteMedia.FindAsync(request.MediaInstanceId, request.UserId) != null)
                return Unit.Value;

            var newFavoriteMedia = new FavoriteMedia()
            {
                MediaInstanceId = request.MediaInstanceId,
                UserId = request.UserId
            };

            await _context.FavoriteMedia.AddAsync(newFavoriteMedia, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
