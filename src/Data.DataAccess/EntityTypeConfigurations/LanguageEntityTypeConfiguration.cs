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
    public class LanguageEntityTypeConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> entityTypeBuilder)
        {
            entityTypeBuilder
               .HasMany(l => l.FilmProductions)
               .WithOne(fp => fp.Language)
               .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
