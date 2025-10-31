using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Placa da moto.
        /// </summary>
        [Required]
        [MaxLength(7)]
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// Modelo da moto.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; } = string.Empty;

        /// <summary>
        /// Status atual da moto (Ex: Disponível, Em Uso, Em Manutenção).
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Disponível";

        /// <summary>
        /// Chave estrangeira para o pátio onde a moto está estacionada.
        /// </summary>
        [ForeignKey("Patio")]
        public int? PatioId { get; set; }

        /// <summary>
        /// Pátio associado à moto.
        /// </summary>
        public Patio? Patio { get; set; }

        /// <summary>
        /// Data de entrada da moto no pátio.
        /// </summary>
        public DateTime? DataEntrada { get; set; }

        /// <summary>
        /// Movimentações relacionadas à moto.
        /// </summary>
        public ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
    }
}