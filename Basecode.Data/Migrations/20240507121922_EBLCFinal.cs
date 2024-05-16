using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Basecode.Data.Migrations
{
    /// <inheritdoc />
    public partial class EBLCFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "subjectname",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Parent");

            migrationBuilder.DropColumn(
                name: "email",
                table: "NewEnrollee");

            migrationBuilder.DropColumn(
                name: "classidT",
                table: "ClassStudents");

            migrationBuilder.DropColumn(
                name: "studentid",
                table: "ClassStudents");

            migrationBuilder.DropColumn(
                name: "schoolyear",
                table: "Class");

            migrationBuilder.RenameColumn(
                name: "profilepic",
                table: "UsersPortal",
                newName: "ProfilePic");

            migrationBuilder.RenameColumn(
                name: "middlename",
                table: "UsersPortal",
                newName: "MiddleName");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "UsersPortal",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "UsersPortal",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "haschild",
                table: "Subject",
                newName: "HasChild");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Subject",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "subjectid",
                table: "Subject",
                newName: "Subject_Id");

            migrationBuilder.RenameColumn(
                name: "studentid",
                table: "Student",
                newName: "Student_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Settings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RTPUsers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RTPCommons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RefreshToken",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Parent",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Gcash",
                table: "Parent",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Learner_Values",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Learner_Values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "studentid",
                table: "Learner_Values",
                newName: "Student_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "HeadSubject",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Grades",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "subjectid",
                table: "Grades",
                newName: "Subject_Id");

            migrationBuilder.RenameColumn(
                name: "studentid",
                table: "Grades",
                newName: "Student_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Core_Values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ClassSubjects",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "subjectid",
                table: "ClassSubjects",
                newName: "Subject_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ClassStudents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "grade",
                table: "Class",
                newName: "Grade");

            migrationBuilder.RenameColumn(
                name: "classsize",
                table: "Class",
                newName: "ClassSize");

            migrationBuilder.RenameColumn(
                name: "classname",
                table: "Class",
                newName: "ClassName");

            migrationBuilder.RenameColumn(
                name: "adviser",
                table: "Class",
                newName: "Adviser");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Class",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ChildSubject",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "subjectid",
                table: "ChildSubject",
                newName: "Subject_Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Behavioural_Statement",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Attendance",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "AspNetUsers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUsers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "Grade",
                table: "Subject",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject_Name",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Administrator",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "DepEdLogo",
                table: "Settings",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassingGrade",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "SchoolLogo",
                table: "Settings",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "School_Name",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithHighHonor",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithHighestHonor",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithHonor",
                table: "Settings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Class_Id",
                table: "ClassStudents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Student_Id",
                table: "ClassStudents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AdminUserPortal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUserPortal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeacherRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserPortalID = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherRegistration", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUserPortal");

            migrationBuilder.DropTable(
                name: "TeacherRegistration");

            migrationBuilder.DropColumn(
                name: "Subject_Name",
                table: "Subject");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Administrator",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "DepEdLogo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "PassingGrade",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "SchoolLogo",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "School_Name",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "WithHighHonor",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "WithHighestHonor",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "WithHonor",
                table: "Settings");

            migrationBuilder.DropColumn(
                name: "Class_Id",
                table: "ClassStudents");

            migrationBuilder.DropColumn(
                name: "Student_Id",
                table: "ClassStudents");

            migrationBuilder.RenameColumn(
                name: "ProfilePic",
                table: "UsersPortal",
                newName: "profilepic");

            migrationBuilder.RenameColumn(
                name: "MiddleName",
                table: "UsersPortal",
                newName: "middlename");

            migrationBuilder.RenameColumn(
                name: "lastname",
                table: "UsersPortal",
                newName: "lastname");

            migrationBuilder.RenameColumn(
                name: "firstname",
                table: "UsersPortal",
                newName: "firstname");

            migrationBuilder.RenameColumn(
                name: "HasChild",
                table: "Subject",
                newName: "haschild");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Subject",
                newName: "grade");

            migrationBuilder.RenameColumn(
                name: "Subject_Id",
                table: "Subject",
                newName: "subjectid");

            migrationBuilder.RenameColumn(
                name: "Student_Id",
                table: "Student",
                newName: "studentid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Settings",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RTPUsers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RTPCommons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RefreshToken",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Parent",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Parent",
                newName: "Gcash");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Learner_Values",
                newName: "grade");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Learner_Values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Student_Id",
                table: "Learner_Values",
                newName: "studentid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "HeadSubject",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Grades",
                newName: "grade");

            migrationBuilder.RenameColumn(
                name: "Subject_Id",
                table: "Grades",
                newName: "subjectid");

            migrationBuilder.RenameColumn(
                name: "Student_Id",
                table: "Grades",
                newName: "studentid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Core_Values",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ClassSubjects",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Subject_Id",
                table: "ClassSubjects",
                newName: "subjectid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ClassStudents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Grade",
                table: "Class",
                newName: "grade");

            migrationBuilder.RenameColumn(
                name: "ClassSize",
                table: "Class",
                newName: "classsize");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Class",
                newName: "classname");

            migrationBuilder.RenameColumn(
                name: "Adviser",
                table: "Class",
                newName: "adviser");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Class",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "ChildSubject",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Subject_Id",
                table: "ChildSubject",
                newName: "subjectid");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Behavioural_Statement",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Attendance",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "AspNetUsers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUsers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetUserClaims",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoles",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "AspNetRoleClaims",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "grade",
                table: "Subject",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "subjectname",
                table: "Subject",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Parent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "NewEnrollee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "classidT",
                table: "ClassStudents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "studentid",
                table: "ClassStudents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "schoolyear",
                table: "Class",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
