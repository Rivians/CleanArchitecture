﻿using CleanArchitecture.Infrastructure.Auhtentication;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.WebApi.OptionsSetup
{
    public sealed class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        private readonly IConfiguration _configuration;     // bu interface ile appsettings.json daki verilere erişebiliriz ve configüre edebiliriz.
        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection("Jwt").Bind(options);  // appsetting.json'daki Jwt kısmındaki property'leri JwtOptions ile bind eder.
        }
    }
}
