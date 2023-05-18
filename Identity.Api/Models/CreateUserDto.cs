using System.ComponentModel.DataAnnotations;

namespace Identity.Api.Models;

public class CreateUserDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    [Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }

}

