using Newtonsoft.Json;
using System;
using System.Text;

namespace SpectoLogic.Identity.EasyAuth.Tokens.Parser
{
    internal class TokenParser
    {
        internal static OIDCToken? ParseToken<T>(string tokenContent) where T : OIDCToken
        {
            T? token = null;
            try
            {
                tokenContent = ConvertTokenToJsonString(tokenContent);
                token = JsonConvert.DeserializeObject<T>(tokenContent);
            }
            catch (Exception)
            {
            }
            return token;
        }
        public static string ConvertTokenToJsonString(string b64token)
        {
            b64token = b64token.PadRight(b64token.Length + (b64token.Length * 3) % 4, '=');
            b64token = Encoding.UTF8.GetString(Convert.FromBase64String(b64token));
            return b64token;
        }
    }
}
