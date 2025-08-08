using LinkNest.Application.UserProfiles.GetUserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkNest.Application.Follows.GetAllFollowees
{
    public class GetAllFolloweesRespones
    {
        public Guid UserProfileId {  get; set; }
        public List<GetUserProfileResponse> FolloweesInfo {  get; set; }
    }
}
