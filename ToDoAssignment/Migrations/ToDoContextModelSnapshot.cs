﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToDoAssignment.Models;

namespace ToDoAssignment.Migrations
{
    [DbContext(typeof(ToDoContext))]
    partial class ToDoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ToDoAssignment.Models.CheckList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CheckListData");

                    b.Property<bool>("ChickListStatus");

                    b.Property<int?>("NotesId");

                    b.HasKey("Id");

                    b.HasIndex("NotesId");

                    b.ToTable("CheckList");
                });

            modelBuilder.Entity("ToDoAssignment.Models.Label", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LabelData");

                    b.Property<int?>("NotesId");

                    b.HasKey("Id");

                    b.HasIndex("NotesId");

                    b.ToTable("Label");
                });

            modelBuilder.Entity("ToDoAssignment.Models.Notes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("PinStatus");

                    b.Property<string>("PlainText");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("ToDoAssignment.Models.CheckList", b =>
                {
                    b.HasOne("ToDoAssignment.Models.Notes")
                        .WithMany("CheckLists")
                        .HasForeignKey("NotesId");
                });

            modelBuilder.Entity("ToDoAssignment.Models.Label", b =>
                {
                    b.HasOne("ToDoAssignment.Models.Notes")
                        .WithMany("Labels")
                        .HasForeignKey("NotesId");
                });
#pragma warning restore 612, 618
        }
    }
}