// Application/Services/VendaService.cs
using SistemaEstoqueSuplementosAcademia.Application.DTOs.Venda;
using SistemaEstoqueSuplementosAcademia.Application.Interfaces;
using SistemaEstoqueSuplementosAcademia.Domain.Entities;
using SistemaEstoqueSuplementosAcademia.Domain.Enums;
using SistemaEstoqueSuplementosAcademia.Domain.Interfaces;

namespace SistemaEstoqueSuplementosAcademia.Application.Services
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VendaService(
            IVendaRepository vendaRepository,
            IProdutoRepository produtoRepository,
            IMovimentacaoEstoqueRepository movimentacaoRepository,
            IUnitOfWork unitOfWork)
        {
            _vendaRepository = vendaRepository;
            _produtoRepository = produtoRepository;
            _movimentacaoRepository = movimentacaoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<VendaResponseDto> CriarAsync(VendaCreateDto dto, int usuarioId)
        {
            var venda = new Venda
            {
                UsuarioId = usuarioId,
                DataHora = DateTime.UtcNow,
                FormaPagamento = dto.FormaPagamento,
                Status = "Concluida"
            };

            decimal valorTotal = 0;

            foreach (var itemDto in dto.Itens)
            {
                var produto = await _produtoRepository.ObterPorIdAsync(itemDto.ProdutoId);
                if (produto is null)
                    throw new KeyNotFoundException($"Produto com Id {itemDto.ProdutoId} não encontrado.");

                if (produto.EstoqueAtual < itemDto.Quantidade)
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.EstoqueAtual}, solicitado: {itemDto.Quantidade}.");

                produto.EstoqueAtual -= itemDto.Quantidade;
                _produtoRepository.Atualizar(produto);

                var subtotal = produto.PrecoVenda * itemDto.Quantidade;
                valorTotal += subtotal;

                venda.Itens.Add(new ItemVenda
                {
                    ProdutoId = produto.Id,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitarioNaVenda = produto.PrecoVenda,
                    Subtotal = subtotal
                });

                await _movimentacaoRepository.AdicionarAsync(new MovimentacaoEstoque
                {
                    ProdutoId = produto.Id,
                    UsuarioId = usuarioId,
                    Tipo = TipoMovimentacaoEstoque.Saida,
                    Quantidade = itemDto.Quantidade,
                    Motivo = null,
                    DataHora = DateTime.UtcNow
                });
            }

            venda.ValorTotal = valorTotal;

            await _vendaRepository.AdicionarAsync(venda);
            await _unitOfWork.SalvarAsync();

            var vendaCompleta = await _vendaRepository.ObterPorIdAsync(venda.Id);
            return MapearParaDto(vendaCompleta!);
        }

        public async Task<VendaResponseDto?> ObterPorIdAsync(int id)
        {
            var venda = await _vendaRepository.ObterPorIdAsync(id);
            return venda is null ? null : MapearParaDto(venda);
        }

        public async Task<IEnumerable<VendaResponseDto>> ObterTodasAsync()
        {
            var vendas = await _vendaRepository.ObterTodasAsync();
            return vendas.Select(MapearParaDto);
        }

        private static VendaResponseDto MapearParaDto(Venda venda)
        {
            return new VendaResponseDto
            {
                Id = venda.Id,
                UsuarioId = venda.UsuarioId,
                UsuarioNome = venda.Usuario?.Nome ?? string.Empty,
                DataHora = venda.DataHora,
                ValorTotal = venda.ValorTotal,
                FormaPagamento = venda.FormaPagamento,
                Status = venda.Status,
                Itens = venda.Itens.Select(i => new VendaItemResponseDto
                {
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto?.Nome ?? string.Empty,
                    Quantidade = i.Quantidade,
                    PrecoUnitarioNaVenda = i.PrecoUnitarioNaVenda,
                    Subtotal = i.Subtotal
                }).ToList()
            };
        }
    }
}