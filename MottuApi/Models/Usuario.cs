using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottuApi.Models
{
    /// <summary>
    /// Representa um usuário autenticável no sistema.
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Nome de usuário utilizado para login.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SenhaHash { get; set; } = string.Empty;
    }
}