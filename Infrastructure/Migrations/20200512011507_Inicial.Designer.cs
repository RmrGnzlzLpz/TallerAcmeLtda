﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(CreditoContext))]
    [Migration("20200512011507_Inicial")]
    partial class Inicial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Domain.Entities.Abono", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CreditoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaDeCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Monto")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CreditoId");

                    b.ToTable("Abonos");
                });

            modelBuilder.Entity("Domain.Entities.AbonoCuota", b =>
                {
                    b.Property<int>("AbonoId")
                        .HasColumnType("int");

                    b.Property<int>("CuotaId")
                        .HasColumnType("int");

                    b.HasKey("AbonoId", "CuotaId");

                    b.HasIndex("CuotaId");

                    b.ToTable("AbonoCuotas");
                });

            modelBuilder.Entity("Domain.Entities.Credito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("EmpleadoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaDeCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Pagado")
                        .HasColumnType("double");

                    b.Property<int>("Plazo")
                        .HasColumnType("int");

                    b.Property<double>("TasaDeInteres")
                        .HasColumnType("double");

                    b.Property<double>("Valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("EmpleadoId");

                    b.ToTable("Creditos");
                });

            modelBuilder.Entity("Domain.Entities.Cuota", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CreditoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaDePago")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.Property<double>("Pagado")
                        .HasColumnType("double");

                    b.Property<double>("Valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CreditoId");

                    b.ToTable("Cuotas");
                });

            modelBuilder.Entity("Domain.Entities.Empleado", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Cedula")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<double>("Salario")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("Empleados");
                });

            modelBuilder.Entity("Domain.Entities.Abono", b =>
                {
                    b.HasOne("Domain.Entities.Credito", null)
                        .WithMany("Abonos")
                        .HasForeignKey("CreditoId");
                });

            modelBuilder.Entity("Domain.Entities.AbonoCuota", b =>
                {
                    b.HasOne("Domain.Entities.Abono", "Abono")
                        .WithMany("AbonoCuotas")
                        .HasForeignKey("AbonoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Cuota", "Cuota")
                        .WithMany("AbonoCuotas")
                        .HasForeignKey("CuotaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.Credito", b =>
                {
                    b.HasOne("Domain.Entities.Empleado", null)
                        .WithMany("Creditos")
                        .HasForeignKey("EmpleadoId");
                });

            modelBuilder.Entity("Domain.Entities.Cuota", b =>
                {
                    b.HasOne("Domain.Entities.Credito", null)
                        .WithMany("Cuotas")
                        .HasForeignKey("CreditoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
