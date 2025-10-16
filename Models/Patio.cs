using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um pátio de estacionamento de motos.
    /// </summary>
    /// <example>
    /// {
    ///   "nome": "Pátio Central",
    ///   "localizacao": "Rua das Motos, 123"
    /// }
    /// </example>
    public class Patio
    {
        /// <summary>
        /// Identificador único do pátio.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do pátio.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Localização física do pátio.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Localizacao { get; set; } = string.Empty;

        /// <summary>
        /// Motos atualmente associadas a este pátio.
        /// </summary>
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();

        /// <summary>
        /// Movimentações relacionadas a este pátio.
        /// </summary>
        public ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
    }
}