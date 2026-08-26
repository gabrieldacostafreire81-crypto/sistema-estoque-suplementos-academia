// Controllers/EstoqueController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EstoqueController : ControllerBase
    {
        private readonly IEstoqueService _service;

        public EstoqueController(IEstoqueService service)
        {
            _service = service;
        }

        [HttpPost("entrada")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarEntrada(MovimentacaoEntradaSaidaDto dto)
        {
            try
            {
                dto.UsuarioId = ObterUsuarioIdDoToken();
                return Ok(await _service.RegistrarEntradaAsync(dto));
            }
            catch (UnauthorizedAccessException) { return Unauthorized(); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPost("saida")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarSaida(MovimentacaoEntradaSaidaDto dto)
        {
            try
            {
                dto.UsuarioId = ObterUsuarioIdDoToken();
                return Ok(await _service.RegistrarSaidaAsync(dto));
            }
            catch (UnauthorizedAccessException) { return Unauthorized(); }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { mensagem = ex.Message }); }
        }

        [HttpPost("ajuste")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarAjuste(MovimentacaoAjusteDto dto)
        {
            try
            {
                dto.UsuarioId = ObterUsuarioIdDoToken();
                return Ok(await _service.RegistrarAjusteAsync(dto));
            }
            catch (UnauthorizedAccessException) { return Unauthorized(); }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpGet("historico")]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> ObterHistorico(
            [FromQuery] int? produtoId, [FromQuery] DateTime? dataInicial, [FromQuery] DateTime? dataFinal)
        {
            return Ok(await _service.ObterHistoricoAsync(produtoId, dataInicial, dataFinal));
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