using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeTreatmentMachineIdOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_treatment_machines_treatment_rooms_treatment_room_id",
                table: "treatment_machines");

            migrationBuilder.AlterColumn<int>(
                name: "treatment_machine_id",
                table: "treatment_rooms",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "fk_treatment_machines_treatment_rooms_treatment_room_id",
                table: "treatment_machines",
                column: "treatment_room_id",
                principalTable: "treatment_rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_treatment_machines_treatment_rooms_treatment_room_id",
                table: "treatment_machines");

            migrationBuilder.AlterColumn<int>(
                name: "treatment_machine_id",
                table: "treatment_rooms",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_treatment_machines_treatment_rooms_treatment_room_id",
                table: "treatment_machines",
                column: "treatment_room_id",
                principalTable: "treatment_rooms",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
