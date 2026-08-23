// Application/Services/ProdutoService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Common;
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Produto;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        public ProdutoService(
            IProdutoRepository produtoRepository,
            ICategoriaRepository categoriaRepository,
            IMarcaRepository marcaRepository,
            IFornecedorRepository fornecedorRepository)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
            _marcaRepository = marcaRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        public async Task<ProdutoResponseDto> CriarAsync(ProdutoCreateDto dto)
        {
            await ValidarReferenciasAsync(dto.CategoriaId, dto.MarcaId, dto.FornecedorId);

            var produto = new Produto
            {
                Nome = dto.Nome,
                CategoriaId = dto.CategoriaId,
                MarcaId = dto.MarcaId,
                FornecedorId = dto.FornecedorId,
                PrecoCompra = dto.PrecoCompra,
                PrecoVenda = dto.PrecoVenda,
                EstoqueAtual = 0,
                EstoqueMinimo = dto.EstoqueMinimo,
                Ativo = true
            };

            await _produtoRepository.AdicionarAsync(produto);
            await _produtoRepository.SalvarAsync();

            var produtoCompleto = await _produtoRepository.ObterPorIdAsync(produto.Id);
            return MapearParaDto(produtoCompleto!);
        }

        public async Task<ProdutoResponseDto> AtualizarAsync(int id, ProdutoUpdateDto dto)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto is null)
                throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

            await ValidarReferenciasAsync(dto.CategoriaId, dto.MarcaId, dto.FornecedorId);

            produto.Nome = dto.Nome;
            produto.CategoriaId = dto.CategoriaId;
            produto.MarcaId = dto.MarcaId;
            produto.FornecedorId = dto.FornecedorId;
            produto.PrecoCompra = dto.PrecoCompra;
            produto.PrecoVenda = dto.PrecoVenda;
            produto.EstoqueMinimo = dto.EstoqueMinimo;

            _produtoRepository.Atualizar(produto);
            await _produtoRepository.SalvarAsync();

            var produtoAtualizado = await _produtoRepository.ObterPorIdAsync(id);
            return MapearParaDto(produtoAtualizado!);
        }

        public async Task<ProdutoResponseDto?> ObterPorIdAsync(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            return produto is null ? null : MapearParaDto(produto);
        }

        public async Task<PagedResultDto<ProdutoResponseDto>> ObterPaginadoAsync(ProdutoFiltroDto filtro)
        {
            var (itens, total) = await _produtoRepository.ObterPaginadoAsync(
                filtro.Nome, filtro.CategoriaId, filtro.Ativo, filtro.Pagina, filtro.TamanhoPagina);

            return new PagedResultDto<ProdutoResponseDto>
            {
                Itens = itens.Select(MapearParaDto),
                TotalRegistros = total,
                Pagina = filtro.Pagina,
                TamanhoPagina = filtro.TamanhoPagina
            };
        }

        public async Task InativarAsync(int id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(id);
            if (produto is null)
                throw new KeyNotFoundException($"Produto com Id {id} não encontrado.");

            produto.Ativo = false;
            _produtoRepository.Atualizar(produto);
            await _produtoRepository.SalvarAsync();
        }

        private async Task ValidarReferenciasAsync(int categoriaId, int marcaId, int fornecedorId)
        {
            if (await _categoriaRepository.ObterPorIdAsync(categoriaId) is null)
                throw new InvalidOperationException($"Categoria com Id {categoriaId} não existe.");

            if (await _marcaRepository.ObterPorIdAsync(marcaId) is null)
                throw new InvalidOperationException($"Marca com Id {marcaId} não existe.");

            if (await _fornecedorRepository.ObterPorIdAsync(fornecedorId) is null)
                throw new InvalidOperationException($"Fornecedor com Id {fornecedorId} não existe.");
        }

        private static ProdutoResponseDto MapearParaDto(Produto produto)
        {
            return new ProdutoResponseDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                CategoriaId = produto.CategoriaId,
                CategoriaNome = produto.Categoria?.Nome ?? string.Empty,
                MarcaId = produto.MarcaId,
                MarcaNome = produto.Marca?.Nome ?? string.Empty,
                FornecedorId = produto.FornecedorId,
                FornecedorNome = produto.Fornecedor?.Nome ?? string.Empty,
                PrecoCompra = produto.PrecoCompra,
                PrecoVenda = produto.PrecoVenda,
                EstoqueAtual = produto.EstoqueAtual,
                EstoqueMinimo = produto.EstoqueMinimo,
                EstoqueBaixo = produto.EstoqueAtual <= produto.EstoqueMinimo,
                Ativo = produto.Ativo
            };
        }
    }
}
