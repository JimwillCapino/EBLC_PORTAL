﻿// <auto-generated />
using System;
using Basecode.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Basecode.Data.Migrations
{
    [DbContext(typeof(BasecodeContext))]
    [Migration("20240507121922_EBLCFinal")]
    partial class EBLCFinal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Basecode.Data.Models.AdminUserPortal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AspUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("AdminUserPortal");
                });

            modelBuilder.Entity("Basecode.Data.Models.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Days_of_Present")
                        .HasColumnType("int");

                    b.Property<int>("Days_of_Schoool")
                        .HasColumnType("int");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("School_Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Studentid")
                        .HasColumnType("int");

                    b.Property<int>("Time_of_Tardy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Attendance");
                });

            modelBuilder.Entity("Basecode.Data.Models.Behavioural_Statement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Core_Values")
                        .HasColumnType("int");

                    b.Property<string>("Statements")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Behavioural_Statement");
                });

            modelBuilder.Entity("Basecode.Data.Models.ChildSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("HeadSubjectId")
                        .HasColumnType("int");

                    b.Property<int>("Subject_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ChildSubject");
                });

            modelBuilder.Entity("Basecode.Data.Models.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Adviser")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClassName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClassSize")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("Basecode.Data.Models.ClassStudents", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Class_Id")
                        .HasColumnType("int");

                    b.Property<int>("Student_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ClassStudents");
                });

            modelBuilder.Entity("Basecode.Data.Models.ClassSubjects", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<int?>("Subject_Id")
                        .HasColumnType("int");

                    b.Property<string>("Teacher_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ClassSubjects");
                });

            modelBuilder.Entity("Basecode.Data.Models.Core_Values", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("core_Values")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Core_Values");
                });

            modelBuilder.Entity("Basecode.Data.Models.Grades", b =>
                {
                    b.Property<int>("Grade_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Grade_Id"));

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<int>("Grade_Level")
                        .HasColumnType("int");

                    b.Property<int?>("Quarter")
                        .HasColumnType("int");

                    b.Property<string>("School_Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Student_Id")
                        .HasColumnType("int");

                    b.Property<int>("Subject_Id")
                        .HasColumnType("int");

                    b.HasKey("Grade_Id");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("Basecode.Data.Models.HeadSubject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Subect_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("HeadSubject");
                });

            modelBuilder.Entity("Basecode.Data.Models.Learner_Values", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Behavioural_Statement")
                        .HasColumnType("int");

                    b.Property<string>("Grade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quarter")
                        .HasColumnType("int");

                    b.Property<string>("School_Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Student_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Learner_Values");
                });

            modelBuilder.Entity("Basecode.Data.Models.NewEnrollee", b =>
                {
                    b.Property<int>("Enrollee_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Enrollee_Id"));

                    b.Property<byte[]>("BirthCertificate")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("CGM")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime?>("ExamSchedule")
                        .HasColumnType("datetime2");

                    b.Property<int>("GradeEnrolled")
                        .HasColumnType("int");

                    b.Property<int>("ParentID")
                        .HasColumnType("int");

                    b.Property<byte[]>("TOR")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.HasKey("Enrollee_Id");

                    b.ToTable("NewEnrollee");
                });

            modelBuilder.Entity("Basecode.Data.Models.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Parent");
                });

            modelBuilder.Entity("Basecode.Data.Models.RTPCommons", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RTPCommons");
                });

            modelBuilder.Entity("Basecode.Data.Models.RTPUsers", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AspUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RTPId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RTPUsers");
                });

            modelBuilder.Entity("Basecode.Data.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ExpiresUtc")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("IssuedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Basecode.Data.Models.Settings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Administrator")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("DepEdLogo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Division")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndofClass")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PassingGrade")
                        .HasColumnType("int");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SchoolId")
                        .HasColumnType("int");

                    b.Property<byte[]>("SchoolLogo")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("School_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("StartofClass")
                        .HasColumnType("datetime2");

                    b.Property<int?>("WithHighHonor")
                        .HasColumnType("int");

                    b.Property<int?>("WithHighestHonor")
                        .HasColumnType("int");

                    b.Property<int?>("WithHonor")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("Basecode.Data.Models.Student", b =>
                {
                    b.Property<int>("Student_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Student_Id"));

                    b.Property<int>("CurrGrade")
                        .HasColumnType("int");

                    b.Property<string>("LRN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ParentId")
                        .HasColumnType("int");

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Student_Id");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("Basecode.Data.Models.Subject", b =>
                {
                    b.Property<int>("Subject_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Subject_Id"));

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<bool>("HasChild")
                        .HasColumnType("bit");

                    b.Property<string>("Subject_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Subject_Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("Basecode.Data.Models.Teacher", b =>
                {
                    b.Property<int>("Teacher_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Teacher_Id"));

                    b.Property<int>("UID")
                        .HasColumnType("int");

                    b.HasKey("Teacher_Id");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("Basecode.Data.Models.TeacherRegistration", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserPortalID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("TeacherRegistration");
                });

            modelBuilder.Entity("Basecode.Data.Models.UsersPortal", b =>
                {
                    b.Property<int>("UID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UID"));

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePic")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("sex")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UID");

                    b.ToTable("UsersPortal");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
