using System;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa o registro de entrada e saída de uma moto em um pátio.
    /// </summary>
    public class Movimentacao
    {
        /// <summary>
        /// Identificador único da movimentação.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identificador da moto relacionada.
        /// </summary>
        public int MotoId { get; set; }

        /// <summary>
        /// Identificador do pátio relacionado.
        /// </summary>
        public int PatioId { get; set; }

        /// <summary>
        /// Data e hora de entrada da moto no pátio.
        /// </summary>
        public DateTime DataEntrada { get; set; }

        /// <summary>
        /// Data e hora de saída da moto do pátio (opcional).
        /// </summary>
        public DateTime? DataSaida { get; set; }

        /// <summary>
        /// Moto relacionada à movimentação.
        /// </summary>
        public Moto? Moto { get; set; }

        /// <summary>
        /// Pátio relacionado à movimentação.
        /// </summary>
        public Patio? Patio { get; set; }
    }
}