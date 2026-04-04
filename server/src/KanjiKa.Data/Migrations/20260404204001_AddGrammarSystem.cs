using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KanjiKa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGrammarSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "grammar_points",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    pattern = table.Column<string>(type: "text", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false),
                    jlpt_level = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_points", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "grammar_examples",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    japanese = table.Column<string>(type: "text", nullable: false),
                    reading = table.Column<string>(type: "text", nullable: false),
                    english = table.Column<string>(type: "text", nullable: false),
                    grammar_point_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_examples", x => x.id);
                    table.ForeignKey(
                        name: "fk_grammar_examples_grammar_points_grammar_point_id",
                        column: x => x.grammar_point_id,
                        principalTable: "grammar_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_exercises",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sentence = table.Column<string>(type: "text", nullable: false),
                    correct_answer = table.Column<string>(type: "text", nullable: false),
                    distractor1 = table.Column<string>(type: "text", nullable: false),
                    distractor2 = table.Column<string>(type: "text", nullable: false),
                    distractor3 = table.Column<string>(type: "text", nullable: false),
                    grammar_point_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_exercises", x => x.id);
                    table.ForeignKey(
                        name: "fk_grammar_exercises_grammar_points_grammar_point_id",
                        column: x => x.grammar_point_id,
                        principalTable: "grammar_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grammar_proficiencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    grammar_point_id = table.Column<int>(type: "integer", nullable: false),
                    correct_count = table.Column<int>(type: "integer", nullable: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false),
                    last_practiced_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grammar_proficiencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_grammar_proficiencies_grammar_points_grammar_point_id",
                        column: x => x.grammar_point_id,
                        principalTable: "grammar_points",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_grammar_proficiencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_grammar_examples_grammar_point_id",
                table: "grammar_examples",
                column: "grammar_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_exercises_grammar_point_id",
                table: "grammar_exercises",
                column: "grammar_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_proficiencies_grammar_point_id",
                table: "grammar_proficiencies",
                column: "grammar_point_id");

            migrationBuilder.CreateIndex(
                name: "ix_grammar_proficiencies_user_id_grammar_point_id",
                table: "grammar_proficiencies",
                columns: new[] { "user_id", "grammar_point_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grammar_examples");

            migrationBuilder.DropTable(
                name: "grammar_exercises");

            migrationBuilder.DropTable(
                name: "grammar_proficiencies");

            migrationBuilder.DropTable(
                name: "grammar_points");
        }
    }
}
