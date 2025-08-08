using LinkNest.Application.Abstraction.Auth;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using LinkNest.Infrastructure.Auth;
using LinkNest.Infrastructure.Data;
using LinkNest.Infrastructure.Email;
using LinkNest.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LinkNest.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration) {


            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ??throw new ArgumentNullException(nameof(configuration));
            //sendGrid
           services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));


            #region EFCore
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            #endregion

            #region Dapper
            services.AddSingleton<ISqlConnectionFactory>(_ => new SqlConnectionFactory(connectionString));
            #endregion

            #region Repository Pattern with UOW
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            #region JWTConfigs
            //(1)
            // identity ===> i spend one day to find out that you are the problem => it should be above JWTConfigs :(
            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


            // JWTHelper (2)
            services.Configure<JWT>(configuration.GetSection("JWT"));

            // (3)
            // to use jwt token to check authantication =>[authorize]
            services.AddAuthentication(options =>
            {
                // to change default authantication to jwt 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //  if u are unauthanticated it will redirect you to login form
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // if there other schemas make is default of jwt
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


                // these configs to check if has token only but i want to check if he has right claims
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                // check if token have specific data
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),

                    // if u want when the token expire he does not give me مهله بعض الوقت 
                    ClockSkew = TimeSpan.Zero

                };
            }

                         );
            #endregion


            #region resolving
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IEmailService, EmailService>();

            #endregion


            return services;
        }
    }
}
