using Identity.Api.Context;
using Identity.Api.Entities;
using Identity.Api.Models;
using Identity.Api.Services;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccauntController : ControllerBase
{
    private readonly AppDBContext _context;
    private ILogger<AccauntController> _logger;
    private readonly TokenService _tokenService;
    
    


    public AccauntController(ILogger<AccauntController> logger, AppDBContext context, TokenService tokenService)
    {
        _logger = logger;
        _context = context;
        _tokenService = tokenService;
    }


    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _context.Users.AnyAsync(user => user.Username == createUserDto.Username))
        {
            return BadRequest();
        }

        var user = createUserDto.Adapt<User>();

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok(createUserDto);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == signInDto.Username);

        if (user == null || user.Password != signInDto.Password)
        {
            return NotFound();
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(token);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var user = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);

        return Ok(user);
    }



    

}
