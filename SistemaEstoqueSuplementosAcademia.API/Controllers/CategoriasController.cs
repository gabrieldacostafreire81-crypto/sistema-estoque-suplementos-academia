using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
   
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriasController(ICategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponseDto>>> ObterTodas()
        {
            var categorias = await _service.ObterTodasAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> ObterPorId(int id)
        {
            var categoria = await _service.ObterPorIdAsync(id);
            if (categoria is null)
                return NotFound();

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaResponseDto>> Criar(CategoriaCreateDto dto)
        {
            try
            {
                var categoria = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = categoria.Id }, categoria);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoriaResponseDto>> Atualizar(int id, CategoriaUpdateDto dto)
        {
            try
            {
                var categoria = await _service.AtualizarAsync(id, dto);
                return Ok(categoria);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Inativar(int id)
        {
            try
            {
                await _service.InativarAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
