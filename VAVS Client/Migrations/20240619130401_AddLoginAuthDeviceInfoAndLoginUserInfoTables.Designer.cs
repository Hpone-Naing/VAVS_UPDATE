﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VAVS_Client.Data;

#nullable disable

namespace VAVS_Client.Migrations
{
    [DbContext(typeof(VAVSClientDBContext))]
    [Migration("20240619130401_AddLoginAuthDeviceInfoAndLoginUserInfoTables")]
    partial class AddLoginAuthDeviceInfoAndLoginUserInfoTables
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("VAVS_Client.Models.DeviceInfo", b =>
                {
                    b.Property<int>("DeviceInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DeviceInfoId"), 1L, 1);

                    b.Property<string>("IpAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("OTP")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PublicIpAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ReRegistrationTime")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ReResendCodeTime")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("RegistrationCount")
                        .HasColumnType("int");

                    b.Property<int>("ResendCodeTime")
                        .HasColumnType("int");

                    b.HasKey("DeviceInfoId");

                    b.ToTable("TB_DeviceInfo");
                });

            modelBuilder.Entity("VAVS_Client.Models.Fuel", b =>
                {
                    b.Property<int>("FuelTypePkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FuelTypePkid"), 1L, 1);

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FuelType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("FuelTypePkid");

                    b.ToTable("TB_FuelType");
                });

            modelBuilder.Entity("VAVS_Client.Models.LoginAuth", b =>
                {
                    b.Property<int>("LoginAuthId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginAuthId"), 1L, 1);

                    b.Property<string>("Nrc")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("OTP")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ReResendCodeTime")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("ResendOTPCount")
                        .HasColumnType("int");

                    b.HasKey("LoginAuthId");

                    b.ToTable("TB_LoginAuth");
                });

            modelBuilder.Entity("VAVS_Client.Models.LoginUserInfo", b =>
                {
                    b.Property<int>("LoginUserInfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginUserInfoId"), 1L, 1);

                    b.Property<string>("BuildType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ContractValue")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CountryOfMade")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("EnginePower")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FuelType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime?>("LoggedInTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ModelYear")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("RememberMe")
                        .HasColumnType("bit");

                    b.Property<string>("StandardValue")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("TaxAmount")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Token")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("VehicleBrand")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("VehicleNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("LoginUserInfoId");

                    b.ToTable("TB_LoginUserInfo");
                });

            modelBuilder.Entity("VAVS_Client.Models.NRC_And_Township", b =>
                {
                    b.Property<int>("NRC_And_Township_Pkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NRC_And_Township_Pkid"), 1L, 1);

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("NrcInitialCodeEnglish")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NrcInitialCodeMyanmar")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NrcTownshipCodeEng")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NrcTownshipCodeMyn")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PresentTownship")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TownshipDigitCode")
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("NRC_And_Township_Pkid");

                    b.ToTable("TB_NRC_And_Township");
                });

            modelBuilder.Entity("VAVS_Client.Models.Payment", b =>
                {
                    b.Property<int>("PaymentPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentPkid"), 1L, 1);

                    b.Property<string>("PaymentAmount")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PaymentPkid");

                    b.ToTable("TB_Payment");
                });

            modelBuilder.Entity("VAVS_Client.Models.PersonalDetail", b =>
                {
                    b.Property<int>("PersonalPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonalPkid"), 1L, 1);

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("HousingNumber")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("NRCBackImagePath")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NRCFrontImagePath")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("NRCNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NRCTownshipInitial")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NRCTownshipNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NRCType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PersonTINNumber")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Quarter")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RegistrationStatus")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("StateDivisionPkid")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TownshipPkid")
                        .HasColumnType("int");

                    b.Property<string>("TransactionID")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PersonalPkid");

                    b.HasIndex("StateDivisionPkid");

                    b.HasIndex("TownshipPkid");

                    b.ToTable("TB_PersonalDetail");
                });

            modelBuilder.Entity("VAVS_Client.Models.StateDivision", b =>
                {
                    b.Property<int>("StateDivisionPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StateDivisionPkid"), 1L, 1);

                    b.Property<string>("CityOfRegion")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("EngShortCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("MynShortCode")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("StateDivisionCode")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("StateDivisionName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("StateDivisionPkid");

                    b.ToTable("TB_StateDivision");
                });

            modelBuilder.Entity("VAVS_Client.Models.TaxValidation", b =>
                {
                    b.Property<int>("TaxValidationPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TaxValidationPkid"), 1L, 1);

                    b.Property<string>("AttachFileName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BuildType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal?>("ContractValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("CountryOfMade")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DemandNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("EnginePower")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FormNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FuelType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ModelYear")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("OfficeLetterNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PaymentRefID")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PersonNRC")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PersonTINNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PersonTownshipPkid")
                        .HasColumnType("int");

                    b.Property<int>("PersonalPkid")
                        .HasColumnType("int");

                    b.Property<string>("QRCodeNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal?>("StandardValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("TaxAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TaxYear")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("VehicleBrand")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("VehicleNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("TaxValidationPkid");

                    b.HasIndex("PersonTownshipPkid");

                    b.HasIndex("PersonalPkid");

                    b.ToTable("TB_TaxValidation");
                });

            modelBuilder.Entity("VAVS_Client.Models.Township", b =>
                {
                    b.Property<int>("TownshipPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TownshipPkid"), 1L, 1);

                    b.Property<string>("DistrictCode")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("StateDivisionID")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<int>("StateDivisionPkid")
                        .HasColumnType("int");

                    b.Property<string>("TownshipCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("TownshipName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TownshipPkid");

                    b.HasIndex("StateDivisionPkid");

                    b.ToTable("TB_Township");
                });

            modelBuilder.Entity("VAVS_Client.Models.Township1", b =>
                {
                    b.Property<int>("TownshipPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TownshipPkid"), 1L, 1);

                    b.Property<string>("DistrictCode")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("StateDivisionID")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("TownshipCode")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("TownshipName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("TownshipPkid");

                    b.ToTable("TB_Township1");
                });

            modelBuilder.Entity("VAVS_Client.Models.User", b =>
                {
                    b.Property<int>("UserPkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPkid"), 1L, 1);

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("UserTypeID")
                        .HasColumnType("int");

                    b.HasKey("UserPkid");

                    b.HasIndex("UserTypeID");

                    b.ToTable("TB_Users");
                });

            modelBuilder.Entity("VAVS_Client.Models.UserType", b =>
                {
                    b.Property<int>("UserTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserTypeID"), 1L, 1);

                    b.Property<string>("UserTypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserTypeID");

                    b.ToTable("TB_UserTypes");
                });

            modelBuilder.Entity("VAVS_Client.Models.VehicleStandardValue", b =>
                {
                    b.Property<int>("VehicleStandardValuePkid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleStandardValuePkid"), 1L, 1);

                    b.Property<string>("AttachFileName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BuildType")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("CountryOfMade")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("EnginePower")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime?>("EntryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FuelTypePkid")
                        .HasColumnType("int");

                    b.Property<bool?>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ModelYear")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("OfficeLetterNo")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Remarks")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("StandardValue")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("StateDivisionPkid")
                        .HasColumnType("int");

                    b.Property<string>("VehicleBrand")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("VehicleNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("VehicleStandardValuePkid");

                    b.HasIndex("FuelTypePkid");

                    b.HasIndex("StateDivisionPkid");

                    b.ToTable("TB_VehicleStandardValue");
                });

            modelBuilder.Entity("VAVS_Client.Models.PersonalDetail", b =>
                {
                    b.HasOne("VAVS_Client.Models.StateDivision", "StateDivision")
                        .WithMany()
                        .HasForeignKey("StateDivisionPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VAVS_Client.Models.Township", "Township")
                        .WithMany()
                        .HasForeignKey("TownshipPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StateDivision");

                    b.Navigation("Township");
                });

            modelBuilder.Entity("VAVS_Client.Models.TaxValidation", b =>
                {
                    b.HasOne("VAVS_Client.Models.Township", "Township")
                        .WithMany()
                        .HasForeignKey("PersonTownshipPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VAVS_Client.Models.PersonalDetail", "PersonalDetail")
                        .WithMany()
                        .HasForeignKey("PersonalPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PersonalDetail");

                    b.Navigation("Township");
                });

            modelBuilder.Entity("VAVS_Client.Models.Township", b =>
                {
                    b.HasOne("VAVS_Client.Models.StateDivision", "StateDivision")
                        .WithMany()
                        .HasForeignKey("StateDivisionPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StateDivision");
                });

            modelBuilder.Entity("VAVS_Client.Models.User", b =>
                {
                    b.HasOne("VAVS_Client.Models.UserType", "UserType")
                        .WithMany()
                        .HasForeignKey("UserTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("VAVS_Client.Models.VehicleStandardValue", b =>
                {
                    b.HasOne("VAVS_Client.Models.Fuel", "Fuel")
                        .WithMany()
                        .HasForeignKey("FuelTypePkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VAVS_Client.Models.StateDivision", "StateDivision")
                        .WithMany()
                        .HasForeignKey("StateDivisionPkid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Fuel");

                    b.Navigation("StateDivision");
                });
#pragma warning restore 612, 618
        }
    }
}
