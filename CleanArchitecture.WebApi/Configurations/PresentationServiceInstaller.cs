
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace CleanArchitecture.WebApi.Configurations
{
    public sealed class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {

            services.AddCors(options =>  // CORS, bir web sayfasının farklı bir kaynaktan (domain, protokol veya port) veri istemesine olanak tanıyan bir mekanizmadır.
            {
                options.AddDefaultPolicy(
                    policy =>
                    policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()  // Bu, CORS politikasında herhangi bir HTTP metoduna (GET, POST, PUT, DELETE vs.) izin verir.
                    .AllowCredentials()  // Bu kısım, kimlik doğrulama bilgilerine (credentials) izin verir.
                    .SetIsOriginAllowed(policy => true));  // Bu satır, tüm kökenlere (origins) izin verir.
            });


            services.AddControllers()
                .AddApplicationPart(typeof(
                CleanArchitecture.Presentation.AssemblyReference).Assembly); // mevcut uygulamama, başka bir katmanda controllerların devam edebileceğini söyledik.   



            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(setup =>
            {
                var jwtSecuritySheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
            });
        }
    }
}
