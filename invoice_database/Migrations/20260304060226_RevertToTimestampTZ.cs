using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace invoice_database.Migrations
{
    /// <inheritdoc />
    public partial class RevertToTimestampTZ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Conditionally convert columns to 'timestamp with time zone'.
            // If AddQueryFilters already ran they are 'timestamp without time zone' → convert.
            // If AddQueryFilters never ran they are already 'timestamp with time zone' → skip.
            migrationBuilder.Sql(@"
DO $$
DECLARE col_type text;
BEGIN
    SELECT data_type INTO col_type
    FROM information_schema.columns
    WHERE table_schema = 'public'
      AND table_name   = 'Invoices'
      AND column_name  = 'IssueDate';

    IF col_type = 'timestamp without time zone' THEN
        ALTER TABLE ""Invoices""
            ALTER COLUMN ""IssueDate""  TYPE timestamp with time zone USING ""IssueDate""  AT TIME ZONE 'UTC',
            ALTER COLUMN ""DueDate""    TYPE timestamp with time zone USING ""DueDate""    AT TIME ZONE 'UTC',
            ALTER COLUMN ""CreatedAt""  TYPE timestamp with time zone USING ""CreatedAt""  AT TIME ZONE 'UTC';
    END IF;
END $$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Invoices",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DueDate",
                table: "Invoices",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "IssueDate",
                table: "Invoices",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
