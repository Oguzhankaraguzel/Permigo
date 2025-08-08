using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class MigV1_0 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "RoleClaims",
            schema: "public",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                role_id = table.Column<Guid>(type: "uuid", nullable: false),
                claim_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                claim_value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_role_claims", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "Roles",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                normalized_name = table.Column<string>(type: "text", nullable: true),
                concurrency_stamp = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_roles", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "UserClaims",
            schema: "public",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                claim_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                claim_value = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_claims", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "UserLogins",
            schema: "public",
            columns: table => new
            {
                login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                provider_key = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                provider_display_name = table.Column<string>(type: "text", nullable: true),
                user_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_logins", x => new { x.login_provider, x.provider_key });
            });

        migrationBuilder.CreateTable(
            name: "UserRoles",
            schema: "public",
            columns: table => new
            {
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                role_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_roles", x => new { x.user_id, x.role_id });
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                gender = table.Column<int>(type: "integer", nullable: false),
                user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                normalized_user_name = table.Column<string>(type: "text", nullable: true),
                email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                normalized_email = table.Column<string>(type: "text", nullable: true),
                email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: true),
                security_stamp = table.Column<string>(type: "text", nullable: true),
                concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                phone_number = table.Column<string>(type: "text", nullable: true),
                phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                access_failed_count = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "UserTokens",
            schema: "public",
            columns: table => new
            {
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                login_provider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                value = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_user_tokens", x => new { x.user_id, x.login_provider, x.name });
            });

        migrationBuilder.CreateTable(
            name: "leave_types",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                description = table.Column<string>(type: "text", nullable: true),
                only_working_days = table.Column<bool>(type: "boolean", nullable: false),
                allow_past_period = table.Column<bool>(type: "boolean", nullable: false),
                max_days_per_request = table.Column<int>(type: "integer", nullable: true),
                applicable_gender = table.Column<int>(type: "integer", nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                create_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                update_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                delete_user_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_leave_types", x => x.id);
                table.ForeignKey(
                    name: "fk_leave_types_app_user_create_user_id",
                    column: x => x.create_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_types_app_user_delete_user_id",
                    column: x => x.delete_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_types_app_user_update_user_id",
                    column: x => x.update_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "working_days",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                date = table.Column<DateOnly>(type: "date", nullable: false),
                is_working_day = table.Column<bool>(type: "boolean", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                create_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                update_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                delete_user_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_working_days", x => x.id);
                table.ForeignKey(
                    name: "fk_working_days_app_user_create_user_id",
                    column: x => x.create_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_working_days_app_user_update_user_id",
                    column: x => x.update_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_working_days_users_delete_user_id",
                    column: x => x.delete_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "leave_entitlements",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                leave_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                year = table.Column<int>(type: "integer", nullable: false),
                allocated_days = table.Column<int>(type: "integer", nullable: false),
                used_days = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                create_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                update_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                delete_user_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_leave_entitlements", x => x.id);
                table.ForeignKey(
                    name: "fk_leave_entitlements_app_user_create_user_id",
                    column: x => x.create_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_entitlements_app_user_delete_user_id",
                    column: x => x.delete_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_entitlements_app_user_employee_id",
                    column: x => x.employee_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_leave_entitlements_app_user_update_user_id",
                    column: x => x.update_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_entitlements_leave_types_leave_type_id",
                    column: x => x.leave_type_id,
                    principalSchema: "public",
                    principalTable: "leave_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "leave_requests",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                requesting_employee_id = table.Column<Guid>(type: "uuid", nullable: false),
                leave_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                start_date = table.Column<DateOnly>(type: "date", nullable: false),
                end_date = table.Column<DateOnly>(type: "date", nullable: false),
                duration_days = table.Column<int>(type: "integer", nullable: false),
                status = table.Column<int>(type: "integer", nullable: false),
                approver_id = table.Column<Guid>(type: "uuid", nullable: true),
                reason = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                create_user_id = table.Column<Guid>(type: "uuid", nullable: false),
                update_user_id = table.Column<Guid>(type: "uuid", nullable: true),
                delete_user_id = table.Column<Guid>(type: "uuid", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_leave_requests", x => x.id);
                table.ForeignKey(
                    name: "fk_leave_requests_app_user_approver_id",
                    column: x => x.approver_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_requests_app_user_create_user_id",
                    column: x => x.create_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_requests_app_user_delete_user_id",
                    column: x => x.delete_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_requests_app_user_requesting_employee_id",
                    column: x => x.requesting_employee_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_leave_requests_app_user_update_user_id",
                    column: x => x.update_user_id,
                    principalSchema: "public",
                    principalTable: "Users",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "fk_leave_requests_leave_types_leave_type_id",
                    column: x => x.leave_type_id,
                    principalSchema: "public",
                    principalTable: "leave_types",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "ix_leave_entitlements_create_user_id",
            schema: "public",
            table: "leave_entitlements",
            column: "create_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_entitlements_delete_user_id",
            schema: "public",
            table: "leave_entitlements",
            column: "delete_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_entitlements_employee_id_leave_type_id_year",
            schema: "public",
            table: "leave_entitlements",
            columns: ["employee_id", "leave_type_id", "year"],
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_leave_entitlements_leave_type_id",
            schema: "public",
            table: "leave_entitlements",
            column: "leave_type_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_entitlements_update_user_id",
            schema: "public",
            table: "leave_entitlements",
            column: "update_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_approver_id",
            schema: "public",
            table: "leave_requests",
            column: "approver_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_create_user_id",
            schema: "public",
            table: "leave_requests",
            column: "create_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_delete_user_id",
            schema: "public",
            table: "leave_requests",
            column: "delete_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_leave_type_id",
            schema: "public",
            table: "leave_requests",
            column: "leave_type_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_requesting_employee_id",
            schema: "public",
            table: "leave_requests",
            column: "requesting_employee_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_requests_update_user_id",
            schema: "public",
            table: "leave_requests",
            column: "update_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_types_create_user_id",
            schema: "public",
            table: "leave_types",
            column: "create_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_types_delete_user_id",
            schema: "public",
            table: "leave_types",
            column: "delete_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_leave_types_name",
            schema: "public",
            table: "leave_types",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_leave_types_update_user_id",
            schema: "public",
            table: "leave_types",
            column: "update_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_roles_name",
            schema: "public",
            table: "Roles",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_user_roles_role_id",
            schema: "public",
            table: "UserRoles",
            column: "role_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "public",
            table: "Users",
            column: "email",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_working_days_create_user_id",
            schema: "public",
            table: "working_days",
            column: "create_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_working_days_date",
            schema: "public",
            table: "working_days",
            column: "date",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_working_days_delete_user_id",
            schema: "public",
            table: "working_days",
            column: "delete_user_id");

        migrationBuilder.CreateIndex(
            name: "ix_working_days_update_user_id",
            schema: "public",
            table: "working_days",
            column: "update_user_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "leave_entitlements",
            schema: "public");

        migrationBuilder.DropTable(
            name: "leave_requests",
            schema: "public");

        migrationBuilder.DropTable(
            name: "RoleClaims",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Roles",
            schema: "public");

        migrationBuilder.DropTable(
            name: "UserClaims",
            schema: "public");

        migrationBuilder.DropTable(
            name: "UserLogins",
            schema: "public");

        migrationBuilder.DropTable(
            name: "UserRoles",
            schema: "public");

        migrationBuilder.DropTable(
            name: "UserTokens",
            schema: "public");

        migrationBuilder.DropTable(
            name: "working_days",
            schema: "public");

        migrationBuilder.DropTable(
            name: "leave_types",
            schema: "public");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "public");
    }
}
