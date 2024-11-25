using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    key = table.Column<string>(type: "VARCHAR(126)", maxLength: 126, nullable: false),
                    data = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cache", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "footnote_text",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_footnote_text", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "language",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false),
                    lang_english = table.Column<string>(type: "VARCHAR(16)", nullable: false),
                    lang_own = table.Column<string>(type: "VARCHAR(16)", nullable: false),
                    lang_code = table.Column<string>(type: "VARCHAR(2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_language", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "scripture",
                columns: table => new
                {
                    id = table.Column<byte>(type: "tinyint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    code = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: false),
                    number = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scripture", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cache_r",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cache_id = table.Column<long>(type: "bigint", nullable: false),
                    fetched_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cache_r", x => x.id);
                    table.ForeignKey(
                        name: "FK_cache_r_cache_cache_id",
                        column: x => x.cache_id,
                        principalTable: "cache",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "translation",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "VARCHAR(250)", maxLength: 300, nullable: false),
                    production_year = table.Column<DateTime>(type: "datetime", nullable: true),
                    added_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    eager_from = table.Column<DateTime>(type: "datetime", nullable: true),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1)
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
                name: "translator",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: false),
                    description = table.Column<string>(type: "varchar(max)", nullable: true),
                    url = table.Column<string>(type: "varchar(max)", nullable: true),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_translator", x => x.id);
                    table.ForeignKey(
                        name: "FK_translator_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "VARCHAR(16)", maxLength: 16, nullable: false),
                    surname = table.Column<string>(type: "VARCHAR(16)", maxLength: 16, nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    gender = table.Column<string>(type: "CHAR(1)", maxLength: 1, nullable: true),
                    Biography = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailVerified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    last_active = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_frozen = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_private = table.Column<DateTime>(type: "datetime", nullable: true),
                    preferred_languageId = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)1),
                    username = table.Column<string>(type: "VARCHAR(16)", maxLength: 16, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "root",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    latin = table.Column<string>(type: "VARCHAR(5)", maxLength: 5, nullable: false),
                    own = table.Column<string>(type: "VARCHAR(5)", maxLength: 5, nullable: false),
                    scripture_id = table.Column<byte>(type: "tinyint", nullable: false)
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
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meaning = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    scripture_id = table.Column<byte>(type: "tinyint", nullable: false),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scripture_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_scripture_meaning_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    number = table.Column<short>(type: "smallint", nullable: false),
                    scripture_id = table.Column<byte>(type: "tinyint", nullable: false)
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
                    assigned_on = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                        name: "FK_translator_translation_translator_translator_id",
                        column: x => x.translator_id,
                        principalTable: "translator",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "block",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    blocker_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    blocked_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    blocked_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    reason = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_block", x => x.id);
                    table.ForeignKey(
                        name: "FK_block_user_blocked_id",
                        column: x => x.blocked_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_block_user_blocker_id",
                        column: x => x.blocker_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    description = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    text = table.Column<string>(type: "VARCHAR(500)", maxLength: 250, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    follower_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    followed_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow", x => x.id);
                    table.ForeignKey(
                        name: "FK_follow_user_followed_id",
                        column: x => x.followed_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "follow_r",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    follower_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    followed_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    occurred_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow_r", x => x.id);
                    table.ForeignKey(
                        name: "FK_follow_r_user_followed_id",
                        column: x => x.followed_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_r_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "freeze_r",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    proceed_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lke", x => x.id);
                    table.ForeignKey(
                        name: "FK_lke_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recipient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    actor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notification_type = table.Column<int>(type: "int", nullable: false),
                    entity_type = table.Column<int>(type: "int", nullable: true),
                    entity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    is_read = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_user_actor_id",
                        column: x => x.actor_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_notification_user_recipient_id",
                        column: x => x.recipient_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_session", x => x.Key);
                    table.ForeignKey(
                        name: "FK_session_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chapter",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    number = table.Column<byte>(type: "tinyint", nullable: false),
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
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meaning = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    section_id = table.Column<short>(type: "smallint", nullable: false),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_meaning_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_section_meaning_section_section_id",
                        column: x => x.section_id,
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
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meaning = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    chapter_id = table.Column<short>(type: "smallint", nullable: false),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapter_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_chapter_meaning_chapter_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_chapter_meaning_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "verse",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    number = table.Column<short>(type: "smallint", nullable: false),
                    text = table.Column<string>(type: "varchar(max)", nullable: false),
                    text_without_vowel = table.Column<string>(type: "varchar(max)", nullable: false),
                    text_simplified = table.Column<string>(type: "varchar(max)", nullable: false),
                    chapter_id = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verse", x => x.id);
                    table.ForeignKey(
                        name: "FK_verse_chapter_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "collection_verse",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    collection_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    verse_id = table.Column<int>(type: "int", nullable: false),
                    saved_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    note = table.Column<string>(type: "VARCHAR(250)", maxLength: 250, nullable: true)
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
                    verse_id = table.Column<int>(type: "int", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    text = table.Column<string>(type: "varchar(max)", nullable: false),
                    verse_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "varchar(max)", nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transliteration",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    text = table.Column<string>(type: "varchar(max)", nullable: false),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false),
                    verse_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transliteration", x => x.id);
                    table.ForeignKey(
                        name: "FK_transliteration_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_transliteration_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "word",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sequence_number = table.Column<short>(type: "smallint", nullable: false),
                    text = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    text_without_vowel = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    text_simplified = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: true),
                    verse_id = table.Column<int>(type: "int", nullable: false),
                    root_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word", x => x.id);
                    table.ForeignKey(
                        name: "FK_word_root_root_id",
                        column: x => x.root_id,
                        principalTable: "root",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_word_verse_verse_id",
                        column: x => x.verse_id,
                        principalTable: "verse",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comment_note",
                columns: table => new
                {
                    comment_id = table.Column<long>(type: "bigint", nullable: false),
                    note_id = table.Column<long>(type: "bigint", nullable: false)
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
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "like_note",
                columns: table => new
                {
                    like_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    note_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like_note", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_like_note_lke_note_id",
                        column: x => x.note_id,
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
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TranslationTextId = table.Column<long>(type: "bigint", nullable: false),
                    suggestion_text = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    TranslationTextId1 = table.Column<long>(type: "bigint", nullable: true)
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
                        name: "FK_suggestion_translation_text_TranslationTextId1",
                        column: x => x.TranslationTextId1,
                        principalTable: "translation_text",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_suggestion_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "word_meaning",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    meaning = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false),
                    word_id = table.Column<long>(type: "bigint", nullable: false),
                    language_id = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_word_meaning", x => x.id);
                    table.ForeignKey(
                        name: "FK_word_meaning_language_language_id",
                        column: x => x.language_id,
                        principalTable: "language",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_word_meaning_word_word_id",
                        column: x => x.word_id,
                        principalTable: "word",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "language",
                columns: new[] { "id", "lang_code", "lang_english", "lang_own" },
                values: new object[,]
                {
                    { (byte)1, "en", "English", "English" },
                    { (byte)2, "de", "German", "Deutsch" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

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
                name: "IX_cache_r_cache_id",
                table: "cache_r",
                column: "cache_id");

            migrationBuilder.CreateIndex(
                name: "IX_chapter_name",
                table: "chapter",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_section_id_number",
                table: "chapter",
                columns: new[] { "section_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_meaning_chapter_id_language_id",
                table: "chapter_meaning",
                columns: new[] { "chapter_id", "language_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chapter_meaning_language_id",
                table: "chapter_meaning",
                column: "language_id");

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
                name: "IX_follow_r_followed_id",
                table: "follow_r",
                column: "followed_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_r_follower_id_followed_id",
                table: "follow_r",
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
                name: "IX_language_lang_code",
                table: "language",
                column: "lang_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_like_comment_comment_id",
                table: "like_comment",
                column: "comment_id");

            migrationBuilder.CreateIndex(
                name: "IX_like_note_note_id",
                table: "like_note",
                column: "note_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_lke_user_id",
                table: "lke",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_note_user_id",
                table: "note",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_note_verse_id_user_id",
                table: "note",
                columns: new[] { "verse_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notification_actor_id",
                table: "notification",
                column: "actor_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_recipient_id",
                table: "notification",
                column: "recipient_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_root_latin_scripture_id",
                table: "root",
                columns: new[] { "latin", "scripture_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_root_scripture_id",
                table: "root",
                column: "scripture_id");

            migrationBuilder.CreateIndex(
                name: "IX_scripture_code",
                table: "scripture",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scripture_name",
                table: "scripture",
                column: "name",
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
                name: "IX_section_name",
                table: "section",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_scripture_id_number",
                table: "section",
                columns: new[] { "scripture_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_meaning_language_id_section_id",
                table: "section_meaning",
                columns: new[] { "language_id", "section_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_section_meaning_section_id",
                table: "section_meaning",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_session_UserId",
                table: "session",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_suggestion_TranslationTextId",
                table: "suggestion",
                column: "TranslationTextId");

            migrationBuilder.CreateIndex(
                name: "IX_suggestion_TranslationTextId1",
                table: "suggestion",
                column: "TranslationTextId1");

            migrationBuilder.CreateIndex(
                name: "IX_suggestion_user_id_TranslationTextId",
                table: "suggestion",
                columns: new[] { "user_id", "TranslationTextId" },
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
                name: "IX_translator_language_id",
                table: "translator",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_translator_translation_translation_id",
                table: "translator_translation",
                column: "translation_id");

            migrationBuilder.CreateIndex(
                name: "IX_transliteration_language_id",
                table: "transliteration",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_transliteration_verse_id_language_id",
                table: "transliteration",
                columns: new[] { "verse_id", "language_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "user",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

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
                name: "IX_user_username",
                table: "user",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "user",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_verse_chapter_id_number",
                table: "verse",
                columns: new[] { "chapter_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_root_id",
                table: "word",
                column: "root_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_sequence_number_verse_id",
                table: "word",
                columns: new[] { "sequence_number", "verse_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_word_verse_id",
                table: "word",
                column: "verse_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_meaning_language_id",
                table: "word_meaning",
                column: "language_id");

            migrationBuilder.CreateIndex(
                name: "IX_word_meaning_word_id",
                table: "word_meaning",
                column: "word_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "block");

            migrationBuilder.DropTable(
                name: "cache_r");

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
                name: "follow_r");

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
                name: "transliteration");

            migrationBuilder.DropTable(
                name: "word_meaning");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "cache");

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
                name: "translator");

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
