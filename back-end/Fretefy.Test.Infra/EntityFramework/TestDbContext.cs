using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Infra.EntityFramework.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Fretefy.Test.Infra.EntityFramework
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Regiao> Regiao { get; set; }
        public DbSet<RegiaoCidade> RegiaoCidade { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeamentos existentes
            modelBuilder.ApplyConfiguration(new CidadeMap());

            // Novos mapeamentos criados
            modelBuilder.ApplyConfiguration(new RegiaoMap());
            modelBuilder.ApplyConfiguration(new RegiaoCidadeMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
