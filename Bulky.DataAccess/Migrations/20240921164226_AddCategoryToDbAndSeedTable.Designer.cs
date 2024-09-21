﻿// <auto-generated />
using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240921164226_AddCategoryToDbAndSeedTable")]
    partial class AddCategoryToDbAndSeedTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BulkyBook.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Web Development"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Programming Languages"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "Databases"
                        },
                        new
                        {
                            Id = 4,
                            DisplayOrder = 4,
                            Name = "DevOps"
                        },
                        new
                        {
                            Id = 5,
                            DisplayOrder = 5,
                            Name = "Software Testing"
                        },
                        new
                        {
                            Id = 6,
                            DisplayOrder = 6,
                            Name = "Mobile Development"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
