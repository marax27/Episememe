using MediatR;

namespace Episememe.Application.Features.GenerateBrowseToken
{
    public class GenerateBrowseTokenQuery : IRequest<string>
    {
        public static GenerateBrowseTokenQuery Create()
            => new GenerateBrowseTokenQuery();
    }
}
