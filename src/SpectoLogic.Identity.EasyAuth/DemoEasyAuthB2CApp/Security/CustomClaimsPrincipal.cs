using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;

namespace DemoEasyAuthB2CApp.Security
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public string Email => Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
        public string ADCD_ID => Claims.FirstOrDefault(c => c.Type == nameof(CustomB2CToken.ADCD_ID))?.Value ?? string.Empty;
        public string LoyaltiyID => Claims.FirstOrDefault(c => c.Type == nameof(CustomB2CToken.LoyaltiyID))?.Value ?? string.Empty;

        public CustomClaimsPrincipal()
        {
        }

        public CustomClaimsPrincipal(IEnumerable<ClaimsIdentity> identities) : base(identities)
        {
        }

        public CustomClaimsPrincipal(BinaryReader reader) : base(reader)
        {
        }

        public CustomClaimsPrincipal(IIdentity identity) : base(identity)
        {
        }

        public CustomClaimsPrincipal(IPrincipal principal) : base(principal)
        {
        }

        protected CustomClaimsPrincipal(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
