using LinkNest.Application.Abstraction.Helpers;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var adminRole = Roles.AdminRole;
            var userRole = Roles.UserRole;
            var moderatorRole = Roles.ModeratorRole;

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { Guid.NewGuid().ToString(), adminRole, adminRole.ToUpper(), Guid.NewGuid().ToString() },
                    { Guid.NewGuid().ToString(), userRole, userRole.ToUpper(), Guid.NewGuid().ToString() },
                    { Guid.NewGuid().ToString(), moderatorRole, moderatorRole.ToUpper(), Guid.NewGuid().ToString() }
                }
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
