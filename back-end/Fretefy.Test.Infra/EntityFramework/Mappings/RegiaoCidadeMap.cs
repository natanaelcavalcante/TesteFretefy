using Fretefy.Test.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fretefy.Test.Infra.EntityFramework.Mappings
{
    public class RegiaoCidadeMap : IEntityTypeConfiguration<RegiaoCidade>
    {
        public void Configure(EntityTypeBuilder<RegiaoCidade> builder)
        {
            builder.HasKey(rc => rc.Id);

            builder.Property(rc => rc.Id)
                   .IsRequired();

            builder.HasOne(rc => rc.Regiao)
                   .WithMany(r => r.RegiaoCidade)
                   .HasForeignKey(rc => rc.RegiaoId);

            builder.HasOne(rc => rc.Cidade)
                   .WithMany()
                   .HasForeignKey(rc => rc.CidadeId);

            builder.ToTable("RegiaoCidade");
        }
    }
}