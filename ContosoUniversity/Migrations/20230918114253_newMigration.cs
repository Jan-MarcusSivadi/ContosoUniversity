using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "RoWVersion",
                table: "Department",
                newName: "RowVersion");

            migrationBuilder.AlterColumn<byte>(
                name: "RowVersion",
                table: "Department",
                type: "tinyint",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldRowVersion: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Course",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course",
                column: "DepartmentID",
                principalTable: "Department",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "RowVersion",
                table: "Department",
                newName: "RoWVersion");

            migrationBuilder.AlterColumn<byte>(
                name: "RoWVersion",
                table: "Department",
                type: "tinyint",
                rowVersion: true,
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldRowVersion: true,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DepartmentID",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course",
                column: "DepartmentID",
                principalTable: "Department",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
