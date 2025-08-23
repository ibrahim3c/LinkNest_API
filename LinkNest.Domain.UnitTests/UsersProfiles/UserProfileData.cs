using ApartmentBooking.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Domain.UnitTests.UsersProfiles
{
    // instead of Arrange
    internal static class UserProfileData
    {
        public static readonly Guid UserProfileGuid = Guid.NewGuid();
        public static readonly FirstName FirstName = new FirstName("John");
        public static readonly LastName LastName = new LastName("Doe");
        public static readonly UserProfileEmail Email = new UserProfileEmail("test@gmail.com");
    }

}
