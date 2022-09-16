using Data.DataModels.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
