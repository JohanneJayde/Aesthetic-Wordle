﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using AestheticWordle.Api.Models;

#nullable disable

namespace AestheticWordle.Api.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240507231650_WordOfTheDayGame")]
    partial class WordOfTheDayGame
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AestheticWordle.Api.Models.WordOfTheDay", b =>
                {
                    b.Property<int>("WordOfTheDayId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WordOfTheDayId"));

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("WordOfTheDayId");

                    b.ToTable("WordOfTheDay");
                });

            modelBuilder.Entity("AestheticWordle.Api.Models.WordOfTheDayGame", b =>
                {
                    b.Property<int>("WordOfTheDayGameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WordOfTheDayGameId"));

                    b.Property<int>("Attempts")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateAttempted")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsWin")
                        .HasColumnType("bit");

                    b.Property<int>("WordOfTheDayId")
                        .HasColumnType("int");

                    b.HasKey("WordOfTheDayGameId");

                    b.HasIndex("WordOfTheDayId");

                    b.ToTable("WordOfTheDayGames");
                });

            modelBuilder.Entity("AestheticWordle.Api.Models.WordOfTheDayGame", b =>
                {
                    b.HasOne("AestheticWordle.Api.Models.WordOfTheDay", "WordOfTheDay")
                        .WithMany("WordOfTheDayGames")
                        .HasForeignKey("WordOfTheDayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WordOfTheDay");
                });

            modelBuilder.Entity("AestheticWordle.Api.Models.WordOfTheDay", b =>
                {
                    b.Navigation("WordOfTheDayGames");
                });
#pragma warning restore 612, 618
        }
    }
}
