using Dapper;
using LinkNest.Application.Abstraction.Data;
using LinkNest.Application.Abstraction.Messaging;
using LinkNest.Application.Posts.GetPostComments;
using LinkNest.Domain.Abstraction;

namespace LinkNest.Application.Posts.GetPostInteractions
{
    internal sealed class GetPostInteractionsQueryHandler : IQueryHandler<GetPostInteractionsQuery, GetPostInteractionsResponse>
    {
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public GetPostInteractionsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<GetPostInteractionsResponse>> Handle(GetPostInteractionsQuery request, CancellationToken cancellationToken)
        {
            using var connection = sqlConnectionFactory.CreateConnection();

            const string sql = """
                SELECT
                "InteractionType" as InteractionType,
                "CreatedAt" AS CreatedAt,
                "UserProfileId" as UserProfileId
                from "PostInteraction"
                where "PostId"=@PostId
                """;

            var interactions = (await connection.QueryAsync<InteractionInfo>(sql, new { PostId = request.postId })).ToList();
            if (!interactions.Any())
                return Result<GetPostInteractionsResponse>.Failure(["No Interactions Found"]);

            var response = new GetPostInteractionsResponse()
            {
                interactionInfos = interactions,
                PostId = request.postId
            };

            return Result<GetPostInteractionsResponse>.Success(response);

        }
    }
}
