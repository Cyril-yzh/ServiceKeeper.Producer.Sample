﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServiceKeeper.Producer.Sample.Domain.EFCore;

#nullable disable

namespace ServiceKeeper.Producer.Sample.Domain.Migrations
{
    [DbContext(typeof(TaskDbContext))]
    [Migration("20230828091113_TaskEntity-Init")]
    partial class TaskEntityInit
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ServiceKeeper.Core.Entity.TaskEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsFirstNode")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("TaskJson")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TriggerJson")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("TaskEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
