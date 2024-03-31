using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly DbContext _context;
        private readonly ILogger<RegiaoRepository> _logger;

        public RegiaoRepository(TestDbContext context, ILogger<RegiaoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Regiao>> ListAsync()
        {
            return await _context.Set<Regiao>()
                         .Include(r => r.RegiaoCidade)
                         .ThenInclude(rc => rc.Cidade)
                         .ToListAsync();
        }

        public async Task<Regiao> GetByIdAsync(Guid id)
        {
            var regiao = await _context.Set<Regiao>()
            .Include(r => r.RegiaoCidade)
                .ThenInclude(rc => rc.Cidade)
            .FirstOrDefaultAsync(r => r.Id == id);

            return regiao;
        }

        public async Task<bool> CreateAsync(Regiao regiao)
        {
            if (await _context.Set<Regiao>().AnyAsync(r => r.Nome == regiao.Nome))
            {
                return false;
            }

            await _context.Set<Regiao>().AddAsync(regiao);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Regiao> GetByNameAsync(string name)
        {
            return await _context.Set<Regiao>()
                                 .FirstOrDefaultAsync(r => r.Nome == name);
        }

        public async Task<bool> UpdateAsync(Regiao regiao, IEnumerable<Guid> cidadesIds)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var regiaoExistente = await _context.Set<Regiao>()
                    .Include(r => r.RegiaoCidade)
                    .FirstOrDefaultAsync(r => r.Id == regiao.Id);

                if (regiaoExistente == null)
                {
                    throw new Exception("Região não encontrada.");
                }

                // Atualiza as propriedades da região
                regiaoExistente.Nome = regiao.Nome;
                regiaoExistente.Ativo = regiao.Ativo;

                var existingCidadesIds = regiaoExistente.RegiaoCidade.Select(rc => rc.CidadeId).ToList();

                var cidadesParaRemover = regiaoExistente.RegiaoCidade
                    .Where(rc => !cidadesIds.Contains(rc.CidadeId))
                    .ToList();

                foreach (var regiaoCidade in cidadesParaRemover)
                {
                    _context.Set<RegiaoCidade>().Remove(regiaoCidade);
                }

                var cidadesParaAdicionar = cidadesIds
                    .Except(existingCidadesIds)
                    .Select(cid => new RegiaoCidade { RegiaoId = regiaoExistente.Id, CidadeId = cid });

                foreach (var regiaoCidade in cidadesParaAdicionar)
                {
                    regiaoExistente.RegiaoCidade.Add(regiaoCidade);
                }

                _context.Set<Regiao>().Update(regiaoExistente);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



    }
}
