﻿using blog.Extensions;
using blog.ViewModels;
using blog.ViewModels.Categories;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] IMemoryCache cache,
        [FromServices] BlogDataContext context)
    {
        try
        {
            //var categories = await context.Categories.ToListAsync();

           var categories = await cache.GetOrCreate("CategoriesCache", entry =>
           {
               entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
               return  context.Categories.ToListAsync();
           });


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

            return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>( category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("Não foi possível incluir a categoria!") );
        }
        catch (Exception)
        {
            return BadRequest(new ResultViewModel<Category>("Falha interna no servidor!"));
        }
    }

    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutPostAsync([FromRoute] int id, [FromBody] EditorCategoryVM model, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

            category.Name = model.Name;
            category.Slug = model.Slug;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("Não foi possível alterar a categoria!"));
        }
        catch (Exception)
        {
            return BadRequest(new ResultViewModel<Category>("Falha interna no servidor!"));
        }
    }

    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id, [FromServices] BlogDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("Não foi possível excluir a categoria"));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
        }
    }
}
