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
    public class FilmProductionDirectorEntityTypeConfiguration: IEntityTypeConfiguration<FilmProductionDirector>
    {
        public void Configure(EntityTypeBuilder<FilmProductionDirector> entityTypeBuilder) 
        {
            entityTypeBuilder
                .HasKey(fpd => new { fpd.FilmProductionId, fpd.DirectorId });
            entityTypeBuilder
                .HasOne(fpd => fpd.FilmProduction)
                .WithMany(fp => fp.FilmProductionDirectors)
                .HasForeignKey(fpd => fpd.FilmProductionId);
            entityTypeBuilder
                .HasOne(fpd => fpd.Director)
                .WithMany(d => d.FilmProductionDirectors)
                .HasForeignKey(fpd => fpd.DirectorId);
        }
    }
}
