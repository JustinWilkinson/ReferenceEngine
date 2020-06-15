﻿// <auto-generated />
using System;
using ReferenceEngine.Styles.UI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ReferenceEngine.Styles.UI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("Bibtex.Abstractions.BibliographyStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BibliographyStyles");
                });

            modelBuilder.Entity("Bibtex.Abstractions.EntryStyle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BibliographyStyleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FieldsString")
                        .HasColumnName("Fields")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BibliographyStyleId");

                    b.ToTable("EntryStyles");
                });

            modelBuilder.Entity("Bibtex.Abstractions.EntryStyle", b =>
                {
                    b.HasOne("Bibtex.Abstractions.BibliographyStyle", null)
                        .WithMany("EntryStyles")
                        .HasForeignKey("BibliographyStyleId");
                });
#pragma warning restore 612, 618
        }
    }
}
