using Microsoft.AspNetCore.Identity;
using Moq;

namespace LinkNest.Application.UnitTests.UserProfiles
{
    public static class UserManagerMockHelper
    {
        public static Mock<UserManager<TUser>> CreateMockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }
    }

}
