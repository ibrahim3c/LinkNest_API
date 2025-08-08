using ApartmentBooking.Domain.Users;
using LinkNest.Domain.Abstraction;
using LinkNest.Domain.Follows;
using LinkNest.Domain.Identity;
using LinkNest.Domain.Posts;
using LinkNest.Domain.UserProfiles.DomainEvents;

namespace LinkNest.Domain.UserProfiles
{
    public class UserProfile : Entity
    {
        public UserProfile(Guid guid, FirstName firstName, LastName lastName, UserProfileEmail email, DateTime dateOfBirth, DateTime createdOn, CurrentCity currentCity,string appUserId) : base(guid)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            CreatedOn = createdOn;
            CurrentCity = currentCity;
            AppUserId = appUserId;
        }
        // for EF Core
        private UserProfile() : base() { }

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public UserProfileEmail Email { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public CurrentCity CurrentCity { get; private set; }

        public string AppUserId { get; private set; }

        // nav properties
        public ICollection<Post> Posts { get; private set; } = new List<Post>();

        public ICollection<Follow> Following { get; private set; } = new List<Follow>();   // Users this user is following
        public ICollection<Follow> Followers { get;  private set; } = new List<Follow>(); // Users who follow this user
        public AppUser AppUser { get; private set; }





        // factory method
        public static UserProfile Create(FirstName firstName, 
            LastName lastName, 
            UserProfileEmail email,
            DateTime dateOfBirth,
            CurrentCity currentCity,
            string appUserId)
        {
            var user = new UserProfile(Guid.NewGuid(), firstName, lastName, email,dateOfBirth,DateTime.UtcNow,currentCity,appUserId);
            user.RaiseDomainEvent(new UserProfileCreatedDomainEvent(user.Guid));
            return user;
        }
        public void Update(FirstName firstName, LastName lastName, UserProfileEmail email, DateTime dateOfBirth, CurrentCity currentCity)
        {
            //To Do - validate the parameters & error handling & raise domain events if needed
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            CurrentCity = currentCity;
        }

    }
}
