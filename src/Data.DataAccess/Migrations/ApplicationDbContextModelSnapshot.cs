﻿// <auto-generated />
using System;
using Data.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Data.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Data.DataModels.Entities.Actor", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Country", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Director", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Directors");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProduction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CountryId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<string>("LanguageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("PlotSummary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("LanguageId");

                    b.ToTable("FilmProductions");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionActor", b =>
                {
                    b.Property<string>("FilmProductionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ActorId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FilmProductionId", "ActorId");

                    b.HasIndex("ActorId");

                    b.ToTable("FilmProductionActors");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionDirector", b =>
                {
                    b.Property<string>("FilmProductionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("DirectorId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FilmProductionId", "DirectorId");

                    b.HasIndex("DirectorId");

                    b.ToTable("FilmProductionDirectors");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionGenre", b =>
                {
                    b.Property<string>("FilmProductionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GenreId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FilmProductionId", "GenreId");

                    b.HasIndex("GenreId");

                    b.ToTable("FilmProductionGenres");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionScreenwriter", b =>
                {
                    b.Property<string>("FilmProductionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ScreenwriterId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("FilmProductionId", "ScreenwriterId");

                    b.HasIndex("ScreenwriterId");

                    b.ToTable("FilmProductionScreenwriters");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Genre", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Language", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Screenwriter", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(25)")
                        .HasMaxLength(25);

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Screenwriters");
                });

            modelBuilder.Entity("Data.DataModels.Entities.Subtitles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FilmProductionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FilmProductionId")
                        .IsUnique()
                        .HasFilter("[FilmProductionId] IS NOT NULL");

                    b.ToTable("Subtitles");
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProduction", b =>
                {
                    b.HasOne("Data.DataModels.Entities.Country", "Country")
                        .WithMany("FilmProductions")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Data.DataModels.Entities.Language", "Language")
                        .WithMany("FilmProductions")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionActor", b =>
                {
                    b.HasOne("Data.DataModels.Entities.Actor", "Actor")
                        .WithMany("FilmProductionActors")
                        .HasForeignKey("ActorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.DataModels.Entities.FilmProduction", "FilmProduction")
                        .WithMany("FilmProductionActors")
                        .HasForeignKey("FilmProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionDirector", b =>
                {
                    b.HasOne("Data.DataModels.Entities.Director", "Director")
                        .WithMany("FilmProductionDirectors")
                        .HasForeignKey("DirectorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.DataModels.Entities.FilmProduction", "FilmProduction")
                        .WithMany("FilmProductionDirectors")
                        .HasForeignKey("FilmProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionGenre", b =>
                {
                    b.HasOne("Data.DataModels.Entities.FilmProduction", "FilmProduction")
                        .WithMany("FilmProductionGenres")
                        .HasForeignKey("FilmProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.DataModels.Entities.Genre", "Genre")
                        .WithMany("FilmProductionGenres")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.DataModels.Entities.FilmProductionScreenwriter", b =>
                {
                    b.HasOne("Data.DataModels.Entities.FilmProduction", "FilmProduction")
                        .WithMany("FilmProductionScreenwriters")
                        .HasForeignKey("FilmProductionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.DataModels.Entities.Screenwriter", "Screenwriter")
                        .WithMany("FilmProductionScreenwriters")
                        .HasForeignKey("ScreenwriterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.DataModels.Entities.Subtitles", b =>
                {
                    b.HasOne("Data.DataModels.Entities.FilmProduction", "FilmProduction")
                        .WithOne("Subtitles")
                        .HasForeignKey("Data.DataModels.Entities.Subtitles", "FilmProductionId");
                });
#pragma warning restore 612, 618
        }
    }
}
