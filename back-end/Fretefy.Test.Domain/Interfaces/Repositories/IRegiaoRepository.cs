using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRegiaoRepository
    {
        Task<IEnumerable<Regiao>> ListAsync();
        Task<Regiao> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(Regiao regiao);
        Task<bool> UpdateAsync(Regiao regiao, IEnumerable<Guid> cidadesIds);
        Task<Regiao> GetByNameAsync(string name);
    }
}
