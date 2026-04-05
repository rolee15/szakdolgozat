using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace KanjiKa.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReadingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "learning_units",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_learning_units", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "reading_passages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    jlpt_level = table.Column<int>(type: "integer", nullable: false),
                    source = table.Column<string>(type: "text", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_passages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "unit_contents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    learning_unit_id = table.Column<int>(type: "integer", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    content_id = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit_contents", x => x.id);
                    table.ForeignKey(
                        name: "fk_unit_contents_learning_units_learning_unit_id",
                        column: x => x.learning_unit_id,
                        principalTable: "learning_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "unit_progresses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    learning_unit_id = table.Column<int>(type: "integer", nullable: false),
                    is_passed = table.Column<bool>(type: "boolean", nullable: false),
                    best_score = table.Column<int>(type: "integer", nullable: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false),
                    last_attempt_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit_progresses", x => x.id);
                    table.ForeignKey(
                        name: "fk_unit_progresses_learning_units_learning_unit_id",
                        column: x => x.learning_unit_id,
                        principalTable: "learning_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_unit_progresses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "unit_tests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    learning_unit_id = table.Column<int>(type: "integer", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    option_a = table.Column<string>(type: "text", nullable: false),
                    option_b = table.Column<string>(type: "text", nullable: false),
                    option_c = table.Column<string>(type: "text", nullable: false),
                    option_d = table.Column<string>(type: "text", nullable: false),
                    correct_option = table.Column<char>(type: "character(1)", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_unit_tests", x => x.id);
                    table.ForeignKey(
                        name: "fk_unit_tests_learning_units_learning_unit_id",
                        column: x => x.learning_unit_id,
                        principalTable: "learning_units",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comprehension_questions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reading_passage_id = table.Column<int>(type: "integer", nullable: false),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    option_a = table.Column<string>(type: "text", nullable: false),
                    option_b = table.Column<string>(type: "text", nullable: false),
                    option_c = table.Column<string>(type: "text", nullable: false),
                    option_d = table.Column<string>(type: "text", nullable: false),
                    correct_option = table.Column<char>(type: "character(1)", nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_comprehension_questions", x => x.id);
                    table.ForeignKey(
                        name: "fk_comprehension_questions_reading_passages_reading_passage_id",
                        column: x => x.reading_passage_id,
                        principalTable: "reading_passages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reading_proficiencies",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    reading_passage_id = table.Column<int>(type: "integer", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    attempt_count = table.Column<int>(type: "integer", nullable: false),
                    is_passed = table.Column<bool>(type: "boolean", nullable: false),
                    last_attempt_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reading_proficiencies", x => x.id);
                    table.ForeignKey(
                        name: "fk_reading_proficiencies_reading_passages_reading_passage_id",
                        column: x => x.reading_passage_id,
                        principalTable: "reading_passages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_reading_proficiencies_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_comprehension_questions_reading_passage_id",
                table: "comprehension_questions",
                column: "reading_passage_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_proficiencies_reading_passage_id",
                table: "reading_proficiencies",
                column: "reading_passage_id");

            migrationBuilder.CreateIndex(
                name: "ix_reading_proficiencies_user_id_reading_passage_id",
                table: "reading_proficiencies",
                columns: new[] { "user_id", "reading_passage_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unit_contents_learning_unit_id",
                table: "unit_contents",
                column: "learning_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_unit_progresses_learning_unit_id",
                table: "unit_progresses",
                column: "learning_unit_id");

            migrationBuilder.CreateIndex(
                name: "ix_unit_progresses_user_id_learning_unit_id",
                table: "unit_progresses",
                columns: new[] { "user_id", "learning_unit_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_unit_tests_learning_unit_id",
                table: "unit_tests",
                column: "learning_unit_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comprehension_questions");

            migrationBuilder.DropTable(
                name: "reading_proficiencies");

            migrationBuilder.DropTable(
                name: "unit_contents");

            migrationBuilder.DropTable(
                name: "unit_progresses");

            migrationBuilder.DropTable(
                name: "unit_tests");

            migrationBuilder.DropTable(
                name: "reading_passages");

            migrationBuilder.DropTable(
                name: "learning_units");
        }
    }
}
