// Controllers/VendasController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VendasController : ControllerBase
    {
        private readonly IVendaService _service;

        public VendasController(IVendaService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ActionResult<VendaResponseDto>> Criar(VendaCreateDto dto)
        {
            var usuarioId = ObterUsuarioIdDoToken();
            try
            {
                var venda = await _service.CriarAsync(dto, usuarioId);
                return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id }, venda);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { mensagem = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return UnprocessableEntity(new { mensagem = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VendaResponseDto>> ObterPorId(int id)
        {
            var venda = await _service.ObterPorIdAsync(id);
            if (venda is null) return NotFound();
            return Ok(venda);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendaResponseDto>>> ObterTodas()
        {
            return Ok(await _service.ObterTodasAsync());
        }

        private int ObterUsuarioIdDoToken()
        {
            var claim = User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (claim is null || !int.TryParse(claim.Value, out var usuarioId))
                throw new UnauthorizedAccessException("Token inválido ou sem identificação de usuário.");

            return usuarioId;
        }
    }
}