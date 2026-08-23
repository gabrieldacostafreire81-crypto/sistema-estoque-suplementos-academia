// Controllers/FornecedoresController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
   
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedorService _service;

        public FornecedoresController(IFornecedorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> ObterPaginado([FromQuery] FornecedorFiltroDto filtro)
        {
            return Ok(await _service.ObterPaginadoAsync(filtro));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FornecedorResponseDto>> ObterPorId(int id)
        {
            var fornecedor = await _service.ObterPorIdAsync(id);
            if (fornecedor is null) return NotFound();
            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorResponseDto>> Criar(FornecedorCreateDto dto)
        {
            try
            {
                var fornecedor = await _service.CriarAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = fornecedor.Id }, fornecedor);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FornecedorResponseDto>> Atualizar(int id, FornecedorUpdateDto dto)
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
