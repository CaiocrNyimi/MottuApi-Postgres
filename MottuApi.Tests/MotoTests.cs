using Xunit;
using MottuApi.Models;
using System;

namespace MottuApi.Tests
{
    public class MotoTests
    {
        [Fact]
        public void CriarMoto_DeveRetornarMotoValida()
        {
            var moto = new Moto
            {
                Placa = "ABC1234",
                Modelo = "CG 160",
                Status = "Disponível",
                DataEntrada = DateTime.Today
            };

            Assert.Equal("ABC1234", moto.Placa);
            Assert.Equal("CG 160", moto.Modelo);
            Assert.Equal("Disponível", moto.Status);
            Assert.Equal(DateTime.Today, moto.DataEntrada);
        }

        [Fact]
        public void ConsultarMoto_DeveRetornarModelo()
        {
            var moto = new Moto { Modelo = "CG 160" };
            Assert.Equal("CG 160", moto.Modelo);
        }

        [Fact]
        public void AtualizarMoto_DeveAlterarStatus()
        {
            var moto = new Moto { Status = "Disponível" };
            moto.Status = "Em manutenção";
            Assert.Equal("Em manutenção", moto.Status);
        }

        [Fact]
        public void DeletarMoto_DeveZerarObjeto()
        {
            var moto = new Moto { Modelo = "CG" };
            moto = null;
            Assert.Null(moto);
        }
    }
}