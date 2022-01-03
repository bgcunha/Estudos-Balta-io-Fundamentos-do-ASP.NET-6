using blog.Extensions;
using blog.ViewModels;
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

            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch (Exception)
        {
            return BadRequest(new ResultViewModel<List<Category>>("Falha Interna no servdor!"));
        }
    }

    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado!"));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception)
        {
            return BadRequest(new ResultViewModel<List<Category>>("Falha Interna no servdor!"));
        }

    }

    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync([FromBody] EditorCategoryVM model, [FromServices] BlogDataContext context)
    {
        try
        {
            //Usando ModelStateExtension
            if (!ModelState.IsValid)
                return BadRequest( new ResultViewModel<Category>(ModelState.GetErros()));

            var category = new Category
            {
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower()
            };

            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Created($"v1/categories/{category.Id}", category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Não foi possível incluir a categoria!");
        }
        catch (Exception)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutPostAsync([FromRoute] int id, [FromBody] EditorCategoryVM model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound("Conteúdo não encontrado");

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Não foi possível alterar a categoria!");
        }
        catch (Exception)
        {
            return BadRequest("Falha interna no servidor!");
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound("Conteúdo não encontrado");

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(category);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, "Não foi possível excluir a categoria");
        }
        catch (Exception)
        {
            return StatusCode(500, "Falha interna no servidor");
        }
    }
}
