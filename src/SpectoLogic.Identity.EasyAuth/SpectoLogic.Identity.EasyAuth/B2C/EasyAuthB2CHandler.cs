using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SpectoLogic.Identity.EasyAuth.B2C.Schemes;
using SpectoLogic.Identity.EasyAuth.B2C.Tokens;
using SpectoLogic.Identity.EasyAuth.Tokens;
using SpectoLogic.Identity.EasyAuth.Tokens.Parser;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SpectoLogic.Identity.EasyAuth.B2C
{
    public class EasyAuthB2CHandler
        : AuthenticationHandler<EasyAuthB2CSchemeOptions>
    {
        public const string EasyAuthB2CScheme = "EasyAuthB2CScheme";
        // https://docs.microsoft.com/en-us/azure/app-service/configure-authentication-oauth-tokens
        private const string EASYAUTH_AAD_HEADER_TOKEN_KEY = "X-MS-TOKEN-AAD-ID-TOKEN";
        private const string EASYAUTH_AAD_HEADER_TOKEN = "TOKENDESCRIPTOR.{0}.SIGNATURE";

        private readonly IConfiguration _config;
        private readonly ILogger<EasyAuthB2CHandler> _logger;

        public EasyAuthB2CHandler(
              IOptionsMonitor<EasyAuthB2CSchemeOptions> options,
              ILoggerFactory logger,
              IConfiguration config,
              UrlEncoder encoder,
              ISystemClock clock)
              : base(options, logger, encoder, clock)
        {
            _config = config;
            _logger = logger.CreateLogger<EasyAuthB2CHandler>();
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var idToken = GetEasyAuthAADIdToken();

            if (!string.IsNullOrEmpty(idToken))
            {
                try
                {
                    var principal = CreateServicePrincipalFromToken(idToken);

                    var ticket = new AuthenticationTicket(
                        principal, Scheme.Name);

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                catch (Exception)
                {
                    return Task.FromResult(AuthenticateResult.Fail("User not found"));
                }
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing the authentication header"));
            }
        }

        /// <summary>
        /// X-MS-TOKEN-AAD-ID-TOKEN
        /// X-MS-TOKEN-AAD-ACCESS-TOKEN
        /// X-MS-CLIENT-PRINCIPAL
        /// EASYAUTH_AAD_HEADER_TOKEN
        /// </summary>
        /// <returns></returns>
        private string GetEasyAuthAADIdToken()
        {
            var token = string.Empty;
            if (Options.IsDevelopmentEnvironment)
            {
                token = CreateEasyAuthAADIdDevelopmentToken();
            }
            else
            {
                token = GetEasyAuthAADIdTokenFromHeader();
            }

            if (!string.IsNullOrEmpty(token))
            {
                // Since Azure Infrastructure provides a SAFE injected AUTH-Header we only parse the content!
                var tokenElements = token.Split(".");
                if (tokenElements.Length == 3)
                {
                    return tokenElements[1];
                }
            }
            return string.Empty;
        }
        private string CreateEasyAuthAADIdDevelopmentToken()
        {
            var token = new B2CIdToken()
            {
                email = _config["EasyAuth:Email"] ?? string.Empty,
                emails = new string[1] { _config["EasyAuth:Email"] ?? string.Empty },
                family_name = _config["EasyAuth:FamilyName"] ?? string.Empty,
                given_name = _config["EasyAuth:GivenName"] ?? string.Empty,

                aud = _config["EasyAuth:Audience"] ?? string.Empty,
                iss = _config["EasyAuth:Issuer"] ?? string.Empty,
                auth_time = 0,
                exp = 0,
                iat = 0,
                nbf = 0,
                nonce = string.Empty,
                sub = _config["EasyAuth:Subject"] ?? string.Empty,
                tfp = _config["EasyAuth:B2CPolicy"] ?? string.Empty,
                ver = _config["EasyAuth:Version"] ?? "1.0"
            };
            var b64Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(token)));
            return $"I.{b64Token}.S";
        }
        private string GetEasyAuthAADIdTokenFromHeader()
        {
            var idToken = string.Empty;

            if (Request.Headers.ContainsKey(EASYAUTH_AAD_HEADER_TOKEN_KEY))
            {
                idToken = Request.Headers[EASYAUTH_AAD_HEADER_TOKEN_KEY].First();
            }
            return idToken;
        }
        private ClaimsPrincipal? CreateServicePrincipalFromToken(string tokenContent)
        {
            OIDCToken? token = null;
            if (Options.TokenFactory == null)
            {
                token = TokenParser.ParseToken<B2CIdToken>(tokenContent);
            }
            else
            {
                token = Options.TokenFactory(TokenParser.ConvertTokenToJsonString(tokenContent));
            }

            if (token != null)
            {
                ClaimsIdentity? identity = null;
                var claims = token.ToClaims();

                var additionalClaims = Options?.InjectAdditionalClaims(claims);
                if (additionalClaims != null)
                    claims.AddRange(additionalClaims);

                if (Options?.ClaimsIdentityFactory == null)
                {
                    identity = new ClaimsIdentity(claims, nameof(EasyAuthB2CHandler));
                }
                else
                {
                    identity = Options.ClaimsIdentityFactory(claims, nameof(EasyAuthB2CHandler));
                }

                ClaimsPrincipal? claimsPrincipal = null;

                if (Options?.ClaimsPrincipalFactory == null)
                {
                    claimsPrincipal = new ClaimsPrincipal(identity);
                }
                else
                {
                    claimsPrincipal = Options.ClaimsPrincipalFactory(identity);
                }

                return claimsPrincipal;
            }
            return null;
        }
    }

}
