using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.DataAccess.EntityTypeConfigurations
{
    public class FilmProductionActorEntityTypeConfiguration: IEntityTypeConfiguration<FilmProductionActor>
    {
        public void Configure(EntityTypeBuilder<FilmProductionActor> entityTypeBuilder) 
        {
            entityTypeBuilder
                .HasKey(fpa => new { fpa.FilmProductionId, fpa.ActorId });
            entityTypeBuilder
                .HasOne(fpa => fpa.FilmProduction)
                .WithMany(fp => fp.FilmProductionActors)
                .HasForeignKey(fpa => fpa.FilmProductionId);
            entityTypeBuilder
                .HasOne(fpa => fpa.Actor)
                .WithMany(a => a.FilmProductionActors)
                .HasForeignKey(fpa => fpa.ActorId);
        }
    }
}
