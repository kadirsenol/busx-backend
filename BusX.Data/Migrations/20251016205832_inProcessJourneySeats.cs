using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusX.Data.Migrations
{
    /// <inheritdoc />
    public partial class inProcessJourneySeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "in_process_journey_seats",
                columns: table => new
                {
                    in_process_journey_seat_id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    journey_id = table.Column<int>(type: "INTEGER", nullable: false),
                    seat_no = table.Column<int>(type: "INTEGER", nullable: false),
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
                    table.PrimaryKey("pk_in_process_journey_seats", x => x.in_process_journey_seat_id);
                });

            migrationBuilder.CreateIndex(
                name: "i_x_in_process_journey_seats_is_deleted",
                table: "in_process_journey_seats",
                column: "is_deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "in_process_journey_seats");
        }
    }
}
