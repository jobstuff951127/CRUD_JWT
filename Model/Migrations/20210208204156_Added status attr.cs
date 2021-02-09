using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class Addedstatusattr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "Costumers",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Costumers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Costumers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Costumers",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)",
                oldUnicode: false,
                oldMaxLength: 15);
        }
    }
}
