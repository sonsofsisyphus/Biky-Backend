using Microsoft.Extensions.Options;
using Services.Authentication;

namespace Biky_Backend.Options
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private const string _SECTION_NAME = "JWT";
        private readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(_SECTION_NAME).Bind(options);
        }
    }
}
