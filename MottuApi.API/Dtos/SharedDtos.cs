namespace MottuApi.Dtos
{
    public class PatioSimplificadoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class MotoSimplificadaDto
    {
        public int Id { get; set; }
        public string Placa { get; set; } = string.Empty;
    }

    public class MovimentacaoSimplificadaDto
    {
        public int Id { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public MotoSimplificadaDto? Moto { get; set; }
    }
}