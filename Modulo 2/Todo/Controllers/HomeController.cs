using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Todo.Data;
using Todo.Model;

namespace Todo.Controllers;

[ApiController]
[Route("v1")]
public class HomeControoller : ControllerBase
{

    [HttpGet]
    [Route("/")]
    public List<TodoModel> Get([FromServices] AppDbContext context)
    {
        return context.Todos.ToList();
    }



}