using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace writings_backend_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cache",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    Data = table.Column<JsonDocument>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cache", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "footnote_text",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_footnote_text", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangEnglish = table.Column<string>(type: "varchar(20)", nullable: false),
                    LangOwn = table.Column<string>(type: "varchar(20)", nullable: false),
                    LangCode = table.Column<string>(type: "varchar(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "request_logs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identifier = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    Endpoint = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    Method = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    StatusCode = table.Column<int>(type: "integer", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "scripture",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "char(1)", maxLength: 1, nullable: false),
                    Number = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scripture", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "translation",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    translation_name = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    production_year = table.Column<DateTime>(type: "timestamp", nullable: true),
                    added_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    eager_from = table.Column<DateTime>(type: "timestamp", nullable: true),
                    language_id = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation", x => x.id);
                    table.ForeignKey(
                        name: "FK_translation_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "translators",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    translator_name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true),
                    Url = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true),
                    language_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translators", x => x.id);
                    table.ForeignKey(
                        name: "FK_translators_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Username = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Surname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Gender = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: true),
                    Biography = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    EmailVerified = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    last_active = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_frozen = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_private = table.Column<DateTime>(type: "timestamp", nullable: true),
                    role_id = table.Column<short>(type: "smallint", nullable: true),
                    preferred_languageId = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_language_preferred_languageId",
                        column: x => x.preferred_languageId,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "root",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latin = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Own = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    scripture_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_root", x => x.id);
                    table.ForeignKey(
                        name: "FK_root_scripture_scripture_id",
                        column: x => x.scripture_id,
                        principalTable: "scripture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scripture_meaning",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Meaning = table.Column<string>(type: "varchar(50)", nullable: false),
                    scripture_id = table.Column<short>(type: "smallint", nullable: false),
                    language_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scripture_meaning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scripture_meaning_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scripture_meaning_scripture_scripture_id",
                        column: x => x.scripture_id,
                        principalTable: "scripture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "section",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    section_name = table.Column<string>(type: "varchar(100)", nullable: false),
                    section_number = table.Column<short>(type: "smallint", nullable: false),
                    scripture_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_scripture_scripture_id",
                        column: x => x.scripture_id,
                        principalTable: "scripture",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "translator_translation",
                columns: table => new
                {
                    translator_id = table.Column<short>(type: "smallint", nullable: false),
                    translation_id = table.Column<short>(type: "smallint", nullable: false),
                    assigned_on = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translator_translation", x => new { x.translator_id, x.translation_id });
                    table.ForeignKey(
                        name: "FK_translator_translation_translation_translation_id",
                        column: x => x.translation_id,
                        principalTable: "translation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_translator_translation_translators_translator_id",
                        column: x => x.translator_id,
                        principalTable: "translators",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "block",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    blocker_id = table.Column<Guid>(type: "uuid", nullable: false),
                    blocked_id = table.Column<Guid>(type: "uuid", nullable: false),
                    blocked_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    reason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_block", x => x.id);
                    table.ForeignKey(
                        name: "FK_block_user_blocked_id",
                        column: x => x.blocked_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_block_user_blocker_id",
                        column: x => x.blocker_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "comment",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    parent_comment_id = table.Column<long>(type: "bigint", nullable: true),
                    CommentVerseId = table.Column<long>(type: "bigint", nullable: true),
                    CommentNoteId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_comment_comment_parent_comment_id",
                        column: x => x.parent_comment_id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follow",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    follower_id = table.Column<Guid>(type: "uuid", nullable: false),
                    followed_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Accepted"),
                    occurred_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow", x => x.id);
                    table.ForeignKey(
                        name: "FK_follow_user_followed_id",
                        column: x => x.followed_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_follow_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "freeze_r",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Frozen"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proceed_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_freeze_r", x => x.id);
                    table.ForeignKey(
                        name: "FK_freeze_r_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lke",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lke", x => x.id);
                    table.ForeignKey(
                        name: "FK_lke_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    recipient_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actor_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_type = table.Column<string>(type: "text", nullable: false),
                    entity_type = table.Column<string>(type: "text", nullable: true),
                    entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_read = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_user_actor_id",
                        column: x => x.actor_id,
                        principalTable: "user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_notification_user_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    id = table.Column<string>(type: "varchar(100)", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: true),
                    expires_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    session = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.id);
                    table.ForeignKey(
                        name: "FK_session_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "chapter",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chapter_name = table.Column<string>(type: "varchar(250)", nullable: false),
                    chapter_number = table.Column<short>(type: "smallint", nullable: false),
                    section_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapter", x => x.id);
                    table.ForeignKey(
                        name: "FK_chapter_section_section_id",
                        column: x => x.section_id,
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "section_meaning",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Meaning = table.Column<string>(type: "text", nullable: false),
                    SectionId = table.Column<short>(type: "smallint", nullable: false),
                    LanguageId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_meaning_language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_section_meaning_section_SectionId",
                        column: x => x.SectionId,
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "like_comment",
                columns: table => new
                {
                    likeId = table.Column<long>(type: "bigint", nullable: false),
                    comment_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_comment", x => x.likeId);
                    table.ForeignKey(
                        name: "FK_like_comment_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_like_comment_lke_likeId",
                        column: x => x.likeId,
                        principalTable: "lke",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chapter_meaning",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chapter_meaning = table.Column<string>(type: "varchar(50)", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false),
                    LanguageId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapter_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_chapter_meaning_chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chapter_meaning_language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "verse",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    verse_number = table.Column<short>(type: "smallint", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    TextNoVowel = table.Column<string>(type: "text", nullable: false),
                    TextSimplified = table.Column<string>(type: "text", nullable: false),
                    ChapterId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verse", x => x.id);
                    table.ForeignKey(
                        name: "FK_verse_chapter_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "collection_verse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    collection_id = table.Column<Guid>(type: "uuid", nullable: false),
                    verse_id = table.Column<int>(type: "integer", nullable: false),
                    saved_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    note = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_verse", x => x.id);
                    table.ForeignKey(
                        name: "FK_collection_verse_collection_collection_id",
                        column: x => x.collection_id,
                        principalTable: "collection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_collection_verse_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment_verse",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    verse_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_verse", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comment_verse_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_verse_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "note",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    verse_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_note", x => x.id);
                    table.ForeignKey(
                        name: "FK_note_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_note_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "translation_text",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    translation_id = table.Column<short>(type: "smallint", nullable: false),
                    verse_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translation_text", x => x.id);
                    table.ForeignKey(
                        name: "FK_translation_text_translation_translation_id",
                        column: x => x.translation_id,
                        principalTable: "translation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_translation_text_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transliterations",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Text = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: false),
                    LanguageId = table.Column<short>(type: "smallint", nullable: false),
                    VerseId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transliterations", x => x.id);
                    table.ForeignKey(
                        name: "FK_transliterations_language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transliterations_verse_VerseId",
                        column: x => x.VerseId,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "word",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sequence_number = table.Column<short>(type: "smallint", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    TextNoVowel = table.Column<string>(type: "text", nullable: true),
                    TextSimplified = table.Column<string>(type: "text", nullable: true),
                    VerseId = table.Column<int>(type: "integer", nullable: false),
                    RootId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word", x => x.id);
                    table.ForeignKey(
                        name: "FK_word_root_RootId",
                        column: x => x.RootId,
                        principalTable: "root",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_word_verse_VerseId",
                        column: x => x.VerseId,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment_note",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    note_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comment_note", x => x.comment_id);
                    table.ForeignKey(
                        name: "FK_comment_note_comment_comment_id",
                        column: x => x.comment_id,
                        principalTable: "comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comment_note_note_note_id",
                        column: x => x.note_id,
                        principalTable: "note",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "like_note",
                columns: table => new
                {
                    likeId = table.Column<long>(type: "bigint", nullable: false),
                    note_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_note", x => x.likeId);
                    table.ForeignKey(
                        name: "FK_like_note_lke_likeId",
                        column: x => x.likeId,
                        principalTable: "lke",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_like_note_note_note_id",
                        column: x => x.note_id,
                        principalTable: "note",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "footnote",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    number = table.Column<short>(type: "smallint", nullable: false),
                    index = table.Column<short>(type: "smallint", nullable: false),
                    footnote_text_id = table.Column<long>(type: "bigint", nullable: false),
                    translation_text_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_footnote", x => x.id);
                    table.ForeignKey(
                        name: "FK_footnote_footnote_text_footnote_text_id",
                        column: x => x.footnote_text_id,
                        principalTable: "footnote_text",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_footnote_translation_text_translation_text_id",
                        column: x => x.translation_text_id,
                        principalTable: "translation_text",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suggestion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TranslationTextId = table.Column<long>(type: "bigint", nullable: false),
                    SuggestionText = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suggestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_suggestion_translation_text_TranslationTextId",
                        column: x => x.TranslationTextId,
                        principalTable: "translation_text",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_suggestion_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "word_meanings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    word_meaning = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    WordId = table.Column<long>(type: "bigint", nullable: false),
                    LanguageId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_meanings", x => x.id);
                    table.ForeignKey(
                        name: "FK_word_meanings_language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_word_meanings_word_WordId",
                        column: x => x.WordId,
                        principalTable: "word",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "language",
                columns: new[] { "id", "LangCode", "LangEnglish", "LangOwn" },
                values: new object[,]
                {
                    { (short)1, "en", "English", "English" },
                    { (short)2, "de", "German", "Deutsch" }
                });

            migrationBuilder.InsertData(
                table: "role",
                columns: new[] { "id", "Description", "role_name" },
                values: new object[,]
                {
                    { (short)1, null, "Admin" },
                    { (short)2, null, "Verified" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_block_blocked_id",
                table: "block",
                column: "blocked_id");

            migrationBuilder.CreateIndex(
                name: "IX_block_blocker_id_blocked_id",
                table: "block",
                columns: new[] { "blocker_id", "blocked_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_chapter_name",
                table: "chapter",
                column: "chapter_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_section_id_chapter_number",
                table: "chapter",
                columns: new[] { "section_id", "chapter_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_meaning_ChapterId_LanguageId",
                table: "chapter_meaning",
                columns: new[] { "ChapterId", "LanguageId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_meaning_LanguageId",
                table: "chapter_meaning",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_collection_user_id_name",
                table: "collection",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_collection_verse_collection_id_verse_id",
                table: "collection_verse",
                columns: new[] { "collection_id", "verse_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_collection_verse_verse_id",
                table: "collection_verse",
                column: "verse_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_parent_comment_id",
                table: "comment",
                column: "parent_comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_user_id",
                table: "comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_note_note_id",
                table: "comment_note",
                column: "note_id");

            migrationBuilder.CreateIndex(
                name: "IX_comment_verse_verse_id",
                table: "comment_verse",
                column: "verse_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_followed_id",
                table: "follow",
                column: "followed_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_follower_id_followed_id",
                table: "follow",
                columns: new[] { "follower_id", "followed_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_footnote_footnote_text_id",
                table: "footnote",
                column: "footnote_text_id");

            migrationBuilder.CreateIndex(
                name: "IX_footnote_number_footnote_text_id_index",
                table: "footnote",
                columns: new[] { "number", "footnote_text_id", "index" });

            migrationBuilder.CreateIndex(
                name: "IX_footnote_translation_text_id",
                table: "footnote",
                column: "translation_text_id");

            migrationBuilder.CreateIndex(
                name: "IX_freeze_r_user_id",
                table: "freeze_r",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_language_LangCode",
                table: "language",
                column: "LangCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_like_comment_comment_id",
                table: "like_comment",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_like_comment_likeId_comment_id",
                table: "like_comment",
                columns: new[] { "likeId", "comment_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_like_note_likeId_note_id",
                table: "like_note",
                columns: new[] { "likeId", "note_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_like_note_note_id",
                table: "like_note",
                column: "note_id");

            migrationBuilder.CreateIndex(
                name: "IX_lke_user_id",
                table: "lke",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_note_user_id",
                table: "note",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_note_verse_id",
                table: "note",
                column: "verse_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_actor_id",
                table: "notification",
                column: "actor_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_recipient_id",
                table: "notification",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "IX_role_role_name",
                table: "role",
                column: "role_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_root_Latin_scripture_id",
                table: "root",
                columns: new[] { "Latin", "scripture_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_root_scripture_id",
                table: "root",
                column: "scripture_id");

            migrationBuilder.CreateIndex(
                name: "IX_scripture_Code",
                table: "scripture",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scripture_Name",
                table: "scripture",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scripture_meaning_language_id_scripture_id",
                table: "scripture_meaning",
                columns: new[] { "language_id", "scripture_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scripture_meaning_scripture_id",
                table: "scripture_meaning",
                column: "scripture_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_scripture_id_section_number",
                table: "section",
                columns: new[] { "scripture_id", "section_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_section_name",
                table: "section",
                column: "section_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_meaning_LanguageId_SectionId",
                table: "section_meaning",
                columns: new[] { "LanguageId", "SectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_meaning_SectionId",
                table: "section_meaning",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_session_user_id",
                table: "session",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_suggestion_TranslationTextId",
                table: "suggestion",
                column: "TranslationTextId");

            migrationBuilder.CreateIndex(
                name: "IX_suggestion_UserId_TranslationTextId",
                table: "suggestion",
                columns: new[] { "UserId", "TranslationTextId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_translation_language_id",
                table: "translation",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_translation_text_translation_id",
                table: "translation_text",
                column: "translation_id");

            migrationBuilder.CreateIndex(
                name: "IX_translation_text_verse_id",
                table: "translation_text",
                column: "verse_id");

            migrationBuilder.CreateIndex(
                name: "IX_translator_translation_translation_id",
                table: "translator_translation",
                column: "translation_id");

            migrationBuilder.CreateIndex(
                name: "IX_translators_language_id",
                table: "translators",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_transliterations_LanguageId",
                table: "transliterations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_transliterations_VerseId",
                table: "transliterations",
                column: "VerseId");

            migrationBuilder.CreateIndex(
                name: "IX_user_Email",
                table: "user",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_preferred_languageId",
                table: "user",
                column: "preferred_languageId");

            migrationBuilder.CreateIndex(
                name: "IX_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_Username",
                table: "user",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_verse_ChapterId_verse_number",
                table: "verse",
                columns: new[] { "ChapterId", "verse_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_RootId",
                table: "word",
                column: "RootId");

            migrationBuilder.CreateIndex(
                name: "IX_word_sequence_number_VerseId",
                table: "word",
                columns: new[] { "sequence_number", "VerseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_VerseId",
                table: "word",
                column: "VerseId");

            migrationBuilder.CreateIndex(
                name: "IX_word_meanings_LanguageId",
                table: "word_meanings",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_word_meanings_WordId",
                table: "word_meanings",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "block");

            migrationBuilder.DropTable(
                name: "cache");

            migrationBuilder.DropTable(
                name: "chapter_meaning");

            migrationBuilder.DropTable(
                name: "collection_verse");

            migrationBuilder.DropTable(
                name: "comment_note");

            migrationBuilder.DropTable(
                name: "comment_verse");

            migrationBuilder.DropTable(
                name: "follow");

            migrationBuilder.DropTable(
                name: "footnote");

            migrationBuilder.DropTable(
                name: "freeze_r");

            migrationBuilder.DropTable(
                name: "like_comment");

            migrationBuilder.DropTable(
                name: "like_note");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "request_logs");

            migrationBuilder.DropTable(
                name: "scripture_meaning");

            migrationBuilder.DropTable(
                name: "section_meaning");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "suggestion");

            migrationBuilder.DropTable(
                name: "translator_translation");

            migrationBuilder.DropTable(
                name: "transliterations");

            migrationBuilder.DropTable(
                name: "word_meanings");

            migrationBuilder.DropTable(
                name: "collection");

            migrationBuilder.DropTable(
                name: "footnote_text");

            migrationBuilder.DropTable(
                name: "comment");

            migrationBuilder.DropTable(
                name: "lke");

            migrationBuilder.DropTable(
                name: "note");

            migrationBuilder.DropTable(
                name: "translation_text");

            migrationBuilder.DropTable(
                name: "translators");

            migrationBuilder.DropTable(
                name: "word");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "translation");

            migrationBuilder.DropTable(
                name: "root");

            migrationBuilder.DropTable(
                name: "verse");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "language");

            migrationBuilder.DropTable(
                name: "chapter");

            migrationBuilder.DropTable(
                name: "section");

            migrationBuilder.DropTable(
                name: "scripture");
        }
    }
}
