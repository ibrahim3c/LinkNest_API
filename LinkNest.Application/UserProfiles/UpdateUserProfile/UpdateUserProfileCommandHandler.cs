using ApartmentBooking.Domain.Users;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Identity;
using LinkNest.Domain.UserProfiles;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace LinkNest.Application.UserProfiles.UpdateUserProfile
{
    internal class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }
        public async Task<Result> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.userProfileRepo.GetByIdAsync(request.Id);
            if (user == null)
                return Result.Failure(["No User Found"]);

            var isEmailTaken = await unitOfWork.userProfileRepo.IsEmailExist(request.Email, user.Email.email);
            if (isEmailTaken)
                return Result.Failure(["Email is already in use by another user."]);


            user.Update(
                new FirstName (request.FirstName),
                new LastName( request.LastName),
                new UserProfileEmail( request.Email),
                request.DateOfBirth,
                new CurrentCity( request.CurrentCity)
            );

            var appUser = await userManager.FindByIdAsync(user.AppUserId);
            if (user == null)
                return Result.Failure(["No User Found"]);

            var fullName = request.FirstName + request.LastName;
            if (appUser.UserName != fullName)
                appUser.UserName = fullName;


            if (appUser.PhoneNumber != request.PhoneNumber)
                appUser.PhoneNumber = request.PhoneNumber;

            await userManager.UpdateAsync(appUser);

            await unitOfWork.SaveChangesAsync();

            return Result.Success();

        }
    }
}
