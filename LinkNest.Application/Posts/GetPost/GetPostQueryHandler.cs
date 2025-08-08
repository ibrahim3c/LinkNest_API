using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.GetPost
{
    public sealed class GetPostQueryHandler : IQueryHandler<GetPostQuery, GetPostResponse>
    {
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public GetPostQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetPostResponse>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            using var connection= sqlConnectionFactory.CreateConnection();
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
                "Guid" = @PostId
        """;

            var Response = await connection.QuerySingleOrDefaultAsync<GetPostResponse>(sql, new { request.PostId });

            if (Response is null)
                return Result<GetPostResponse>.Failure(["Post not found"]);

            return  Result<GetPostResponse>.Success(Response);
        }
    }
}
