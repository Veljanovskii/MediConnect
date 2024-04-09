﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240404210812_RemoveUniqueConstraintFromPatientId")]
    partial class RemoveUniqueConstraintFromPatientId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Consultation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("doctor_id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<bool>("IsUrgent")
                        .HasColumnType("boolean")
                        .HasColumnName("is_urgent");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("patient_id");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<int>("TreatmentRoomId")
                        .HasColumnType("integer")
                        .HasColumnName("treatment_room_id");

                    b.HasKey("Id")
                        .HasName("pk_consultations");

                    b.HasIndex("DoctorId")
                        .HasDatabaseName("ix_consultations_doctor_id");

                    b.HasIndex("PatientId")
                        .HasDatabaseName("ix_consultations_patient_id");

                    b.HasIndex("TreatmentRoomId")
                        .HasDatabaseName("ix_consultations_treatment_room_id");

                    b.ToTable("consultations", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean")
                        .HasColumnName("is_available");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Specialization")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("specialization");

                    b.HasKey("Id")
                        .HasName("pk_doctors");

                    b.ToTable("doctors", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.DoctorAvailability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<int>("DoctorId")
                        .HasColumnType("integer")
                        .HasColumnName("doctor_id");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_time");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_time");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("pk_doctor_availabilities");

                    b.HasIndex("DoctorId")
                        .IsUnique()
                        .HasDatabaseName("ix_doctor_availabilities_doctor_id");

                    b.ToTable("doctor_availabilities", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.MedicalHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("condition");

                    b.Property<string>("HistoryDetails")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("history_details");

                    b.Property<int>("PatientId")
                        .HasColumnType("integer")
                        .HasColumnName("patient_id");

                    b.HasKey("Id")
                        .HasName("pk_medical_histories");

                    b.HasIndex("PatientId")
                        .HasDatabaseName("ix_medical_histories_patient_id");

                    b.ToTable("medical_histories", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("registration_date");

                    b.HasKey("Id")
                        .HasName("pk_patients");

                    b.ToTable("patients", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.TreatmentMachine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("TreatmentRoomId")
                        .HasColumnType("integer")
                        .HasColumnName("treatment_room_id");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("type");

                    b.Property<bool>("UnderMaintenance")
                        .HasColumnType("boolean")
                        .HasColumnName("under_maintenance");

                    b.HasKey("Id")
                        .HasName("pk_treatment_machines");

                    b.HasIndex("TreatmentRoomId")
                        .IsUnique()
                        .HasDatabaseName("ix_treatment_machines_treatment_room_id");

                    b.ToTable("treatment_machines", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.TreatmentRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("RoomType")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("room_type");

                    b.Property<int?>("TreatmentMachineId")
                        .HasColumnType("integer")
                        .HasColumnName("treatment_machine_id");

                    b.HasKey("Id")
                        .HasName("pk_treatment_rooms");

                    b.ToTable("treatment_rooms", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.Consultation", b =>
                {
                    b.HasOne("Domain.Entities.Doctor", "Doctor")
                        .WithMany("Consultations")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_consultations_doctors_doctor_id");

                    b.HasOne("Domain.Entities.Patient", "Patient")
                        .WithMany("Consultations")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_consultations_patients_patient_id");

                    b.HasOne("Domain.Entities.TreatmentRoom", "TreatmentRoom")
                        .WithMany("Consultations")
                        .HasForeignKey("TreatmentRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_consultations_treatment_rooms_treatment_room_id");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");

                    b.Navigation("TreatmentRoom");
                });

            modelBuilder.Entity("Domain.Entities.DoctorAvailability", b =>
                {
                    b.HasOne("Domain.Entities.Doctor", "Doctor")
                        .WithOne("Availability")
                        .HasForeignKey("Domain.Entities.DoctorAvailability", "DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_doctor_availabilities_doctors_doctor_id");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("Domain.Entities.MedicalHistory", b =>
                {
                    b.HasOne("Domain.Entities.Patient", "Patient")
                        .WithMany("MedicalHistories")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_medical_histories_patients_patient_id");

                    b.Navigation("Patient");
                });

            modelBuilder.Entity("Domain.Entities.TreatmentMachine", b =>
                {
                    b.HasOne("Domain.Entities.TreatmentRoom", "TreatmentRoom")
                        .WithOne("TreatmentMachine")
                        .HasForeignKey("Domain.Entities.TreatmentMachine", "TreatmentRoomId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired()
                        .HasConstraintName("fk_treatment_machines_treatment_rooms_treatment_room_id");

                    b.Navigation("TreatmentRoom");
                });

            modelBuilder.Entity("Domain.Entities.Doctor", b =>
                {
                    b.Navigation("Availability")
                        .IsRequired();

                    b.Navigation("Consultations");
                });

            modelBuilder.Entity("Domain.Entities.Patient", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("MedicalHistories");
                });

            modelBuilder.Entity("Domain.Entities.TreatmentRoom", b =>
                {
                    b.Navigation("Consultations");

                    b.Navigation("TreatmentMachine")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
