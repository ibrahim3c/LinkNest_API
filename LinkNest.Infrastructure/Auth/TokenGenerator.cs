using LinkNest.Application.Abstraction.Auth;
using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Abstraction.IServices;
using LinkNest.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LinkNest.Infrastructure.Auth
{
    internal class TokenGenerator : ITokenGenerator
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IOptionsMonitor<JWT> JWTConfigs;
        private readonly IPermissionService permissionService;

        public TokenGenerator(UserManager<AppUser> userManager,IOptionsMonitor<JWT>jWTConfigs,IPermissionService permissionService)
        {
            this.userManager = userManager;
            JWTConfigs = jWTConfigs;
            this.permissionService = permissionService;
        }
        public async Task<string> GenerateJwtTokenAsync(AppUser appUser)
        {

            var userClaims = await userManager.GetClaimsAsync(appUser);
            //var roles = await userManager.GetRolesAsync(appUser);
            //var roleClaims = new List<Claim>();

            //foreach (var role in roles)
            //    roleClaims.Add(new Claim(Constants.RolesKey, role));

            // Add permissions claims
            var permClaims = new List<Claim>();
            var permissions = await permissionService.GetPermissionsAsync(appUser.Id);
            foreach (var permission in permissions)
            {
                permClaims.Add(new Claim(Constants.PermissionsKey, permission));
            }

            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                        new Claim(Constants.UserIdKey, appUser.Id)
                    }
            .Union(userClaims)
            //.Union(roleClaims)
            .Union(permClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfigs.CurrentValue.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: JWTConfigs.CurrentValue.Issuer,
                audience: JWTConfigs.CurrentValue.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(JWTConfigs.CurrentValue.ExpireAfterInMinute),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


        }
        public  RefreshToken GenereteRefreshToken()
        {

            var randomNum = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNum);

            return new RefreshToken
            {
                ExpiresOn = DateTime.UtcNow.AddDays(15),
                Token = Convert.ToBase64String(randomNum),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}
