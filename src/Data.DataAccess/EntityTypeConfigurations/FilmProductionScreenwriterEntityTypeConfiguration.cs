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
    public class FilmProductionScreenwriterEntityTypeConfiguration : IEntityTypeConfiguration<FilmProductionScreenwriter>
    {
        public void Configure(EntityTypeBuilder<FilmProductionScreenwriter> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(fps => new { fps.FilmProductionId, fps.ScreenwriterId });
            entityTypeBuilder
                .HasOne(fps => fps.FilmProduction)
                .WithMany(fp => fp.FilmProductionScreenwriters)
                .HasForeignKey(fps => fps.FilmProductionId);
            entityTypeBuilder
                .HasOne(fps => fps.Screenwriter)
                .WithMany(s => s.FilmProductionScreenwriters)
                .HasForeignKey(fps => fps.ScreenwriterId);
        }
    }
}
