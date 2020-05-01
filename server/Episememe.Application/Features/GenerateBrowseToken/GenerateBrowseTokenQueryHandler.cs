using System;
using System.Security.Cryptography;
using MediatR;

namespace Episememe.Application.Features.GenerateBrowseToken
{
    public class GenerateBrowseTokenQueryHandler : RequestHandler<GenerateBrowseTokenQuery, string>
    {
        protected override string Handle(GenerateBrowseTokenQuery request)
        {
            using var provider = new RNGCryptoServiceProvider();
            var bytes = new byte[16];
            provider.GetBytes(bytes);
            return new Guid(bytes).ToString();
        }
    }
}
