// Application/Services/FornecedorService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Common;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Fornecedor;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class FornecedorService : IFornecedorService
    {
        private readonly IFornecedorRepository _repository;

        public FornecedorService(IFornecedorRepository repository)
        {
            _repository = repository;
        }

        public async Task<FornecedorResponseDto> CriarAsync(FornecedorCreateDto dto)
        {
            var cnpjJaExiste = await _repository.ExisteComCnpjAsync(dto.Cnpj);
            if (cnpjJaExiste)
                throw new InvalidOperationException($"Já existe um fornecedor com o CNPJ '{dto.Cnpj}'.");

            var fornecedor = new Fornecedor
            {
                Nome = dto.Nome,
                Cnpj = dto.Cnpj,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Ativo = true
            };

            await _repository.AdicionarAsync(fornecedor);
            await _repository.SalvarAsync();

            return MapearParaDto(fornecedor);
        }

        public async Task<FornecedorResponseDto> AtualizarAsync(int id, FornecedorUpdateDto dto)
        {
            var fornecedor = await _repository.ObterPorIdAsync(id);
            if (fornecedor is null)
                throw new KeyNotFoundException($"Fornecedor com Id {id} não encontrado.");

            var cnpjJaExisteEmOutro = await _repository.ExisteComCnpjAsync(dto.Cnpj, idParaIgnorar: id);
            if (cnpjJaExisteEmOutro)
                throw new InvalidOperationException($"Já existe outro fornecedor com o CNPJ '{dto.Cnpj}'.");

            fornecedor.Nome = dto.Nome;
            fornecedor.Cnpj = dto.Cnpj;
            fornecedor.Telefone = dto.Telefone;
            fornecedor.Email = dto.Email;

            _repository.Atualizar(fornecedor);
            await _repository.SalvarAsync();

            return MapearParaDto(fornecedor);
        }

        public async Task<FornecedorResponseDto?> ObterPorIdAsync(int id)
        {
            var fornecedor = await _repository.ObterPorIdAsync(id);
            return fornecedor is null ? null : MapearParaDto(fornecedor);
        }

        public async Task<PagedResultDto<FornecedorResponseDto>> ObterPaginadoAsync(FornecedorFiltroDto filtro)
        {
            var (itens, total) = await _repository.ObterPaginadoAsync(
                filtro.Nome, filtro.Ativo, filtro.Pagina, filtro.TamanhoPagina);

            return new PagedResultDto<FornecedorResponseDto>
            {
                Itens = itens.Select(MapearParaDto),
                TotalRegistros = total,
                Pagina = filtro.Pagina,
                TamanhoPagina = filtro.TamanhoPagina
            };
        }

        public async Task InativarAsync(int id)
        {
            var fornecedor = await _repository.ObterPorIdAsync(id);
            if (fornecedor is null)
                throw new KeyNotFoundException($"Fornecedor com Id {id} não encontrado.");

            fornecedor.Ativo = false;
            _repository.Atualizar(fornecedor);
            await _repository.SalvarAsync();
        }

        private static FornecedorResponseDto MapearParaDto(Fornecedor fornecedor)
        {
            return new FornecedorResponseDto
            {
                Id = fornecedor.Id,
                Nome = fornecedor.Nome,
                Cnpj = fornecedor.Cnpj,
                Telefone = fornecedor.Telefone,
                Email = fornecedor.Email,
                Ativo = fornecedor.Ativo
            };
        }
    }
}
