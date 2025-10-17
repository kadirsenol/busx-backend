using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusX.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "journeys",
                columns: table => new
                {
                    journey_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    from = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    to = table.Column<string>(type: "TEXT", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    departure = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    provider = table.Column<string>(type: "TEXT", nullable: false),
                    base_price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    total_seat = table.Column<int>(type: "INTEGER", nullable: false),
                    is_wifi = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_service = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_tv = table.Column<bool>(type: "INTEGER", nullable: false),
                    is_air = table.Column<bool>(type: "INTEGER", nullable: false),
                    create_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    modify_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    delete_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    delete_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    guid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_journeys", x => x.journey_id);
                });

            migrationBuilder.CreateTable(
                name: "stations",
                columns: table => new
                {
                    station_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    city = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    create_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    modify_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    delete_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    delete_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    guid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stations", x => x.station_id);
                });

            migrationBuilder.CreateTable(
                name: "tickets",
                columns: table => new
                {
                    ticket_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    pnr = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    journey_id = table.Column<int>(type: "INTEGER", nullable: false),
                    seat_no = table.Column<int>(type: "INTEGER", nullable: false),
                    passenger_name = table.Column<string>(type: "TEXT", nullable: false),
                    passenger_surname = table.Column<string>(type: "TEXT", nullable: false),
                    is_male = table.Column<bool>(type: "INTEGER", nullable: false),
                    tc_no = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    create_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    create_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modify_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    modify_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    delete_user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    delete_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    is_deleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    guid = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tickets", x => x.ticket_id);
                    table.ForeignKey(
                        name: "FK_tickets_journeys_journey_id",
                        column: x => x.journey_id,
                        principalTable: "journeys",
                        principalColumn: "journey_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_journeys_is_deleted",
                table: "journeys",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "i_x_stations_is_deleted",
                table: "stations",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "i_x_tickets_is_deleted",
                table: "tickets",
                column: "is_deleted");

            migrationBuilder.CreateIndex(
                name: "i_x_tickets_journey_id",
                table: "tickets",
                column: "journey_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "stations");

            migrationBuilder.DropTable(
                name: "tickets");

            migrationBuilder.DropTable(
                name: "journeys");
        }
    }
}
