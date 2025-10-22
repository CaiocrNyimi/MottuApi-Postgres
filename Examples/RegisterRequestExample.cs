using Swashbuckle.AspNetCore.Filters;
using MottuApi.Dtos;

namespace MottuApi.Examples
{
    public class RegisterRequestExample : IExamplesProvider<RegisterRequestDto>
    {
        public RegisterRequestDto GetExamples() => new RegisterRequestDto
        {
            Username = "admin",
            Senha = "12345"
        };
    }
}