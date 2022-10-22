using Data.DataModels.Entities;
using Data.DataModels.Entities.Identity;
using Data.DataModels.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Subsogator.Common.GlobalConstants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<
            ApplicationUser, ApplicationRole, string, 
            IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, 
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        IConfigurationBuilder _configurationBuilder;

        IConfigurationRoot _configurationRoot;

        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
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
            dbContextOptionsBuilder.UseLazyLoadingProxies();

            if (!dbContextOptionsBuilder.IsConfigured)
            {
                string secretsJSONFullPath = ConnectionConstants.DatabaseConnectionString;
                _configurationBuilder = new ConfigurationBuilder()
                    .SetBasePath(Path.Join(secretsJSONFullPath))
                    .AddJsonFile(ConnectionConstants.SecretsJSONFileName);
                _configurationRoot = _configurationBuilder.Build();

                dbContextOptionsBuilder
                    .UseSqlServer(
                        _configurationRoot.GetSection(
                            ConnectionConstants.SecretsJSONConnectionStringSection
                    ).Value
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            ConfigureUserIdentityRelations(modelBuilder);
        }

        public override int SaveChanges()
        {
            return SaveChanges(true);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyEntityChanges();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyEntityChanges();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyEntityChanges()
        {
            var changeTrackerEntries = ChangeTracker.Entries()
               .Where(
                    e => e.Entity is IAuditInfo &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified
                ))
               .Select(e => e)
               .ToList();

            foreach (var changeTrackerEntry in changeTrackerEntries)
            {
                var auditableEntity = changeTrackerEntry.Entity as IAuditInfo;

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

        private void ConfigureUserIdentityRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
