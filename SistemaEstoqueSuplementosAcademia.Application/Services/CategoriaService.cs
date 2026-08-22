using SistemaEstoqueSuplementosAcademia.Application.DTOs.Categoria;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoriaResponseDto> CriarAsync(CategoriaCreateDto dto)
        {
            var nomeJaExiste = await _repository.ExisteComNomeAsync(dto.Nome);
            if (nomeJaExiste)
                throw new InvalidOperationException($"Já existe uma categoria com o nome '{dto.Nome}'.");

            var categoria = new Categoria
            {
                Nome = dto.Nome,
                Ativo = true
            };

            await _repository.AdicionarAsync(categoria);
            await _repository.SalvarAsync();

            return MapearParaDto(categoria);
        }

        public async Task<CategoriaResponseDto> AtualizarAsync(int id, CategoriaUpdateDto dto)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");

            var nomeJaExisteEmOutra = await _repository.ExisteComNomeAsync(dto.Nome, idParaIgnorar: id);
            if (nomeJaExisteEmOutra)
                throw new InvalidOperationException($"Já existe outra categoria com o nome '{dto.Nome}'.");

            categoria.Nome = dto.Nome;
            _repository.Atualizar(categoria);
            await _repository.SalvarAsync();

            return MapearParaDto(categoria);
        }

        public async Task<CategoriaResponseDto?> ObterPorIdAsync(int id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            return categoria is null ? null : MapearParaDto(categoria);
        }

        public async Task<IEnumerable<CategoriaResponseDto>> ObterTodasAsync()
        {
            var categorias = await _repository.ObterTodasAsync();
            return categorias.Select(MapearParaDto);
        }

        public async Task InativarAsync(int id)
        {
            var categoria = await _repository.ObterPorIdAsync(id);
            if (categoria is null)
                throw new KeyNotFoundException($"Categoria com Id {id} não encontrada.");

            categoria.Ativo = false;
            _repository.Atualizar(categoria);
            await _repository.SalvarAsync();
        }

        private static CategoriaResponseDto MapearParaDto(Categoria categoria)
        {
            return new CategoriaResponseDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Ativo = categoria.Ativo
            };
        }
    }
}
