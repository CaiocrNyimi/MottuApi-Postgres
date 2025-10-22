using System.ComponentModel.DataAnnotations;

namespace MottuApi.Dtos
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Senha { get; set; } = string.Empty;
    }
}