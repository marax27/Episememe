﻿// <auto-generated />
using System;
using Episememe.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Episememe.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20200519122541_AddIsPrivateToMediaInstance")]
    partial class AddIsPrivateToMediaInstance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("Episememe.Domain.Entities.BrowseToken", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BrowseTokens");
                });

            modelBuilder.Entity("Episememe.Domain.Entities.MediaInstance", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("DataType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MediaInstances");
                });

            modelBuilder.Entity("Episememe.Domain.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Episememe.Domain.HelperEntities.MediaTag", b =>
                {
                    b.Property<string>("MediaInstanceId")
                        .HasColumnType("TEXT");

                    b.Property<int>("TagId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MediaInstanceId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("MediaTags");
                });

            modelBuilder.Entity("Episememe.Domain.HelperEntities.MediaTag", b =>
                {
                    b.HasOne("Episememe.Domain.Entities.MediaInstance", "MediaInstance")
                        .WithMany("MediaTags")
                        .HasForeignKey("MediaInstanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Episememe.Domain.Entities.Tag", "Tag")
                        .WithMany("MediaTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
