﻿using Data.DataModels.Entities;
using Data.DataModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class ApplicationDbContext: DbContext
    {
        IConfigurationBuilder configurationBuilder;

        IConfigurationRoot configurationRoot;

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base() 
        {

        }

        public virtual DbSet<Actor> Actors { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Director> Directors { get; set; }

        public virtual DbSet<FilmProduction> FilmProductions { get; set; }

        public virtual DbSet<FilmProductionActor> FilmProductionActors { get; set; }

        public virtual DbSet<FilmProductionDirector> FilmProductionDirectors { get; set; }

        public virtual DbSet<FilmProductionGenre> FilmProductionGenres { get; set; }

        public virtual DbSet<FilmProductionScreenwriter> FilmProductionScreenwriters { get; set; }

        public virtual DbSet<Genre> Genres { get; set; }

        public virtual DbSet<Language> Languages { get; set; }

        public virtual DbSet<Screenwriter> Screenwriters { get; set; }

        public virtual DbSet<Subtitles> Subtitles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            string secretsJSONFullPath = ConnectionConstants.DatabaseConnectionString;
            this.configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Path.Join(secretsJSONFullPath))
                .AddJsonFile(ConnectionConstants.SecretsJSONFileName);
            this.configurationRoot = configurationBuilder.Build();
            dbContextOptionsBuilder
                .UseSqlServer(
                    configurationRoot.GetSection(ConnectionConstants.SecretsJSONConnectionStringSection).Value
                );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            return SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyEntityChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void ApplyEntityChanges()
        {
            var changeTrackerEntries = this.ChangeTracker.Entries()
               .Where(
                    x => x.Entity is IAuditInfo &&
                    (x.State == EntityState.Added || x.State == EntityState.Modified)
                );

            foreach (var changeTrackerEntry in changeTrackerEntries)
            {
                var auditableEntity = (IAuditInfo) changeTrackerEntry.Entity;

                switch (changeTrackerEntry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreatedOn = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        auditableEntity.ModifiedOn = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
