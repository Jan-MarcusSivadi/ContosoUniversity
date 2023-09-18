using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContosoUniversity.Migrations
{
    public partial class migration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "EnrollmentID",
                table: "Enrollment",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "DepartmentID",
                table: "Department",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CourseAssignment",
                newName: "ID");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Department_DepartmentID",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Enrollment",
                newName: "EnrollmentID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Department",
                newName: "DepartmentID");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CourseAssignment",
                newName: "Id");

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
                principalColumn: "DepartmentID");
        }
    }
}
