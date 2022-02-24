using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using SpectoLogic.Identity.EasyAuth.B2C.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SpectoLogic.Identity.EasyAuth.B2C.Schemes
{
    public class EasyAuthB2CSchemeOptions : AuthenticationSchemeOptions
    {
        public bool IsDevelopmentEnvironment { get; set; } = false;
        public Func<string, B2CIdToken?>? TokenFactory { get; set; } = (string json) => JsonConvert.DeserializeObject<B2CIdToken>(json);
        public Func<ClaimsIdentity, ClaimsPrincipal>? ClaimsPrincipalFactory { get; set; } = (identity) => new ClaimsPrincipal(identity);
        public Func<IEnumerable<Claim>, string, ClaimsIdentity>? ClaimsIdentityFactory { get; set; } = (claims, authType) => new ClaimsIdentity(claims,authType);
        public Func<IEnumerable<Claim>, IList<Claim>?> InjectAdditionalClaims = (existingClaims) => { return null; };
    }
}
