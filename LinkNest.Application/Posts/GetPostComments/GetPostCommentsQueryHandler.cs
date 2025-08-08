using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.GetPostComments
{
    internal sealed class GetPostInteractionsQueryHandler : IQueryHandler<GetPostCommentsQuery, GetPostCommentsResponse>
    {
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public GetPostInteractionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetPostCommentsResponse>> Handle(GetPostCommentsQuery request, CancellationToken cancellationToken)
        {
            using var connection= sqlConnectionFactory.CreateConnection();

           const string sql = """
                SELECT
                "Content" as Content,
                "CreatedAt" AS CreatedAt,
                "UserProfileId" as UserProfileId
                from "PostComment"
                where "PostId"=@PostId
                """;

            var comments = (await connection.QueryAsync<CommentInfo>(sql, new { PostId = request.postId })).ToList();
            if (!comments.Any())
                return Result<GetPostCommentsResponse>.Failure(["No Comments Found"]);

            var response = new GetPostCommentsResponse()
            {
                PostComments = comments,
                PostId = request.postId
            };

            return  Result<GetPostCommentsResponse>.Success(response);

        }
    }
}
