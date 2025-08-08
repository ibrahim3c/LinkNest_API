using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Application.UserProfiles.GetUserProfile;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Follows.GetAllFollowees
{
    public sealed class GetAllFolloweesQueryHandler : IQueryHandler<GetAllFolloweesQuery, GetAllFolloweesRespones>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetAllFolloweesQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetAllFolloweesRespones>> Handle(GetAllFolloweesQuery request, CancellationToken cancellationToken)
        {
            using var connection= _connectionFactory.CreateConnection();
            var sql = """
                SELECT 
                   up."Guid" AS UserProfileId,
                   up."FirstName" AS FirstName,
                   up."LastName" AS LastName,
                   up."Email" AS Email,
                   up."DateOfBirth" AS DateOfBirth,
                   up."CreatedOn" AS CreatedOn,
                   up."CurrentCity" AS CurrentCity
                FROM 
                    "UserProfile" up
                INNER JOIN 
                    "Follows" f ON up."Guid" = f."FolloweeId"
                WHERE 
                    f."FollowerId" = @UserProfileId;
                """;


            var followees = (await connection.QueryAsync<GetUserProfileResponse>(sql, new { UserProfileId = request.userProfileId })).ToList();
            if (!followees.Any())
                return Result<GetAllFolloweesRespones>.Failure(["No Followees Found"]);

            var response = new GetAllFolloweesRespones
            {
                FolloweesInfo = followees,
                UserProfileId = request.userProfileId
            };
            return Result<GetAllFolloweesRespones>.Success(response);
        }
    }
}
