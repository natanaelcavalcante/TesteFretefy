using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.Domain.DTOs
{
    public class RegiaoCreateDTO
    {
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public List<Guid> CidadesId { get; set; } = new List<Guid>();
    }
}