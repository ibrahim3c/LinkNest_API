using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LinkNest.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPermissionTableAndRolePermissionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    PermissionId = table.Column<int>(type: "integer", nullable: false),
                    AppRoleId = table.Column<string>(type: "text", nullable: false),
                    PermissionsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionsId",
                        column: x => x.PermissionsId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Account_Read" },
                    { 2, "Account_LockUnlock" },
                    { 3, "Account_ResetPassword" },
                    { 4, "Account_ForgotPassword" },
                    { 5, "Auth_Login" },
                    { 6, "Auth_Register" },
                    { 7, "Auth_RefreshToken" },
                    { 8, "Auth_RevokeToken" },
                    { 9, "UserProfile_Read" },
                    { 10, "UserProfile_ReadAll" },
                    { 11, "UserProfile_Update" },
                    { 12, "Post_Read" },
                    { 13, "Post_ReadAll" },
                    { 14, "Post_Create" },
                    { 15, "Post_Update" },
                    { 16, "Post_Delete" },
                    { 17, "PostComment_Create" },
                    { 18, "PostComment_Read" },
                    { 19, "PostComment_Update" },
                    { 20, "PostComment_Delete" },
                    { 21, "PostInteraction_Create" },
                    { 22, "PostInteraction_Read" },
                    { 23, "PostInteraction_Delete" },
                    { 24, "Follow_Manage" },
                    { 25, "Follow_ReadFollowers" },
                    { 26, "Follow_ReadFollowees" },
                    { 27, "Role_Manage" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_AppRoleId",
                table: "RolePermissions",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionsId",
                table: "RolePermissions",
                column: "PermissionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
