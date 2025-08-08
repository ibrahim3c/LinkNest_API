using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.UserProfiles.GetUserProfile
{
    internal sealed class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, GetUserProfileResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetUserProfileQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetUserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            using var _connection = _connectionFactory.CreateConnection();

            var sql = """
                SELECT 
                    up."Guid" AS UserProfileId,
                    up."FirstName" AS FirstName,
                    up."LastName" AS LastName,
                    up."Email" AS Email,
                    up."DateOfBirth" AS DateOfBirth,
                    up."CreatedOn" AS CreatedOn,
                    up."CurrentCity" AS CurrentCity, 
                    u."PhoneNumber" AS PhoneNumber
                FROM public."UserProfile" up
                INNER JOIN "Users" u ON up."AppUserId" = u."Id"
                WHERE "Guid" = @UserProfileId
                """;

            var response = await _connection.QueryFirstOrDefaultAsync<GetUserProfileResponse>(sql, new { UserProfileId = request.userProfileId });
            if (response == null)
                return Result<GetUserProfileResponse>.Failure(["User Profile not found"]);

            return Result<GetUserProfileResponse>.Success(response);
        }
    }
}
