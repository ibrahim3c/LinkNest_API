using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Application.Posts.GetPost;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.GetUserPosts
{
    internal sealed class GetUserPostsQueryHandler : IQueryHandler<GetUserPostsQuery, GetUserPostsResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetUserPostsQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetUserPostsResponse>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            using var connection=_connectionFactory.CreateConnection();
            const string sql = """
            SELECT 
                "Guid" AS Id,
                "Content" AS Content,
                "CreatedAt" AS CreatedAt,
                "ImageUrl" AS ImageUrl,
                "UserProfileId" AS UserProfileId
            FROM 
                "Post"
            WHERE 
                "UserProfileId" = @UserProfileId
        """;

            var posts = (await connection.QueryAsync<GetPostResponse>(sql, new { request.UserProfileId })).ToList();
            if (!posts.Any())
                return Result<GetUserPostsResponse>.Failure(["No Posts found for this user"]);

            var response = new GetUserPostsResponse
            {
                posts = posts,
                userProfileId = request.UserProfileId
            };

            return Result<GetUserPostsResponse>.Success(response);
        }
    }
}
