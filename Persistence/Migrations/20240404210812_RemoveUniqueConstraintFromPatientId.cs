using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromPatientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_medical_histories_patient_id",
                table: "medical_histories");

            migrationBuilder.CreateIndex(
                name: "ix_medical_histories_patient_id",
                table: "medical_histories",
                column: "patient_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_medical_histories_patient_id",
                table: "medical_histories");

            migrationBuilder.CreateIndex(
                name: "ix_medical_histories_patient_id",
                table: "medical_histories",
                column: "patient_id",
                unique: true);
        }
    }
}
