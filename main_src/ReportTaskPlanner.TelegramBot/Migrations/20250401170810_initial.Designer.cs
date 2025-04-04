﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Data.ApplicationTimeData;

#nullable disable

namespace ReportTaskPlanner.TelegramBot.Migrations
{
    [DbContext(typeof(ApplicationTimeDbContext))]
    [Migration("20250401170810_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("ReportTaskPlanner.TelegramBot.ApplicationTimeManagement.Models.ApplicationTime", b =>
                {
                    b.Property<string>("ZoneName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT")
                        .HasColumnName("date_time");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("display_name");

                    b.Property<long>("TimeStamp")
                        .HasColumnType("INTEGER")
                        .HasColumnName("time_stamp");

                    b.HasKey("ZoneName")
                        .HasName("time_zone_name");

                    b.ToTable("Application_Time", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
