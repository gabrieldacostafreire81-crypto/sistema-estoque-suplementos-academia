// Application/Services/EstoqueService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Estoque;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Enums;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EstoqueService(
            IProdutoRepository produtoRepository,
            IMovimentacaoEstoqueRepository movimentacaoRepository,
            IUnitOfWork unitOfWork)
        {
            _produtoRepository = produtoRepository;
            _movimentacaoRepository = movimentacaoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<MovimentacaoResponseDto> RegistrarEntradaAsync(MovimentacaoEntradaSaidaDto dto)
        {
            var produto = await ObterProdutoOuFalharAsync(dto.ProdutoId);

            produto.EstoqueAtual += dto.Quantidade;

            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                UsuarioId = dto.UsuarioId,
                Tipo = TipoMovimentacaoEstoque.Entrada,
                Quantidade = dto.Quantidade,
                Motivo = null,
                DataHora = DateTime.UtcNow
            };

            return await PersistirMovimentacaoAsync(produto, movimentacao);
        }

        public async Task<MovimentacaoResponseDto> RegistrarSaidaAsync(MovimentacaoEntradaSaidaDto dto)
        {
            var produto = await ObterProdutoOuFalharAsync(dto.ProdutoId);

            if (produto.EstoqueAtual - dto.Quantidade < 0)
                throw new InvalidOperationException(
                    $"Estoque insuficiente. Disponível: {produto.EstoqueAtual}, solicitado: {dto.Quantidade}.");

            produto.EstoqueAtual -= dto.Quantidade;

            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                UsuarioId = dto.UsuarioId,
                Tipo = TipoMovimentacaoEstoque.Saida,
                Quantidade = dto.Quantidade,
                Motivo = null,
                DataHora = DateTime.UtcNow
            };

            return await PersistirMovimentacaoAsync(produto, movimentacao);
        }

        public async Task<MovimentacaoResponseDto> RegistrarAjusteAsync(MovimentacaoAjusteDto dto)
        {
            var produto = await ObterProdutoOuFalharAsync(dto.ProdutoId);

            var diferenca = dto.NovaQuantidade - produto.EstoqueAtual;
            produto.EstoqueAtual = dto.NovaQuantidade;

            var movimentacao = new MovimentacaoEstoque
            {
                ProdutoId = produto.Id,
                UsuarioId = dto.UsuarioId,
                Tipo = TipoMovimentacaoEstoque.Ajuste,
                Quantidade = Math.Abs(diferenca),
                Motivo = dto.Motivo,
                DataHora = DateTime.UtcNow
            };

            return await PersistirMovimentacaoAsync(produto, movimentacao);
        }

        public async Task<IEnumerable<MovimentacaoResponseDto>> ObterHistoricoPorProdutoAsync(int produtoId)
        {
            var movimentacoes = await _movimentacaoRepository.ObterPorProdutoAsync(produtoId);
            return movimentacoes.Select(m => MapearParaDto(m));
        }

        private async Task<Produto> ObterProdutoOuFalharAsync(int produtoId)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(produtoId);
            if (produto is null)
                throw new KeyNotFoundException($"Produto com Id {produtoId} não encontrado.");

            return produto;
        }

        private async Task<MovimentacaoResponseDto> PersistirMovimentacaoAsync(
            Produto produto, MovimentacaoEstoque movimentacao)
        {
            _produtoRepository.Atualizar(produto);
            await _movimentacaoRepository.AdicionarAsync(movimentacao);

            await _unitOfWork.SalvarAsync();

            movimentacao.Produto = produto;
            return MapearParaDto(movimentacao, produto.EstoqueAtual);
        }

        private static MovimentacaoResponseDto MapearParaDto(MovimentacaoEstoque m, int? estoqueResultanteForcado = null)
        {
            return new MovimentacaoResponseDto
            {
                Id = m.Id,
                ProdutoId = m.ProdutoId,
                ProdutoNome = m.Produto?.Nome ?? string.Empty,
                UsuarioId = m.UsuarioId,
                UsuarioNome = m.Usuario?.Nome ?? string.Empty,
                Tipo = m.Tipo.ToString(),
                Quantidade = m.Quantidade,
                Motivo = m.Motivo,
                DataHora = m.DataHora,
                EstoqueResultante = estoqueResultanteForcado ?? 0
            };
        }
    }
}
