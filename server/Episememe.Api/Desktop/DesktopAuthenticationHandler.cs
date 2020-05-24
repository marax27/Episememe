using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Episememe.Api.Desktop
{
    public class DesktopAuthenticationOptions : AuthenticationSchemeOptions { }

    public class DesktopAuthenticationHandler : AuthenticationHandler<DesktopAuthenticationOptions>
    {
        private const string LocalUserId = "local";

        public DesktopAuthenticationHandler(
            IOptionsMonitor<DesktopAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[] {new Claim(ClaimTypes.NameIdentifier, LocalUserId)},
                        Scheme.Name
                        )
                    ),
                Scheme.Name
                );

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
