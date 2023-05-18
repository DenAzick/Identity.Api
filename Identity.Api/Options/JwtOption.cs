namespace Identity.Api.Services;

public class JwtOption
{
    public string SignInkey { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int ExpiresInSeconds { get; set; }
}
