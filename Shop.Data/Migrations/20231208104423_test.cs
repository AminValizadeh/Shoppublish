using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "EmailActiveCode", "FullName", "ImageName", "Mobile" },
                values: new object[] { new DateTime(2023, 12, 8, 14, 14, 23, 547, DateTimeKind.Local).AddTicks(205), "4f9a01599a46491188ea4794365edf2d", "admin", "admin", "093779077" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "EmailActiveCode", "FullName", "ImageName", "Mobile" },
                values: new object[] { new DateTime(2023, 12, 8, 14, 8, 33, 201, DateTimeKind.Local).AddTicks(7403), "d5081c536cf0433fa68df6189d087448", "", null, null });
        }
    }
}
