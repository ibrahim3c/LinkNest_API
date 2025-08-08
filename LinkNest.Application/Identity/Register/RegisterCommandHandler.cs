using ApartmentBooking.Domain.Users;
using LinkNest.Application.Abstraction.Auth;
using LinkNest.Application.Abstraction.Helpers;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using LinkNest.Domain.UserProfiles;
using Microsoft.AspNetCore.Identity;

namespace LinkNest.Application.Identity.Register
{
    internal class RegisterCommandHandler : IAuthCommandHandler<RegisterCommand>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IUnitOfWork uow;
        public RegisterCommandHandler(IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            ITokenGenerator tokenGenerator
            )
        {
            this.uow = unitOfWork;
            this.userManager = userManager;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            if (await userManager.FindByEmailAsync(request.Email) is not null || await uow.userProfileRepo.IsEmailExist(request.Email))
                return new AuthResult()
                {
                    Success = false,
                    Messages = new List<string> { "Email is already Registered!" }
                };


            // Create a new user
            var user = new AppUser
            {
                UserName = request.Fname + request.Lname, 
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, request.Password);
            // add role to user
            await userManager.AddToRoleAsync(user, Roles.UserRole);


            //Create a userProfile
            var userProfile = UserProfile.Create(new FirstName(request.Fname),
                new LastName(request.Lname),
                new UserProfileEmail(request.Email),
                    request.BirthDate,
                new CurrentCity(request.CurrentCity), user.Id);

            await uow.userProfileRepo.AddAsync(userProfile);
            await uow.SaveChangesAsync();




            // generate token
            var token = await tokenGenerator.GenerateJwtTokenAsync(user);
            // generate refreshToken
            var refreshToken = tokenGenerator.GenereteRefreshToken();


            // then save it in db
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            return new AuthResult()
            {
                Success = true,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn,
                Token = token,
                RefreshToken = refreshToken.Token
            };

        }
    }
}
