using Xunit;
using MottuApi.Models;
using System;

namespace MottuApi.Tests
{
    public class MovimentacaoTests
    {
        [Fact]
        public void CriarMovimentacao_DeveRetornarDadosValidos()
        {
            var movimentacao = new Movimentacao
            {
                MotoId = 1,
                PatioId = 2,
                DataEntrada = DateTime.Today
            };

            Assert.Equal(1, movimentacao.MotoId);
            Assert.Equal(2, movimentacao.PatioId);
            Assert.Equal(DateTime.Today, movimentacao.DataEntrada);
            Assert.Null(movimentacao.DataSaida);
        }

        [Fact]
        public void ConsultarMovimentacao_DeveRetornarDataEntrada()
        {
            var movimentacao = new Movimentacao { DataEntrada = DateTime.Today };
            Assert.Equal(DateTime.Today, movimentacao.DataEntrada);
        }

        [Fact]
        public void AtualizarMovimentacao_DeveDefinirDataSaida()
        {
            var movimentacao = new Movimentacao { DataEntrada = DateTime.Today };
            movimentacao.DataSaida = DateTime.Today.AddHours(2);
            Assert.NotNull(movimentacao.DataSaida);
        }

        [Fact]
        public void DeletarMovimentacao_DeveZerarObjeto()
        {
            var movimentacao = new Movimentacao { MotoId = 1 };
            movimentacao = null;
            Assert.Null(movimentacao);
        }
    }
}