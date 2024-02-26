﻿// <auto-generated />
using System;
using FileSystemAPI.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileSystemAPI.Persistence.Migrations
{
    [DbContext(typeof(FileSystemDbContext))]
    [Migration("20240226194003_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FileSystemAPI.Domain.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("FolderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("StoredFileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("File", (string)null);
                });

            modelBuilder.Entity("FileSystemAPI.Domain.Entities.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FolderName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("int");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folder", (string)null);

                    b.HasData(
                        new
                        {
                            Id = -1,
                            Active = true,
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FolderName = "",
                            Size = 0L
                        });
                });

            modelBuilder.Entity("FileSystemAPI.Domain.Entities.File", b =>
                {
                    b.HasOne("FileSystemAPI.Domain.Entities.Folder", "Folder")
                        .WithMany("Files")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Folder");
                });

            modelBuilder.Entity("FileSystemAPI.Domain.Entities.Folder", b =>
                {
                    b.HasOne("FileSystemAPI.Domain.Entities.Folder", "ParentFolder")
                        .WithMany("SubFolders")
                        .HasForeignKey("ParentFolderId");

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("FileSystemAPI.Domain.Entities.Folder", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("SubFolders");
                });
#pragma warning restore 612, 618
        }
    }
}
