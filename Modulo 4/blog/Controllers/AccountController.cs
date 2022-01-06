using Blog.Data;
using blog.Extensions;
using Blog.Models;
using blog.Services;
using blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

[ApiController]
public class AccountController : ControllerBase
{
    private readonly TokenService _tokenService;
    public AccountController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("v1/accounts/")]
    public async Task<IActionResult> Post([FromBody] RegisterVM model, [FromServices] BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            context.Users.AddAsync(user);
            context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Este e-mail já está cadastrado!"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginVM model,
        [FromServices] BlogDataContext context,
        [FromServices] TokenService tokenService
    )
    {

        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var user = await context
           .Users
           .AsNoTracking()
           .Include(x => x.Roles)
           .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));

        try
        {
            var token = tokenService.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }
    }

    // [Authorize(Roles = "user")]
    // [HttpGet("v1/user")]
    // public IActionResult GetUser() => Ok(User.Identity.Name);
    //
    // [Authorize(Roles = "author")]
    // [HttpGet("v1/author")]
    // public IActionResult GetAuthor() => Ok(User.Identity.Name);
    //
    // [Authorize(Roles = "admin")]
    // [HttpGet("v1/admin")]
    // public IActionResult GetAdmin() => Ok(User.Identity.Name);
}