using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSteam.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class LicenceAndGamerUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_IdPublisher",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GenrePerGames_Games_IdGame",
                table: "GenrePerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_GenrePerGames_Genres_IdGenre",
                table: "GenrePerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPerGames_Games_IdGame",
                table: "PlatformPerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPerGames_Platforms_IdPlatform",
                table: "PlatformPerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaim_Role_RoleId",
                table: "RoleClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_User_UserId",
                table: "UserClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_User_UserId",
                table: "UserLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_User_UserId",
                table: "UserToken");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_IdPublisher",
                table: "Games",
                column: "IdPublisher",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GenrePerGames_Games_IdGame",
                table: "GenrePerGames",
                column: "IdGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_GenrePerGames_Genres_IdGenre",
                table: "GenrePerGames",
                column: "IdGenre",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPerGames_Games_IdGame",
                table: "PlatformPerGames",
                column: "IdGame",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPerGames_Platforms_IdPlatform",
                table: "PlatformPerGames",
                column: "IdPlatform",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaim_Role_RoleId",
                table: "RoleClaim",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_User_UserId",
                table: "UserClaim",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_User_UserId",
                table: "UserLogin",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_User_UserId",
                table: "UserToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Publishers_IdPublisher",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GenrePerGames_Games_IdGame",
                table: "GenrePerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_GenrePerGames_Genres_IdGenre",
                table: "GenrePerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPerGames_Games_IdGame",
                table: "PlatformPerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_PlatformPerGames_Platforms_IdPlatform",
                table: "PlatformPerGames");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaim_Role_RoleId",
                table: "RoleClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaim_User_UserId",
                table: "UserClaim");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogin_User_UserId",
                table: "UserLogin");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserToken_User_UserId",
                table: "UserToken");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Publishers_IdPublisher",
                table: "Games",
                column: "IdPublisher",
                principalTable: "Publishers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GenrePerGames_Games_IdGame",
                table: "GenrePerGames",
                column: "IdGame",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GenrePerGames_Genres_IdGenre",
                table: "GenrePerGames",
                column: "IdGenre",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPerGames_Games_IdGame",
                table: "PlatformPerGames",
                column: "IdGame",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlatformPerGames_Platforms_IdPlatform",
                table: "PlatformPerGames",
                column: "IdPlatform",
                principalTable: "Platforms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaim_Role_RoleId",
                table: "RoleClaim",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaim_User_UserId",
                table: "UserClaim",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogin_User_UserId",
                table: "UserLogin",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Role_RoleId",
                table: "UserRole",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_User_UserId",
                table: "UserRole",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserToken_User_UserId",
                table: "UserToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
