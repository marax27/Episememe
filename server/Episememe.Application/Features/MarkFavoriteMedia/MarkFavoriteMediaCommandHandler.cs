using Episememe.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Episememe.Domain.Entities;

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
            var mediaInstance = await _context.MediaInstances
                .Include(mi => mi.FavoriteMedia)
                .SingleAsync(mi => mi.Id == request.MediaInstanceId, cancellationToken);

            var newFavoriteMedia = new FavoriteMedia()
            {
                MediaInstance = mediaInstance,
                UserId = request.UserId
            };

            mediaInstance.FavoriteMedia.Add(newFavoriteMedia);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
