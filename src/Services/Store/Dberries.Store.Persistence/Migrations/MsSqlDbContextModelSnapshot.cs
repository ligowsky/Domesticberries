﻿// <auto-generated />
using System;
using Dberries.Store.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dberries.Store.Persistence.Migrations
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

            modelBuilder.Entity("Dberries.Store.Core.Item", b =>
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

            modelBuilder.Entity("Dberries.Store.Core.Location", b =>
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

            modelBuilder.Entity("ItemLocation", b =>
                {
                    b.Property<Guid>("ItemsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LocationsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ItemsId", "LocationsId");

                    b.HasIndex("LocationsId");

                    b.ToTable("LocationItem", (string)null);
                });

            modelBuilder.Entity("Dberries.Store.Core.Item", b =>
                {
                    b.OwnsMany("Dberries.Store.Core.Rating", "Ratings", b1 =>
                        {
                            b1.Property<Guid?>("ItemId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid?>("UserId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<byte?>("Value")
                                .IsRequired()
                                .HasColumnType("tinyint");

                            b1.HasKey("ItemId", "UserId");

                            b1.ToTable("Rating", "Item");

                            b1.WithOwner()
                                .HasForeignKey("ItemId");
                        });

                    b.Navigation("Ratings");
                });

            modelBuilder.Entity("ItemLocation", b =>
                {
                    b.HasOne("Dberries.Store.Core.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dberries.Store.Core.Location", null)
                        .WithMany()
                        .HasForeignKey("LocationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
