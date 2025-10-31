namespace MottuApi.Dtos
{
    public class MotoResponseDto
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public DateTime? DataEntrada { get; set; }
        public PatioSimplificadoDto? Patio { get; set; }
    }
}