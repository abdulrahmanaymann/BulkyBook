﻿// <auto-generated />
using BulkyBook.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BulkyBook.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("BulkyBook.Models.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Price100")
                        .HasColumnType("float");

                    b.Property<double>("Price50")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "Author 1",
                            CategoryId = 1,
                            Description = "Description 1",
                            ISBN = "ISBN 1",
                            ImageUrl = "",
                            ListPrice = 10.99,
                            Price = 9.9900000000000002,
                            Price100 = 7.9900000000000002,
                            Price50 = 8.9900000000000002,
                            Title = "Book 1"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Author 2",
                            CategoryId = 2,
                            Description = "Description 2",
                            ISBN = "ISBN 2",
                            ImageUrl = "",
                            ListPrice = 20.989999999999998,
                            Price = 19.989999999999998,
                            Price100 = 17.989999999999998,
                            Price50 = 18.989999999999998,
                            Title = "Book 2"
                        },
                        new
                        {
                            Id = 3,
                            Author = "Author 3",
                            CategoryId = 3,
                            Description = "Description 3",
                            ISBN = "ISBN 3",
                            ImageUrl = "",
                            ListPrice = 30.989999999999998,
                            Price = 29.989999999999998,
                            Price100 = 27.989999999999998,
                            Price50 = 28.989999999999998,
                            Title = "Book 3"
                        },
                        new
                        {
                            Id = 4,
                            Author = "Author 4",
                            CategoryId = 4,
                            Description = "Description 4",
                            ISBN = "ISBN 4",
                            ImageUrl = "",
                            ListPrice = 40.990000000000002,
                            Price = 39.990000000000002,
                            Price100 = 37.990000000000002,
                            Price50 = 38.990000000000002,
                            Title = "Book 4"
                        },
                        new
                        {
                            Id = 5,
                            Author = "Author 5",
                            CategoryId = 2,
                            Description = "Description 5",
                            ISBN = "ISBN 5",
                            ImageUrl = "",
                            ListPrice = 50.990000000000002,
                            Price = 49.990000000000002,
                            Price100 = 47.990000000000002,
                            Price50 = 48.990000000000002,
                            Title = "Book 5"
                        },
                        new
                        {
                            Id = 6,
                            Author = "Author 6",
                            CategoryId = 1,
                            Description = "Description 6",
                            ISBN = "ISBN 6",
                            ImageUrl = "",
                            ListPrice = 60.990000000000002,
                            Price = 59.990000000000002,
                            Price100 = 57.990000000000002,
                            Price50 = 58.990000000000002,
                            Title = "Book 6"
                        });
                });

            modelBuilder.Entity("BulkyBook.Models.Models.Product", b =>
                {
                    b.HasOne("BulkyBook.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
