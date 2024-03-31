using Fretefy.Test.Domain.DTOs;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fretefy.Test.WebApi.Controllers
{
    [ApiController]
    [Route("api/regiao")]
    public class RegiaoController : ControllerBase
    {
        private readonly IRegiaoService _regiaoService;

        public RegiaoController(IRegiaoService regiaoService)
        {
            _regiaoService = regiaoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var regiao = await _regiaoService.ListAsync();
            return Ok(regiao);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var regiao = await _regiaoService.GetByIdAsync(id);
            if (regiao == null)
            {
                return NotFound();
            }
            return Ok(regiao);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegiaoCreateDTO regiaoDto)
        {
            try
            {
                if (string.IsNullOrEmpty(regiaoDto.Nome) || !regiaoDto.CidadesId.Any())
                {
                    return BadRequest("Nome da região e ao menos uma cidade são obrigatórios.");
                }

                var regiao = new Regiao
                {
                    Nome = regiaoDto.Nome,
                    Ativo = true
                };

                foreach (var cidadeId in regiaoDto.CidadesId)
                {
                    regiao.RegiaoCidade.Add(new RegiaoCidade { CidadeId = cidadeId });
                }

                var created = await _regiaoService.CreateAsync(regiao);
                if (!created)
                {
                    return BadRequest("Não foi possível criar a região.");
                }

                return CreatedAtAction(nameof(Get), new { id = regiao.Id }, regiao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] RegiaoUpdateDTO regiaoUpdateDto)
        {
            if (regiaoUpdateDto == null || id != regiaoUpdateDto.Id || !regiaoUpdateDto.CidadesId.Any())
            {
                return BadRequest("Dados inválidos.");
            }

            var regiao = new Regiao
            {
                Id = regiaoUpdateDto.Id,
                Nome = regiaoUpdateDto.Nome,
                Ativo = regiaoUpdateDto.Ativo
            };

            var updated = await _regiaoService.UpdateAsync(regiao, regiaoUpdateDto.CidadesId);
            if (!updated)
            {
                return BadRequest("Não foi possível atualizar a região.");
            }

            return NoContent();
        }



        [HttpPut("{id}/toggle-active")]
        public async Task<IActionResult> ToggleActive(Guid id)
        {
            var result = await _regiaoService.ToggleActiveAsync(id);
            if (!result)
            {
                return BadRequest("Não foi possível alterar o estado da região.");
            }
            return NoContent();
        }
    }
}
