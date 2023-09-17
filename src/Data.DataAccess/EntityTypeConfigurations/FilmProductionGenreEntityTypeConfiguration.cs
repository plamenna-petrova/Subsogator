using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.DataAccess.EntityTypeConfigurations
{
    public class FilmProductionGenreEntityTypeConfiguration: IEntityTypeConfiguration<FilmProductionGenre>
    {
        public void Configure(EntityTypeBuilder<FilmProductionGenre> entityTypeBuilder) 
        {
            entityTypeBuilder
                .HasKey(fpg => new { fpg.FilmProductionId, fpg.GenreId });
            entityTypeBuilder
                .HasOne(fpg => fpg.FilmProduction)
                .WithMany(fp => fp.FilmProductionGenres)
                .HasForeignKey(fpg => fpg.FilmProductionId);
            entityTypeBuilder
                .HasOne(fpg => fpg.Genre)
                .WithMany(g => g.FilmProductionGenres)
                .HasForeignKey(fpg => fpg.GenreId);
        }
    }
}
