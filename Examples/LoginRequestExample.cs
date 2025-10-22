using Swashbuckle.AspNetCore.Filters;
using MottuApi.Dtos;

namespace MottuApi.Examples
{
    public class LoginRequestExample : IExamplesProvider<LoginRequestDto>
    {
        public LoginRequestDto GetExamples() => new LoginRequestDto
        {
            Username = "admin",
            Senha = "12345"
        };
    }
}