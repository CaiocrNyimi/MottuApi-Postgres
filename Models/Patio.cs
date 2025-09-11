using System.Collections.Generic;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um pátio de estacionamento de motos.
    /// </summary>
    public class Patio
    {
        /// <summary>
        /// Identificador único do pátio.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do pátio.
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Localização física do pátio.
        /// </summary>
        public string Localizacao { get; set; } = string.Empty;

        /// <summary>
        /// Movimentações relacionadas a este pátio.
        /// </summary>
        public ICollection<Movimentacao>? Movimentacoes { get; set; }
    }
}