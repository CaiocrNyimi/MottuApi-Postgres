using System.ComponentModel.DataAnnotations;

namespace MottuApi.Dtos
{
    public class PatioRequestDto
    {
        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Localizacao { get; set; } = string.Empty;
    }
}