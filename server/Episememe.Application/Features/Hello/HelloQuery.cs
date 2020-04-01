using MediatR;

namespace Episememe.Application.Features.Hello
{
    public class HelloQuery : IRequest<string>
    {
        public string UserId { get; }

        private HelloQuery(string userId)
            => UserId = userId;

        public static HelloQuery Create(string userId)
        {
            return new HelloQuery(userId);
        }
    }
}
