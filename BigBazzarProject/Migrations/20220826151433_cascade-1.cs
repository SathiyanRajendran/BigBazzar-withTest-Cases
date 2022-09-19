﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBazzar.Migrations
{
    public partial class cascade1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderMasters_OrderMasterId",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderMasters_OrderMasterId",
                table: "OrderDetails",
                column: "OrderMasterId",
                principalTable: "OrderMasters",
                principalColumn: "OrderMasterId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_OrderMasters_OrderMasterId",
                table: "OrderDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_OrderMasters_OrderMasterId",
                table: "OrderDetails",
                column: "OrderMasterId",
                principalTable: "OrderMasters",
                principalColumn: "OrderMasterId");
        }
    }
}
