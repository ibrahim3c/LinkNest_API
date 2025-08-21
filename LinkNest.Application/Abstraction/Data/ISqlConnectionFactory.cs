using System.Data;

namespace LinkNest.Application.Abstraction.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
