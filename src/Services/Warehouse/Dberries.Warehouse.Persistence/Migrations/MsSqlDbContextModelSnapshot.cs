﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Warehouse.Persistence;

#nullable disable

namespace Warehouse.Persistence.Migrations
{
    [DbContext(typeof(MsSqlDbContext))]
    partial class MsSqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Warehouse.Core.Item", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Items", "Item");
                });

            modelBuilder.Entity("Warehouse.Core.Location", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("Locations", "Location");
                });

            modelBuilder.Entity("Warehouse.Core.Location", b =>
                {
                    b.OwnsOne("Warehouse.Core.Coordinates", "Coordinates", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<double>("Latitude")
                                .HasColumnType("float");

                            b1.Property<double>("Longitude")
                                .HasColumnType("float");

                            b1.HasKey("LocationId");

                            b1.ToTable("Locations", "Location");

                            b1.WithOwner()
                                .HasForeignKey("LocationId");
                        });

                    b.OwnsMany("Warehouse.Core.Stock", "Stock", b1 =>
                        {
                            b1.Property<Guid>("LocationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("ItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int?>("Quantity")
                                .IsRequired()
                                .HasColumnType("int");

                            b1.HasKey("LocationId", "ItemId");

                            b1.HasIndex("ItemId");

                            b1.ToTable("Stock", "Location");

                            b1.HasOne("Warehouse.Core.Item", "Item")
                                .WithMany()
                                .HasForeignKey("ItemId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner()
                                .HasForeignKey("LocationId");

                            b1.Navigation("Item");
                        });

                    b.Navigation("Coordinates")
                        .IsRequired();

                    b.Navigation("Stock");
                });
#pragma warning restore 612, 618
        }
    }
}
