using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync([FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context.Categories.ToListAsync();

            return Ok(categories);
        }
        catch (Exception e)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
        {
            var categories = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (categories == null)
                return NotFound();

            return Ok(categories);
        }
        catch (Exception e)
        {
            return BadRequest("Falha interna no servidor!");
        }

    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] Category model, [FromServices] BlogDataContext context)
    {
        try
        {
            context.Categories.AddAsync(model);
            context.SaveChangesAsync();

            return Created($"v1/categories/{model.Id}", model);
        }
        catch (DbUpdateException e)
        {
            return StatusCode(500, "Não foi possivel incluir a categoria!");
        }
        catch (Exception e)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }

    [HttpPut("v1/categories/id:int")]
    public async Task<IActionResult> PutPostAsync([FromRoute] int id, [FromBody] Category model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException e)
        {
            return StatusCode(500, "Não foi possivel alerar a categoria!");
        }
        catch (Exception e)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromBody] Category model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            context.Categories.Remove(category);
            context.SaveChanges();

            return Created($"v1/categories/{model.Id}", model);
        }
        catch (DbUpdateException e)
        {
            return StatusCode(500, "Não foi possivel deletar a categoria!");
        }
        catch (Exception e)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }
}
