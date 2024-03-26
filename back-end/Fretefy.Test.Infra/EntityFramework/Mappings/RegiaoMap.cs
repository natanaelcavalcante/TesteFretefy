using Fretefy.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fretefy.Test.Infra.EntityFramework.Mappings
{
       public class RegiaoMap : IEntityTypeConfiguration<Regiao>
       {
              public void Configure(EntityTypeBuilder<Regiao> builder)
              {
                     builder.HasKey(r => r.Id);

                     builder.Property(r => r.Id)
                            .IsRequired();

                     builder.Property(r => r.Nome)
                            .IsRequired()
                            .HasMaxLength(100);

                     builder.Property(r => r.Ativo)
                            .IsRequired();

                     builder.HasMany(r => r.RegiaoCidade)
                            .WithOne(rc => rc.Regiao)
                            .HasForeignKey(rc => rc.RegiaoId);

                     // Para buscas mais eficientes
                     builder.HasIndex(r => r.Nome).IsUnique();

                     builder.ToTable("Regiao");
              }
       }
}