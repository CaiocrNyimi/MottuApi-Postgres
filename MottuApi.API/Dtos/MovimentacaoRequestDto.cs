namespace MottuApi.Dtos
{
    public class MovimentacaoRequestDto
    {
        public int MotoId { get; set; }
        public int PatioId { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
    }
}