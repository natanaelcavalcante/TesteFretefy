using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class RegiaoUpdateDTO
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public bool Ativo { get; set; }
    public List<Guid> CidadesIds { get; set; }
}

