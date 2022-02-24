using SpectoLogic.Identity.EasyAuth.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SpectoLogic.Identity.EasyAuth.B2C.Tokens
{
    public class B2CIdToken : OIDCToken
    {
#pragma warning disable IDE1006 // Naming Styles
        public int auth_time { get; set; } = 0;
        public string[] emails { get; set; } = new string[0];
        public string tfp { get; set; } = string.Empty;

        // OID ==> SUB
        public override string email => emails.FirstOrDefault();
        public override string oid => sub;
#pragma warning restore IDE1006 // Naming Styles
        public override void OnBuildClaims(List<Claim> claims)
        {
            base.OnBuildClaims(claims);
            if (emails.Any())
            {
                foreach (var email in emails)
                {
                    if (claims.FirstOrDefault(c => c.Type == ClaimTypes.Email && c.Value.ToLower() == email.ToLower()) == null)
                    {
                        claims.Add(new Claim(ClaimTypes.Email, email));
                    }
                }
            }
            claims.Add(new Claim("tpf", tfp));
            claims.Add(new Claim("authtime", "0"));
        }
    }
}
