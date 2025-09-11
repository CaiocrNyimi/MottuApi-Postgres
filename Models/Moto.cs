using System;
using System.Collections.Generic;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa uma moto estacionada no sistema.
    /// </summary>
    public class Moto
    {
        /// <summary>
        /// Identificador único da moto.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Placa da moto.
        /// </summary>
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// Status atual da moto (Ex: Disponível, Em Uso, Em Manutenção).
        /// </summary>
        public string Status { get; set; } = "Disponível";

        /// <summary>
        /// Nome do pátio onde a moto está estacionada.
        /// </summary>
        public string Patio { get; set; } = string.Empty;

        /// <summary>
        /// Data de entrada da moto no pátio.
        /// </summary>
        public DateTime DataEntrada { get; set; } = DateTime.Now;

        /// <summary>
        /// Data de saída da moto do pátio (opcional).
        /// </summary>
        public DateTime? DataSaida { get; set; }

        /// <summary>
        /// Movimentações relacionadas à moto.
        /// </summary>
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
    }
}