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
    public IActionResult Get([FromServices] AppDbContext context)
        => Ok(context.Todos.ToList());


    [HttpGet]
    [Route("/{id:int}")]
    public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.FirstOrDefault(x => x.Id == id);

        if (todo == null)
            return NotFound();

        return Ok(todo);
    }

    [HttpPost]
    [Route("/")]
    public IActionResult Post([FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        context.Todos.Add(model);
        context.SaveChanges();

        return Created($"/{model.Id}", model);
    }

    [HttpPut]
    [Route("/{id:int}")]
    public IActionResult Put([FromRoute] int id, [FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.FirstOrDefault(x => x.Id == id);
        if (todo == null)
            return NotFound();

        todo.Title = model.Title;
        todo.Done = model.Done;

        context.Todos.Update(todo);
        context.SaveChanges();

        return Ok(todo);

    }

    [HttpDelete]
    [Route("/{id:int}")]
    public IActionResult Delete([FromRoute] int id, [FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        var todo = context.Todos.FirstOrDefault(x => x.Id == id);
        if (todo == null)
            return NotFound();

        context.Todos.Remove(todo);
        context.SaveChanges();

        return Ok(todo);
    }


}