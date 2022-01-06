using Blog.Data;
using blog.Extensions;
using Blog.Models;
using blog.Services;
using blog.ViewModels;
using blog.ViewModels.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

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
    public async Task<IActionResult> Post( 
        [FromBody] RegisterVM model, 
        [FromServices] BlogDataContext context,
        [FromServices] EmailService emailService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErros()));

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@","-").Replace(".", "-")
        };

        var password = PasswordGenerator.Generate(25);
        user.PasswordHash =  PasswordHasher.Hash(password);

        try
        {
            context.Users.AddAsync(user);
            context.SaveChangesAsync();

            emailService.Send(
                user.Name, 
                user.Email, 
                "Bem vindo ao blog!", 
                $"Sua senha é {password}");

            return Ok(new ResultViewModel<dynamic>(new
            {
                user  = user.Email, password
            }));
        }
        catch (DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("Este e-mail já está cadastrado!") );
        }
        catch (Exception )
        {
            return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
        }
    }

    [HttpPost("v1/login")]
    public IActionResult Login()
    {
        var token = _tokenService.GenerateToken(new Models.User());

        return Ok(token);
    }

    [Authorize]
    [HttpPost("v1/accounts/upload-image")]
    public async Task<IActionResult> UploadImage(
        [FromBody] UploadImageVM model,
        [FromServices] BlogDataContext context)
    {
        var fileName = $"{Guid.NewGuid()}.jpg";
        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(model.Base64Image, "");
        var bytes = Convert.FromBase64String(data);

        try
        {
            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

        if (user == null)
            return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

        user.Image = $"https://localhost:0000/images/{fileName}";
        try
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Falha interna no servidor"));
        }

        return Ok(new ResultViewModel<string>("Imagem alterada com sucesso!", null));
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