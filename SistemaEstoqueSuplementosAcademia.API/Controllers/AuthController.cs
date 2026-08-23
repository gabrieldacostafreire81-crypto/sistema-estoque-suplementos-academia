// Controllers/AuthController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Auth;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto dto)
        {
            try
            {
                return Ok(await _service.LoginAsync(dto));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { mensagem = ex.Message });
            }
        }

        [HttpPost("usuarios")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<LoginResponseDto>> CriarUsuario(UsuarioCreateDto dto)
        {
            try
            {
                var resultado = await _service.CriarUsuarioAsync(dto);
                return CreatedAtAction(nameof(Login), resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { mensagem = ex.Message });
            }
        }
    }
}
