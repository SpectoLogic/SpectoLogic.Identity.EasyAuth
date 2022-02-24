using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SpectoLogic.Identity.EasyAuth.Tokens
{
    public class OIDCToken
    {
#pragma warning disable IDE1006 // Naming Styles
        public string aud { get; set; } = string.Empty;
        public string iss { get; set; } = string.Empty;
        public int iat { get; set; } = 0;
        public int nbf { get; set; } = 0;
        public int exp { get; set; } = 0;
        public string sub { get; set; } = string.Empty;
        public string nonce { get; set; } = string.Empty;
        public string given_name { get; set; } = string.Empty;
        public string family_name { get; set; } = string.Empty;

        public virtual string email { get; set; } = string.Empty;
        // OID ==> SUB bei B2CId
        public virtual string oid { get; set; } = string.Empty;

        public string ver { get; set; } = string.Empty;
#pragma warning restore IDE1006 // Naming Styles

        public List<Claim> ToClaims(List<Claim>? existingClaims = null)
        {
            if (existingClaims == null) existingClaims = new List<Claim>();
            OnBuildClaims(existingClaims);
            return existingClaims;
        }

        public virtual void OnBuildClaims(List<Claim> claims)
        {
            claims.Add(new Claim(ClaimTypes.Sid, oid));
            if (claims.FirstOrDefault(c => c.Type == ClaimTypes.Email && c.Value.ToLower() == email.ToLower()) == null)
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }
            claims.Add(new Claim(ClaimTypes.GivenName, given_name));
            claims.Add(new Claim(ClaimTypes.Name, $"{given_name} {family_name}"));
        }
    }

}
