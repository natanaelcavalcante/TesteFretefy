using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Fretefy.Test.Domain.Interfaces.Services;
using Fretefy.Test.Domain.DTOs;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Fretefy.Test.Domain.Services
{
    public class RegiaoService : IRegiaoService
    {
        private readonly IRegiaoRepository _regiaoRepository;

        public RegiaoService(IRegiaoRepository regiaoRepository)
        {
            _regiaoRepository = regiaoRepository;
        }

        public async Task<IEnumerable<Regiao>> ListAsync()
        {
            return await _regiaoRepository.ListAsync();
        }

        public async Task<IEnumerable<RegiaoDTO>> ListDtoAsync()
        {
            var regioes = await _regiaoRepository.ListAsync();

            var regioesDto = regioes.Select(regiao => new RegiaoDTO
            {
                Id = regiao.Id,
                Nome = regiao.Nome,
                Ativo = regiao.Ativo,
                CidadesIds = regiao.RegiaoCidade.Select(rc => rc.CidadeId).ToList()
            });

            return regioesDto;
        }

        public async Task<Regiao> GetByIdAsync(Guid id)
        {
            return await _regiaoRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(Regiao regiao)
        {
            if (string.IsNullOrWhiteSpace(regiao.Nome))
            {
                throw new Exception("O nome da região é obrigatório.");
            }

            var existingRegiao = await _regiaoRepository.GetByNameAsync(regiao.Nome);
            if (existingRegiao != null)
            {
                throw new Exception("Já existe uma região com este nome.");
            }

            if (regiao.RegiaoCidade == null || !regiao.RegiaoCidade.Any())
            {
                throw new Exception("Uma região deve ter ao menos uma cidade.");
            }

            if (regiao.RegiaoCidade.GroupBy(rc => rc.CidadeId).Any(g => g.Count() > 1))
            {
                throw new Exception("Não pode haver cidades duplicadas na região.");
            }

            return await _regiaoRepository.CreateAsync(regiao);
        }
        public async Task<bool> UpdateAsync(Regiao regiao, IEnumerable<Guid> cidadesIds)
        {
            var existingRegiao = await _regiaoRepository.GetByIdAsync(regiao.Id);
            if (existingRegiao == null)
            {
                throw new Exception("Região não encontrada.");
            }

            existingRegiao.Nome = regiao.Nome;
            existingRegiao.Ativo = regiao.Ativo;

            // Atualize a lista de cidades da região
            return await _regiaoRepository.UpdateAsync(existingRegiao, cidadesIds);
        }

        public async Task<bool> ToggleActiveAsync(Guid id)
        {
            var regiao = await _regiaoRepository.GetByIdAsync(id);
            if (regiao == null)
            {
                throw new Exception("Região não encontrada.");
            }

            regiao.Ativo = !regiao.Ativo;

            // Obter os IDs das cidades antes de chamar UpdateAsync
            var cidadesIds = regiao.RegiaoCidade.Select(rc => rc.CidadeId);

            await _regiaoRepository.UpdateAsync(regiao, cidadesIds);
            return true;
        }

    }
}
