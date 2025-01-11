using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Phonebook.Report.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reports",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    request_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    report_status = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    person_count = table.Column<long>(type: "bigint", nullable: true),
                    phone_count = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_reports", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reports");
        }
    }
}
