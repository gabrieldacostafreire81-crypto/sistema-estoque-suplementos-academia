// Application/Services/MarcaService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Marca;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class MarcaService : IMarcaService
    {
        private readonly IMarcaRepository _repository;

        public MarcaService(IMarcaRepository repository)
        {
            _repository = repository;
        }

        public async Task<MarcaResponseDto> CriarAsync(MarcaCreateDto dto)
        {
            var nomeJaExiste = await _repository.ExisteComNomeAsync(dto.Nome);
            if (nomeJaExiste)
                throw new InvalidOperationException($"Já existe uma marca com o nome '{dto.Nome}'.");

            var marca = new Marca { Nome = dto.Nome, Ativo = true };

            await _repository.AdicionarAsync(marca);
            await _repository.SalvarAsync();

            return MapearParaDto(marca);
        }

        public async Task<MarcaResponseDto> AtualizarAsync(int id, MarcaUpdateDto dto)
        {
            var marca = await _repository.ObterPorIdAsync(id);
            if (marca is null)
                throw new KeyNotFoundException($"Marca com Id {id} não encontrada.");

            var nomeJaExisteEmOutra = await _repository.ExisteComNomeAsync(dto.Nome, idParaIgnorar: id);
            if (nomeJaExisteEmOutra)
                throw new InvalidOperationException($"Já existe outra marca com o nome '{dto.Nome}'.");

            marca.Nome = dto.Nome;
            _repository.Atualizar(marca);
            await _repository.SalvarAsync();

            return MapearParaDto(marca);
        }

        public async Task<MarcaResponseDto?> ObterPorIdAsync(int id)
        {
            var marca = await _repository.ObterPorIdAsync(id);
            return marca is null ? null : MapearParaDto(marca);
        }

        public async Task<IEnumerable<MarcaResponseDto>> ObterTodasAsync()
        {
            var marcas = await _repository.ObterTodasAsync();
            return marcas.Select(MapearParaDto);
        }

        public async Task InativarAsync(int id)
        {
            var marca = await _repository.ObterPorIdAsync(id);
            if (marca is null)
                throw new KeyNotFoundException($"Marca com Id {id} não encontrada.");

            marca.Ativo = false;
            _repository.Atualizar(marca);
            await _repository.SalvarAsync();
        }

        private static MarcaResponseDto MapearParaDto(Marca marca)
        {
            return new MarcaResponseDto
            {
                Id = marca.Id,
                Nome = marca.Nome,
                Ativo = marca.Ativo
            };
        }
    }
}