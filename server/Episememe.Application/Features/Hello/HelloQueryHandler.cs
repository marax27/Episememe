using MediatR;

namespace Episememe.Application.Features.Hello
{
    class HelloQueryHandler : RequestHandler<HelloQuery, string>
    {
        protected override string Handle(HelloQuery request)
        {
            var userId = request.UserId;
            return string.IsNullOrEmpty(userId) ? "Who are you?" : $"Hello, {userId}!";
        }
    }
}
