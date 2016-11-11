using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MLevanov_CMTool.Models;

namespace MLevanov_CMTool.Migrations
{
    [DbContext(typeof(CmToolContext))]
    partial class CmToolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MLevanov_CMTool.Models.Good", b =>
                {
                    b.Property<int>("GoodId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int?>("GoodsClassGoodsClassId");

                    b.Property<int>("MinPack");

                    b.Property<string>("Name");

                    b.Property<float>("Width");

                    b.HasKey("GoodId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.GoodsClass", b =>
                {
                    b.Property<int>("GoodsClassId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category");

                    b.Property<string>("ClassName");

                    b.Property<string>("Group");

                    b.Property<string>("Section");

                    b.Property<string>("Segment");

                    b.HasKey("GoodsClassId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Sale", b =>
                {
                    b.Property<int>("SaleId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Forecast");

                    b.Property<int>("MinStock");

                    b.Property<string>("ProductCode");

                    b.Property<float>("PurchasingPrice");

                    b.Property<int>("Range");

                    b.Property<int?>("SalesGoodGoodId");

                    b.Property<float>("SalesPrice");

                    b.Property<int?>("SalesStoreStoreId");

                    b.Property<int>("Salespcs");

                    b.Property<string>("StoreCode");

                    b.Property<string>("Week");

                    b.HasKey("SaleId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Shelve", b =>
                {
                    b.Property<int>("ShelveId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("SectionsNumber");

                    b.Property<int?>("ShelveGoodsClassGoodsClassId");

                    b.Property<int?>("ShelveStoreStoreId");

                    b.Property<int>("ShelvesNumber");

                    b.Property<int>("ShelvesWidth");

                    b.HasKey("ShelveId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StoreCode");

                    b.Property<string>("StoreName");

                    b.Property<float>("TotalLength");

                    b.HasKey("StoreId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Good", b =>
                {
                    b.HasOne("MLevanov_CMTool.Models.GoodsClass")
                        .WithMany()
                        .HasForeignKey("GoodsClassGoodsClassId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Sale", b =>
                {
                    b.HasOne("MLevanov_CMTool.Models.Good")
                        .WithMany()
                        .HasForeignKey("SalesGoodGoodId");

                    b.HasOne("MLevanov_CMTool.Models.Store")
                        .WithMany()
                        .HasForeignKey("SalesStoreStoreId");
                });

            modelBuilder.Entity("MLevanov_CMTool.Models.Shelve", b =>
                {
                    b.HasOne("MLevanov_CMTool.Models.GoodsClass")
                        .WithMany()
                        .HasForeignKey("ShelveGoodsClassGoodsClassId");

                    b.HasOne("MLevanov_CMTool.Models.Store")
                        .WithMany()
                        .HasForeignKey("ShelveStoreStoreId");
                });
        }
    }
}
