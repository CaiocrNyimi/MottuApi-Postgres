using Swashbuckle.AspNetCore.Filters;
using MottuApi.Dtos;

namespace MottuApi.Examples
{
    public class MovimentacaoRequestExample : IExamplesProvider<MovimentacaoRequestDto>
    {
        public MovimentacaoRequestDto GetExamples() => new MovimentacaoRequestDto
        {
            MotoId = 1,
            PatioId = 1,
            DataEntrada = DateTime.Parse("2025-10-16T09:00:00")
        };
    }
}