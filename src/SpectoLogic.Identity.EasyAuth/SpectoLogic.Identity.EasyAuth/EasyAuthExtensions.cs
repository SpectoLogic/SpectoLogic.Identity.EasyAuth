using Microsoft.Extensions.DependencyInjection;
using SpectoLogic.Identity.EasyAuth.B2C;
using SpectoLogic.Identity.EasyAuth.B2C.Schemes;
using System;

namespace SpectoLogic.Identity.EasyAuth
{
    public static class EasyAuthExtensions
    {
        public static void UseEasyAuthB2C(this IServiceCollection services, Action<EasyAuthB2CSchemeOptions> options)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = EasyAuthB2CHandler.EasyAuthB2CScheme;
            })
            .AddScheme<EasyAuthB2CSchemeOptions, EasyAuthB2CHandler>
                (EasyAuthB2CHandler.EasyAuthB2CScheme, op => options(op));
        }
    }
}
