using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Metadata;

namespace MLevanov_CMTool.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsClass",
                columns: table => new
                {
                    GoodsClassId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    Group = table.Column<string>(nullable: true),
                    Section = table.Column<string>(nullable: true),
                    Segment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsClass", x => x.GoodsClassId);
                });
            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StoreCode = table.Column<string>(nullable: true),
                    StoreName = table.Column<string>(nullable: true),
                    TotalLength = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreId);
                });
            migrationBuilder.CreateTable(
                name: "Good",
                columns: table => new
                {
                    GoodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    GoodsClassGoodsClassId = table.Column<int>(nullable: true),
                    MinPack = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Width = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Good", x => x.GoodId);
                    table.ForeignKey(
                        name: "FK_Good_GoodsClass_GoodsClassGoodsClassId",
                        column: x => x.GoodsClassGoodsClassId,
                        principalTable: "GoodsClass",
                        principalColumn: "GoodsClassId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Shelve",
                columns: table => new
                {
                    ShelveId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SectionsNumber = table.Column<int>(nullable: false),
                    ShelveGoodsClassGoodsClassId = table.Column<int>(nullable: true),
                    ShelveStoreStoreId = table.Column<int>(nullable: true),
                    ShelvesNumber = table.Column<int>(nullable: false),
                    ShelvesWidth = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelve", x => x.ShelveId);
                    table.ForeignKey(
                        name: "FK_Shelve_GoodsClass_ShelveGoodsClassGoodsClassId",
                        column: x => x.ShelveGoodsClassGoodsClassId,
                        principalTable: "GoodsClass",
                        principalColumn: "GoodsClassId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shelve_Store_ShelveStoreStoreId",
                        column: x => x.ShelveStoreStoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    SaleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Forecast = table.Column<int>(nullable: false),
                    MinStock = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    PurchasingPrice = table.Column<float>(nullable: false),
                    Range = table.Column<int>(nullable: false),
                    SalesGoodGoodId = table.Column<int>(nullable: true),
                    SalesPrice = table.Column<float>(nullable: false),
                    SalesStoreStoreId = table.Column<int>(nullable: true),
                    Salespcs = table.Column<int>(nullable: false),
                    StoreCode = table.Column<string>(nullable: true),
                    Week = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.SaleId);
                    table.ForeignKey(
                        name: "FK_Sale_Good_SalesGoodGoodId",
                        column: x => x.SalesGoodGoodId,
                        principalTable: "Good",
                        principalColumn: "GoodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_Store_SalesStoreStoreId",
                        column: x => x.SalesStoreStoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Sale");
            migrationBuilder.DropTable("Shelve");
            migrationBuilder.DropTable("Good");
            migrationBuilder.DropTable("Store");
            migrationBuilder.DropTable("GoodsClass");
        }
    }
}
