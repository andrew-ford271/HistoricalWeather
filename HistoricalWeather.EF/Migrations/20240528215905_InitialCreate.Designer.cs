﻿// <auto-generated />
using HistoricalWeather.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HistoricalWeather.EF.Migrations
{
    [DbContext(typeof(NoaaWeatherContext))]
    [Migration("20240528215905_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HistoricalWeather.Domain.Models.Station", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("char(11)");

                    b.Property<decimal>("Elevation")
                        .HasColumnType("decimal(5,1)");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("decimal(6,4)");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("decimal(7,4)");

                    b.Property<string>("State")
                        .HasColumnType("char(2)");

                    b.Property<string>("StationName")
                        .IsRequired()
                        .HasColumnType("char(32)");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("HistoricalWeather.Domain.Models.StationObservationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EndDate")
                        .HasColumnType("int");

                    b.Property<string>("ObservationType")
                        .IsRequired()
                        .HasColumnType("char(4)");

                    b.Property<int>("StartDate")
                        .HasColumnType("int");

                    b.Property<string>("StationId")
                        .IsRequired()
                        .HasColumnType("char(11)");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("StationObservationTypes");
                });

            modelBuilder.Entity("HistoricalWeather.Domain.Models.WeatherRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<string>("MFlag")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("ObservationType")
                        .IsRequired()
                        .HasColumnType("char(4)");

                    b.Property<string>("QFlag")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<string>("SFlag")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<string>("StationId")
                        .IsRequired()
                        .HasColumnType("char(11)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StationId");

                    b.ToTable("WeatherRecords");
                });

            modelBuilder.Entity("HistoricalWeather.Domain.Models.StationObservationType", b =>
                {
                    b.HasOne("HistoricalWeather.Domain.Models.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });

            modelBuilder.Entity("HistoricalWeather.Domain.Models.WeatherRecord", b =>
                {
                    b.HasOne("HistoricalWeather.Domain.Models.Station", "Station")
                        .WithMany()
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Station");
                });
#pragma warning restore 612, 618
        }
    }
}
