using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class RegiaoRepository : IRegiaoRepository
    {
        private readonly DbContext _context;

        public RegiaoRepository(TestDbContext context)
        {
            _context = context;
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
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var regiaoExistente = await _context.Set<Regiao>().FindAsync(regiao.Id);
                if (regiaoExistente == null)
                {
                    throw new Exception("Região não encontrada.");
                }

                regiaoExistente.Nome = regiao.Nome;
                regiaoExistente.Ativo = regiao.Ativo;
                _context.Set<Regiao>().Update(regiaoExistente);

                cidadesIds ??= Enumerable.Empty<Guid>();

                var regiaoCidadesAtuais = _context.Set<RegiaoCidade>().Where(rc => rc.RegiaoId == regiao.Id).ToList();

                var cidadesParaRemover = regiaoCidadesAtuais.Where(rc => !cidadesIds.Contains(rc.CidadeId)).ToList();
                _context.Set<RegiaoCidade>().RemoveRange(cidadesParaRemover);

                var cidadesAtuaisIds = regiaoCidadesAtuais.Select(rc => rc.CidadeId);
                var cidadesParaAdicionarIds = cidadesIds.Except(cidadesAtuaisIds);
                foreach (var cidadeId in cidadesParaAdicionarIds)
                {
                    var novaRegiaoCidade = new RegiaoCidade { RegiaoId = regiao.Id, CidadeId = cidadeId };
                    await _context.Set<RegiaoCidade>().AddAsync(novaRegiaoCidade);
                }

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
