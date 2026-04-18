using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KanjiKa.Data.Migrations
{
    /// <inheritdoc />
    public partial class UnifyProficiencyHierarchy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "proficiencies");

            migrationBuilder.RenameColumn(
                name: "last_attempt_at",
                table: "reading_proficiencies",
                newName: "last_practiced_at");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "learned_at",
                table: "reading_proficiencies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "next_review_date",
                table: "reading_proficiencies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "srs_stage",
                table: "reading_proficiencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "last_practiced_at",
                table: "kanji_proficiencies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "learned_at",
                table: "grammar_proficiencies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "next_review_date",
                table: "grammar_proficiencies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "srs_stage",
                table: "grammar_proficiencies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "kana_proficiencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    srs_stage = table.Column<int>(type: "integer", nullable: false),
                    next_review_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    learned_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_practiced_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kana_proficiencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_kana_proficiencies_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kana_proficiencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_kana_proficiencies_character_id",
                table: "kana_proficiencies",
                column: "character_id");

            migrationBuilder.CreateIndex(
                name: "ix_kana_proficiencies_user_id_character_id",
                table: "kana_proficiencies",
                columns: new[] { "user_id", "character_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kana_proficiencies");

            migrationBuilder.DropColumn(
                name: "learned_at",
                table: "reading_proficiencies");

            migrationBuilder.DropColumn(
                name: "next_review_date",
                table: "reading_proficiencies");

            migrationBuilder.DropColumn(
                name: "srs_stage",
                table: "reading_proficiencies");

            migrationBuilder.DropColumn(
                name: "last_practiced_at",
                table: "kanji_proficiencies");

            migrationBuilder.DropColumn(
                name: "learned_at",
                table: "grammar_proficiencies");

            migrationBuilder.DropColumn(
                name: "next_review_date",
                table: "grammar_proficiencies");

            migrationBuilder.DropColumn(
                name: "srs_stage",
                table: "grammar_proficiencies");

            migrationBuilder.RenameColumn(
                name: "last_practiced_at",
                table: "reading_proficiencies",
                newName: "last_attempt_at");

            migrationBuilder.CreateTable(
                name: "proficiencies",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    character_id = table.Column<int>(type: "integer", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false),
                    last_practiced = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    learned_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    next_review_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    srs_stage = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_proficiencies", x => new { x.user_id, x.character_id });
                    table.ForeignKey(
                        name: "fk_proficiencies_characters_character_id",
                        column: x => x.character_id,
                        principalTable: "characters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_proficiencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_proficiencies_character_id",
                table: "proficiencies",
                column: "character_id");
        }
    }
}
