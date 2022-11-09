﻿// <auto-generated />
using System;
using ElevatorApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElevatorApi.Migrations
{
    [DbContext(typeof(SqlDbContext))]
    [Migration("20221109161426_RemovedBadData")]
    partial class RemovedBadData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ElevatorApi.Data.Entities.CommentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ErrandEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LastEditedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastEditedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ErrandEntityId");

                    b.ToTable("CommentEntity");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.ElevatorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("ElevatorStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("LastEditedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastEditedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Elevators");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.ErrandEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssignedToId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ElevatorEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ErrandStatus")
                        .HasColumnType("int");

                    b.Property<Guid>("LastEditedById")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastEditedDateUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ElevatorEntityId");

                    b.ToTable("Errands");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.CommentEntity", b =>
                {
                    b.HasOne("ElevatorApi.Data.Entities.ErrandEntity", "ErrandEntity")
                        .WithMany("Comments")
                        .HasForeignKey("ErrandEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ErrandEntity");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.ErrandEntity", b =>
                {
                    b.HasOne("ElevatorApi.Data.Entities.ElevatorEntity", "ElevatorEntity")
                        .WithMany("Errands")
                        .HasForeignKey("ElevatorEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ElevatorEntity");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.ElevatorEntity", b =>
                {
                    b.Navigation("Errands");
                });

            modelBuilder.Entity("ElevatorApi.Data.Entities.ErrandEntity", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
