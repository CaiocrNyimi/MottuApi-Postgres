using Swashbuckle.AspNetCore.Filters;
using MottuApi.Dtos;

namespace MottuApi.Examples
{
    public class MotoRequestExample : IExamplesProvider<MotoRequestDto>
    {
        public MotoRequestDto GetExamples() => new MotoRequestDto
        {
            Placa = "ABC1A34",
            Modelo = "Honda CG 160",
            Status = "Disponível"
        };
    }
}