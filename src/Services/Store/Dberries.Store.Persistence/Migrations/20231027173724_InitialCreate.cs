using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dberries.Store.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Item");

            migrationBuilder.EnsureSchema(
                name: "Location");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                schema: "Location",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                schema: "Item",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => new { x.ItemId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Rating_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Item",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "Location",
                columns: table => new
                {
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => new { x.LocationId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_Stock_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Item",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stock_Locations_LocationId",
                        column: x => x.LocationId,
                        principalSchema: "Location",
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ExternalId",
                schema: "Item",
                table: "Items",
                column: "ExternalId",
                unique: true,
                descending: new bool[0],
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_ExternalId",
                schema: "Location",
                table: "Locations",
                column: "ExternalId",
                unique: true,
                descending: new bool[0],
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ItemId",
                schema: "Location",
                table: "Stock",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating",
                schema: "Item");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "Location");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "Item");

            migrationBuilder.DropTable(
                name: "Locations",
                schema: "Location");
        }
    }
}
