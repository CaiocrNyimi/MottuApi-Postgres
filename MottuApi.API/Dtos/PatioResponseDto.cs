namespace MottuApi.Dtos
{
    public class PatioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;

        public List<MotoSimplificadaDto> Motos { get; set; } = new();
        public List<MovimentacaoSimplificadaDto> Movimentacoes { get; set; } = new();
    }
}