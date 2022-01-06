using blog.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("")]
//[ApiKey]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Resposta = "Hello", Texto = "World" });
    }
}

