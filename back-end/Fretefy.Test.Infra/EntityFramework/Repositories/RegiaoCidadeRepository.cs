using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoCidadeRepository : IRegiaoCidadeRepository
    {
        private readonly DbContext _context;

        public RegiaoCidadeRepository(TestDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RegiaoCidade>> ListByRegiaoIdAsync(Guid regiaoId)
        {
            return await _context.Set<RegiaoCidade>()
                .Where(rc => rc.RegiaoId == regiaoId)
                .Include(rc => rc.Cidade)
                .ToListAsync();
        }

        public async Task AddCidadeToRegiaoAsync(Guid regiaoId, Guid cidadeId)
        {
            var alreadyExists = await _context.Set<RegiaoCidade>()
                .AnyAsync(rc => rc.RegiaoId == regiaoId && rc.CidadeId == cidadeId);

            if (!alreadyExists)
            {
                var regiaoCidade = new RegiaoCidade
                {
                    RegiaoId = regiaoId,
                    CidadeId = cidadeId
                };
                await _context.Set<RegiaoCidade>().AddAsync(regiaoCidade);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveCidadeFromRegiaoAsync(Guid regiaoId, Guid cidadeId)
        {
            var regiaoCidade = await _context.Set<RegiaoCidade>()
                .FirstOrDefaultAsync(rc => rc.RegiaoId == regiaoId && rc.CidadeId == cidadeId);
            if (regiaoCidade != null)
            {
                _context.Set<RegiaoCidade>().Remove(regiaoCidade);
                await _context.SaveChangesAsync();
            }
        }
    }
}
