// Application/Services/AuthService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Auth;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Enums;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

            if (usuario is null || !usuario.Ativo)
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            var senhaValida = _passwordHasher.VerificarSenha(dto.Senha, usuario.SenhaHash);
            if (!senhaValida)
                throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

            var token = _tokenService.GerarToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil.ToString()
            };
        }

        public async Task<LoginResponseDto> CriarUsuarioAsync(UsuarioCreateDto dto)
        {
            var emailJaExiste = await _usuarioRepository.ExisteComEmailAsync(dto.Email);
            if (emailJaExiste)
                throw new InvalidOperationException($"Já existe um usuário com o e-mail '{dto.Email}'.");

            if (!Enum.TryParse<PerfilUsuario>(dto.Perfil, ignoreCase: true, out var perfil))
                throw new InvalidOperationException(
                    $"Perfil '{dto.Perfil}' inválido. Valores aceitos: Administrador, Funcionario.");

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                SenhaHash = _passwordHasher.GerarHash(dto.Senha),
                Perfil = perfil,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };

            await _usuarioRepository.AdicionarAsync(usuario);
            await _unitOfWork.SalvarAsync();

            var token = _tokenService.GerarToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil.ToString()
            };
        }
    }
}
