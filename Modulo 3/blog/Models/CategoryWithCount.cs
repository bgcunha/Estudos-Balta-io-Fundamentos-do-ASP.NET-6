using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;


public class CategoryWithCount
{
    public string Name { get; set; }
    public int Count { get; set; }

}
