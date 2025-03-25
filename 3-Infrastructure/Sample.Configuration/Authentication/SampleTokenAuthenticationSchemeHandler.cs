using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sample.Commons;
using Sample.Commons.Extensions;
using Sample.Configuration.Authorizations;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Sample.Configuration.Authentication
{
    public class SampleTokenAuthenticationSchemeHandler
       : AuthenticationHandler<SampleTokenAuthenticationSchemeOptions>
    {
        private readonly GeneralSettings _generalSettings;
        private readonly AccessManagementInMemory _accessManagementInMemory;
        private readonly AccessTheControllers _accessTheControllers;
        private readonly IWebHostEnvironment _env;

        public SampleTokenAuthenticationSchemeHandler(
            IOptionsMonitor<SampleTokenAuthenticationSchemeOptions> options,
            IOptionsMonitor<GeneralSettings> generalSettings,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            AccessManagementInMemory accessManagementInMemory,
            AccessTheControllers accessTheControllers,
            IWebHostEnvironment env) : base(options, logger, encoder, clock)
        {
            _generalSettings = generalSettings.CurrentValue;
            _accessManagementInMemory = accessManagementInMemory;
            _accessTheControllers = accessTheControllers;
            _env = env;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Endpoint endpoint = Context.GetEndpoint();

            if (endpoint == null) return Task.FromResult(AuthenticateResult.NoResult());

            var filter = endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>();

            if (filter != null) return Task.FromResult(AuthenticateResult.NoResult());

            if (!Request.Headers.ContainsKey(SampleTokenAuthenticationSchemeOptions.CustomToken))
            {
                return Task.FromResult(AuthenticateResult.Fail("Token Not Found."));
            }

            var token = Request.Headers[SampleTokenAuthenticationSchemeOptions.CustomToken].ToString();

            if (token.IsNullOrEmpty() == true)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is Empty"));
            }

            if (token.IsValidToken() == false)
            {
                return Task.FromResult(AuthenticateResult.Fail("Token is invalid"));
            }

            var userAgent = Request.Headers["User-Agent"].ToString();

            var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            return Task.FromResult(GetUserByToken(token, userAgent, descriptor));
        }

        AuthenticateResult GetUserByToken(string token, string clientInformation, ControllerActionDescriptor descriptor)
        {
            AccessInformation accessInformation;

            if (_env.EnvironmentName.IsProduction())
                accessInformation = _accessManagementInMemory.GetAccessInformationByTokenAndSystemInformation(token, clientInformation);
            else
                accessInformation = _accessManagementInMemory.GetAccessInformationByToken(token);

            if (accessInformation == null)
            {
                return AuthenticateResult.Fail("Token is invalid");
            }

            var userAgent = Request.Headers["User-Agent"].ToString();

            // Accessing the IP Address
            var ipAddress = Context.Connection.RemoteIpAddress?.ToString();

            var access = _accessTheControllers.Controllers.FirstOrDefault
                (x => x.ControllerName == descriptor.ControllerName && x.Actions.
                Any(y => y.ActionName == descriptor.ActionName) == true);

            if (access == null) return AuthenticateResult.Fail("Access is denied!");

            var action = access.Actions.FirstOrDefault(x => x.ActionName == descriptor.ActionName);

            if (accessInformation.User == null)
                return AuthenticateResult.Fail("Access is denied!");

            if (accessInformation.User != null)
            {
                if (action.UserRoles.Contains(accessInformation.User.Role) == false)
                    return AuthenticateResult.Fail("Access is denied!");
            }

            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, token)
            };

            var claimsIdentity = new ClaimsIdentity(claims,
                        nameof(SampleTokenAuthenticationSchemeHandler));

            var ticket = new AuthenticationTicket(
                new ClaimsPrincipal(claimsIdentity), Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }

    public static class ExtensionString
    {
        public static bool IsProduction(this string value)
        {
            return value == "Production";
        }
    }
}