﻿// <auto-generated />
using System;
using CargoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CargoApp.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20200407121154_CorrectTableNames")]
    partial class CorrectTableNames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CargoApp.Models.Car", b =>
                {
                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<float>("Carrying")
                        .HasColumnType("real");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Volume")
                        .HasColumnType("real");

                    b.HasKey("Number");

                    b.HasIndex("DriverId")
                        .IsUnique();

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CargoApp.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Login");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CargoApp.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Inn")
                        .IsRequired()
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<string>("Kpp")
                        .IsRequired()
                        .HasColumnType("char(9)");

                    b.Property<float>("MaxCarrying")
                        .HasColumnType("real");

                    b.Property<float>("MaxVolume")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ogrn")
                        .IsRequired()
                        .HasColumnType("char(13)");

                    b.Property<DateTime>("OgrnDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasAlternateKey("Ogrn");

                    b.HasAlternateKey("Inn", "Kpp");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CargoApp.Models.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Login");

                    b.HasIndex("CompanyId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("CargoApp.Models.Good", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Height")
                        .HasColumnType("real");

                    b.Property<bool>("IsFragile")
                        .HasColumnType("bit");

                    b.Property<float>("Length")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RequestId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.Property<float>("Width")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Goods");
                });

            modelBuilder.Entity("CargoApp.Models.Logistician", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasAlternateKey("Login");

                    b.HasIndex("CompanyId");

                    b.ToTable("Logisticians");
                });

            modelBuilder.Entity("CargoApp.Models.Passport", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(7)")
                        .HasMaxLength(7);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IssuedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("IssuedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Series")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sex")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClientId");

                    b.ToTable("Passports");
                });

            modelBuilder.Entity("CargoApp.Models.Rating", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Mark")
                        .HasColumnType("int");

                    b.HasKey("ClientId", "CompanyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("CargoApp.Models.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClientId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentLatitude")
                        .HasColumnType("int");

                    b.Property<int>("CurrentLongitude")
                        .HasColumnType("int");

                    b.Property<int>("CurrentStatus")
                        .HasColumnType("int");

                    b.Property<int?>("DriverId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReceivingDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SendingDateTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("DriverId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("CargoApp.Models.UserRegData", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Пользователь");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Login");

                    b.ToTable("UserRegData");
                });

            modelBuilder.Entity("CargoApp.Models.Car", b =>
                {
                    b.HasOne("CargoApp.Models.Driver", "Driver")
                        .WithOne("Car")
                        .HasForeignKey("CargoApp.Models.Car", "DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CargoApp.Models.Client", b =>
                {
                    b.HasOne("CargoApp.Models.UserRegData", "RegData")
                        .WithMany()
                        .HasForeignKey("Login")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CargoApp.Models.Company", b =>
                {
                    b.OwnsOne("CargoApp.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("CompanyId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Addition")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Flat")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("House")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Index")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<string>("Region")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("CompanyId");

                            b1.ToTable("Companies");

                            b1.WithOwner()
                                .HasForeignKey("CompanyId");
                        });

                    b.OwnsOne("CargoApp.Models.DeliveryArea", "Area", b1 =>
                        {
                            b1.Property<int>("CompanyId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<int>("Radius")
                                .HasColumnType("int");

                            b1.HasKey("CompanyId");

                            b1.ToTable("Companies");

                            b1.WithOwner()
                                .HasForeignKey("CompanyId");
                        });
                });

            modelBuilder.Entity("CargoApp.Models.Driver", b =>
                {
                    b.HasOne("CargoApp.Models.Company", "Company")
                        .WithMany("Drivers")
                        .HasForeignKey("CompanyId")
                        .IsRequired();

                    b.HasOne("CargoApp.Models.UserRegData", "RegData")
                        .WithMany()
                        .HasForeignKey("Login")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CargoApp.Models.DeliveryArea", "DeliveryArea", b1 =>
                        {
                            b1.Property<int>("DriverId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<int>("Radius")
                                .HasColumnType("int");

                            b1.HasKey("DriverId");

                            b1.ToTable("Drivers");

                            b1.WithOwner()
                                .HasForeignKey("DriverId");
                        });
                });

            modelBuilder.Entity("CargoApp.Models.Good", b =>
                {
                    b.HasOne("CargoApp.Models.Request", "Request")
                        .WithMany("Goods")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CargoApp.Models.Logistician", b =>
                {
                    b.HasOne("CargoApp.Models.Company", "Company")
                        .WithMany("Logisticians")
                        .HasForeignKey("CompanyId")
                        .IsRequired();

                    b.HasOne("CargoApp.Models.UserRegData", "RegData")
                        .WithMany()
                        .HasForeignKey("Login")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CargoApp.Models.Passport", b =>
                {
                    b.HasOne("CargoApp.Models.Client", "Client")
                        .WithOne("Passport")
                        .HasForeignKey("CargoApp.Models.Passport", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("CargoApp.Models.Address", "BirthPlace", b1 =>
                        {
                            b1.Property<int>("PassportClientId")
                                .HasColumnType("int");

                            b1.Property<string>("Addition")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Flat")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("House")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Index")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<string>("Region")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("PassportClientId");

                            b1.ToTable("Passports");

                            b1.WithOwner()
                                .HasForeignKey("PassportClientId");
                        });
                });

            modelBuilder.Entity("CargoApp.Models.Rating", b =>
                {
                    b.HasOne("CargoApp.Models.Client", null)
                        .WithMany("CompaniesMarks")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CargoApp.Models.Company", null)
                        .WithMany("ClientsMarks")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CargoApp.Models.Request", b =>
                {
                    b.HasOne("CargoApp.Models.Client", "Client")
                        .WithMany("Requests")
                        .HasForeignKey("ClientId");

                    b.HasOne("CargoApp.Models.Driver", "Driver")
                        .WithMany("Requests")
                        .HasForeignKey("DriverId");

                    b.OwnsOne("CargoApp.Models.Address", "ReceivingAddress", b1 =>
                        {
                            b1.Property<int>("RequestId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Addition")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Flat")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("House")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Index")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<string>("Region")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("RequestId");

                            b1.ToTable("Requests");

                            b1.WithOwner()
                                .HasForeignKey("RequestId");
                        });

                    b.OwnsOne("CargoApp.Models.Address", "SendingAddress", b1 =>
                        {
                            b1.Property<int>("RequestId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("Addition")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("City")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Flat")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("House")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Index")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Latitude")
                                .HasColumnType("int");

                            b1.Property<int>("Longitude")
                                .HasColumnType("int");

                            b1.Property<string>("Region")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("RequestId");

                            b1.ToTable("Requests");

                            b1.WithOwner()
                                .HasForeignKey("RequestId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
