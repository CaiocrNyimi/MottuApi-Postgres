using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Identificador da moto relacionada.
        /// </summary>
        [ForeignKey("Moto")]
        public int MotoId { get; set; }

        /// <summary>
        /// Moto relacionada à movimentação.
        /// </summary>
        public Moto Moto { get; set; } = null!;

        /// <summary>
        /// Identificador do pátio relacionado.
        /// </summary>
        [ForeignKey("Patio")]
        public int PatioId { get; set; }

        /// <summary>
        /// Pátio relacionado à movimentação.
        /// </summary>
        public Patio Patio { get; set; } = null!;

        /// <summary>
        /// Data e hora de entrada da moto no pátio.
        /// </summary>
        public DateTime DataEntrada { get; set; } = DateTime.Now;

        /// <summary>
        /// Data e hora de saída da moto do pátio (opcional).
        /// </summary>
        public DateTime? DataSaida { get; set; }
    }
}