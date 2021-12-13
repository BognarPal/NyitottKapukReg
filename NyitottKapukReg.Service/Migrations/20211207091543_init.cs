using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NyitottKapukReg.Service.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Day",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    MaxVisitors = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Day", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitorGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GroupNumber = table.Column<int>(nullable: false),
                    ClassroomNumber = table.Column<string>(nullable: false),
                    DayId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorGroup_Day_DayId",
                        column: x => x.DayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DayId = table.Column<int>(nullable: false),
                    VisitorGroupId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ParentName1 = table.Column<string>(nullable: true),
                    ParentName2 = table.Column<string>(nullable: true),
                    StudentName1 = table.Column<string>(nullable: true),
                    StudentName2 = table.Column<string>(nullable: true),
                    StudentName3 = table.Column<string>(nullable: true),
                    StudentName4 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registrations_Day_DayId",
                        column: x => x.DayId,
                        principalTable: "Day",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_VisitorGroup_VisitorGroupId",
                        column: x => x.VisitorGroupId,
                        principalTable: "VisitorGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Day",
                columns: new[] { "Id", "Date", "MaxVisitors" },
                values: new object[] { 1, new DateTime(2022, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 192 });

            migrationBuilder.InsertData(
                table: "VisitorGroup",
                columns: new[] { "Id", "ClassroomNumber", "DayId", "GroupNumber" },
                values: new object[,]
                {
                    { 1, "110", null, 1 },
                    { 2, "110", null, 2 },
                    { 3, "110", null, 3 },
                    { 4, "110", null, 4 },
                    { 5, "106", null, 5 },
                    { 6, "112", null, 6 },
                    { 7, "205", null, 7 },
                    { 8, "207", null, 8 },
                    { 9, "208", null, 9 },
                    { 10, "209", null, 10 },
                    { 11, "210", null, 11 },
                    { 12, "212", null, 12 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_DayId",
                table: "Registrations",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_VisitorGroupId",
                table: "Registrations",
                column: "VisitorGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorGroup_DayId",
                table: "VisitorGroup",
                column: "DayId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "VisitorGroup");

            migrationBuilder.DropTable(
                name: "Day");
        }
    }
}
