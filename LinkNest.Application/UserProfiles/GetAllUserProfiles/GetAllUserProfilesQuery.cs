using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Application.UserProfiles.GetUserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.UserProfiles.GetAllUserProfiles
{
    public sealed record GetAllUserProfilesQuery():IQuery<List<GetUserProfileResponse>>;
}
