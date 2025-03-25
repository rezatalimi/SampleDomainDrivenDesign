using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;

namespace Sample.Configuration.Authentication
{
    public class SampleTokenAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string SchemeName = "Sample";
        public const string CustomToken = "SampleToken";
        /// <summary>
        /// Generates the swagger custom Token security scheme object
        /// </summary>
        /// <returns>The swagger custom Token security scheme</returns>
        public static OpenApiSecurityScheme GetSwaggerCustomTokenApiSecurityScheme()
        {
            var scheme = new OpenApiSecurityScheme
            {
                Name = CustomToken,
                Type = SecuritySchemeType.ApiKey,
                Scheme = SchemeName,
                In = ParameterLocation.Header,
                Description = "Sample Token authorization header.\r\n\r\n Example: \"SampleToken  123456\""
            };
            return scheme;
        }

        /// <summary>
        /// Generates the swagger security scheme object
        /// </summary>
        /// <returns>The swagger security scheme</returns>
        public static OpenApiSecurityRequirement GetSwaggerCustomTokenSecurityRequirement()
        {
            var req = new OpenApiSecurityRequirement()
                      {
                        {
                          new OpenApiSecurityScheme()
                          {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = SchemeName
                            }
                          },
                          new string[] {}
                        }
                      };
            return req;
        }
    }
}
