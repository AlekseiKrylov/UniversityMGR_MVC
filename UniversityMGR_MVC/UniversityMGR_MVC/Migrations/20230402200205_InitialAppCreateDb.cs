using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityMGR_MVC.Migrations
{
    public partial class InitialAppCreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Name", "Description" },
                values: new object[,]
                {
                    { 1, "Astronomy", "Astronomy is the only field of study at Hogwarts that has a direct equivalent in the Muggle world." },
                    { 2, "Charms", "Charms is a type of spells concerned with giving an object new and unexpected properties." },
                    { 3, "D.A.D.A.", "Defence Against the Dark Arts, shortened to D.A.D.A., teaches students defensive techniques to defend against the Dark Arts, and to be protected from dark creatures." },
                    { 4, "Herbology", "Herbology is the study of magical plants and how to take care of, utilise and combat them." },
                    { 5, "History of Magic", "History of Magic is the study of magical history." },
                    { 6, "Potions", "Potions is the art of creating mixtures with magical effects." },
                    { 7, "Transfiguration", "Transfiguration is the art of changing the form or appearance of an object." }
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "Name", "CourseId" },
                values: new object[,]
                {
                    { 1, "GD-01", 2 },
                    { 2, "HP-01", 4 },
                    { 3, "RC-01", 7 },
                    { 4, "SR-01", 6 }
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "FirstName", "LastName", "GroupId" },
                values: new object[,]
                {
                    { 1, "Katie", "Bell", 1 },
                    { 2, "Neville", "Longbottom", 1 },
                    { 3, "Hermione", "Granger", 1 },
                    { 4, "Harry", "Potter", 1 },
                    { 5, "Ronald", "Weasley", 1 },
                    { 6, "Romilda", "Vane", 1 },
                    { 7, "Ginevra", "Weasley", 1 },
                    { 8, "Parvati", "Patil", 1 },
                    { 9, "Ritchie", "Coote", 1 },
                    { 10, "Cedric", "Diggory", 2 },
                    { 11, "Hannah", "Longbottom", 2 },
                    { 12, "Susan", "Bones", 2 },
                    { 13, "Eleanor", "Branstone", 2 },
                    { 14, "Zacharias", "Smithv", 2 },
                    { 15, "Ernest", "Macmillan", 2 },
                    { 16, "Heidi", "Macavoy", 2 },
                    { 17, "Beatrice", "Haywood", 2 },
                    { 18, "Herbert", "Fleet", 2 },
                    { 19, "Malcolm", "Preece", 2 },
                    { 20, "Marcus", "Belby", 3 },
                    { 21, "Luna", "Lovegood", 3 },
                    { 22, "Cho", "Chang", 3 },
                    { 23, "Padma", "Patil", 3 },
                    { 24, "Roger", "Davies", 3 },
                    { 25, "Michael", "Corner", 3 },
                    { 26, "Penelope", "Clearwater", 3 },
                    { 27, "Terry", "Boot", 3 },
                    { 28, "Anthony", "Goldstein", 3 },
                    { 29, "Grant", "Page", 3 },
                    { 30, "Marietta", "Edgecombe", 3 },
                    { 31, "Draco", "Malfoy", 4 },
                    { 32, "Millicent", "Bulstrode", 4 },
                    { 33, "Vincent", "Crabbe", 4 },
                    { 34, "Pansy", "Parkinson", 4 },
                    { 35, "Marcus", "Flint", 4 },
                    { 36, "Adrian", "Pucey", 4 },
                    { 37, "Lucian", "Bole", 4 },
                    { 38, "Agnes", "Monkleigh", 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_CourseId",
                table: "Groups",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GroupId",
                table: "Students",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
