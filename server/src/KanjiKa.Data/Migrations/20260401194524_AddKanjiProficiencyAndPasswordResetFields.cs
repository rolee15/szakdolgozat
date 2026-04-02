using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KanjiKa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKanjiProficiencyAndPasswordResetFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "password_reset_code",
                table: "users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "password_reset_expiry",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "kanji_proficiencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    kanji_id = table.Column<int>(type: "integer", nullable: false),
                    srs_stage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kanji_proficiencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_kanji_proficiencies_kanjis_kanji_id",
                        column: x => x.kanji_id,
                        principalTable: "kanjis",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kanji_proficiencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_kanji_proficiencies_kanji_id",
                table: "kanji_proficiencies",
                column: "kanji_id");

            migrationBuilder.CreateIndex(
                name: "ix_kanji_proficiencies_user_id_kanji_id",
                table: "kanji_proficiencies",
                columns: new[] { "user_id", "kanji_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kanji_proficiencies");

            migrationBuilder.DropColumn(
                name: "password_reset_code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "password_reset_expiry",
                table: "users");
        }
    }
}
