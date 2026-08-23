// Controllers/MarcasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MarcasController : ControllerBase
    {
        private readonly IMarcaService _service;

        public MarcasController(IMarcaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MarcaResponseDto>>> ObterTodas()
        {
            return Ok(await _service.ObterTodasAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MarcaResponseDto>> ObterPorId(int id)
        {
            var marca = await _service.ObterPorIdAsync(id);
            if (marca is null) return NotFound();
            return Ok(marca);
        }

        [HttpPost]
        public async Task<ActionResult<MarcaResponseDto>> Criar(MarcaCreateDto dto)
        {
            try
            {
                var marca = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = marca.Id }, marca);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MarcaResponseDto>> Atualizar(int id, MarcaUpdateDto dto)
        {
            try
            {
                return Ok(await _service.AtualizarAsync(id, dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return Conflict(new { mensagem = ex.Message }); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                await _service.InativarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }
    }
}