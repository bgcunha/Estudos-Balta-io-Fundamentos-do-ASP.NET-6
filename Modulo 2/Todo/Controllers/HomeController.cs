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

    [HttpGet]
    [Route("/{id:int}")]
    public TodoModel GetById([FromRoute] int id, [FromServices] AppDbContext context)
    {
        return context.Todos.FirstOrDefault(x => x.Id == id);
    }

    [HttpPost]
    [Route("/")]
    public TodoModel Post([FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        context.Todos.Add(model);
        context.SaveChanges();

        return model;
    }

    [HttpPut]
    [Route("/{id:int}")]
    public TodoModel Put([FromRoute] int id, [FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        var todo = GetById(id, context);
        if (todo == null)
            return model;

        todo.Title = model.Title;
        todo.Done = model.Done;

        context.Todos.Update(todo);
        context.SaveChanges();

        return todo;
    }

    [HttpDelete]
    [Route("/{id:int}")]
    public TodoModel Delete([FromRoute] int id, [FromBody] TodoModel model, [FromServices] AppDbContext context)
    {
        var todo = GetById(id, context);

        context.Todos.Remove(todo);
        context.SaveChanges();

        return todo;
    }


}