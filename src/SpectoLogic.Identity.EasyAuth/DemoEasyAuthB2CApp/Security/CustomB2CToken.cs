using Newtonsoft.Json;
using SpectoLogic.Identity.EasyAuth.B2C.Tokens;
using System.Security.Claims;

namespace DemoEasyAuthB2CApp.Security
{
    public class CustomB2CToken : B2CIdToken
    {
        [JsonProperty("extension_LoyaltiyID")]
        public string LoyaltiyID { get; set; } = string.Empty;
        [JsonProperty("extension_ADCD_ID")]
        public string ADCD_ID { get; set; } = string.Empty;

        public override void OnBuildClaims(List<Claim> claims)
        {
            base.OnBuildClaims(claims);
            claims.Add(new Claim(nameof(LoyaltiyID), LoyaltiyID));
            claims.Add(new Claim(nameof(ADCD_ID), ADCD_ID));
        }
    }
}
