using System.ComponentModel.DataAnnotations;

public class EntregaRequestDto
{
    [Required]
    public string Modelo { get; set; } = string.Empty;
    
    [Required]
    public float DistanciaKm { get; set; }
}