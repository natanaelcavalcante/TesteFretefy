using Fretefy.Test.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRegiaoCidadeRepository
    {
        Task<IEnumerable<RegiaoCidade>> ListByRegiaoIdAsync(Guid regiaoId);
        Task AddCidadeToRegiaoAsync(Guid regiaoId, Guid cidadeId);
        Task RemoveCidadeFromRegiaoAsync(Guid regiaoId, Guid cidadeId);
    }
}
