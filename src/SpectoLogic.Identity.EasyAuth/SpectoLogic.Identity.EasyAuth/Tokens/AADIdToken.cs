using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SpectoLogic.Identity.EasyAuth.Tokens
{
    public class AADIdToken : OIDCToken
    {
#pragma warning disable IDE1006 // Naming Styles
        public string aio { get; set; } = string.Empty;
        public string[] amr { get; set; } = new string[0];
        public override string email { get; set; } = string.Empty;
        public string idp { get; set; } = string.Empty;
        public string ipaddr { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public override string oid { get; set; } = string.Empty;
        public string tid { get; set; } = string.Empty;
        public string unique_name { get; set; } = string.Empty;
#pragma warning restore IDE1006 // Naming Styles
        public override void OnBuildClaims(List<Claim> claims)
        {
            base.OnBuildClaims(claims);
            if (claims.FirstOrDefault(c => c.Type == ClaimTypes.Email && c.Value.ToLower() == email.ToLower()) == null)
            {
                claims.Add(new Claim(ClaimTypes.Email, email));
            }
            claims.Add(new Claim("aio", aio));
            claims.Add(new Claim("tid", tid));
            claims.Add(new Claim("idp", idp));
            claims.Add(new Claim("ipaddr", ipaddr));
            if (claims.FirstOrDefault(c => c.Type == ClaimTypes.Name && c.Value.ToLower() == name.ToLower()) == null)
            {
                claims.Add(new Claim(ClaimTypes.Name, name));
            }
            foreach (var armV in amr)
            {
                claims.Add(new Claim("amr", armV));
            }
        }
    }
}
