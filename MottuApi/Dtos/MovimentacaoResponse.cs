namespace MottuApi.Dtos
{
    public class MovimentacaoResponseDto
    {
        public int Id { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }

        public MotoSimplificadaDto Moto { get; set; } = new();
        public PatioSimplificadoDto Patio { get; set; } = new();
    }
}