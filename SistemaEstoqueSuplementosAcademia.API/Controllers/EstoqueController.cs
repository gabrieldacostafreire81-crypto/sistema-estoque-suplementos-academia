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
            dto.UsuarioId = ObterUsuarioIdDoToken();
            try
            {
                return Ok(await _service.RegistrarEntradaAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpPost("saida")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarSaida(MovimentacaoEntradaSaidaDto dto)
        {
            dto.UsuarioId = ObterUsuarioIdDoToken();
            try
            {
                return Ok(await _service.RegistrarSaidaAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
            catch (InvalidOperationException ex) { return UnprocessableEntity(new { mensagem = ex.Message }); }
        }

        [HttpPost("ajuste")]
        public async Task<ActionResult<MovimentacaoResponseDto>> RegistrarAjuste(MovimentacaoAjusteDto dto)
        {
            dto.UsuarioId = ObterUsuarioIdDoToken();
            try
            {
                return Ok(await _service.RegistrarAjusteAsync(dto));
            }
            catch (KeyNotFoundException) { return NotFound(); }
        }

        [HttpGet("produto/{produtoId}/historico")]
        public async Task<ActionResult<IEnumerable<MovimentacaoResponseDto>>> ObterHistorico(int produtoId)
        {
            return Ok(await _service.ObterHistoricoPorProdutoAsync(produtoId));
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