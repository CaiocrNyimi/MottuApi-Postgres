using System.ComponentModel.DataAnnotations;

namespace MottuApi.Dtos
{
    public class MotoRequestDto
    {
        [Required]
        [MaxLength(7)]
        [RegularExpression(@"^[A-Z]{3}[0-9]{4}$|^[A-Z]{3}[0-9]{1}[A-Z]{1}[0-9]{2}$", ErrorMessage = "Placa inválida. Use o padrão ABC1234 ou Mercosul (ABC1D23).")]
        public string Placa { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = string.Empty;

        public int? PatioId { get; set; }
        public DateTime? DataEntrada { get; set; }
    }
}