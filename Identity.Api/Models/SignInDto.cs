namespace Identity.Api.Models;

public class SignInDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
}
