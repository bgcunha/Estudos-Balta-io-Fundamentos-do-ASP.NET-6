using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels.Categories
{
    public class EditorCategoryVM
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 03 e 40 caracteres!")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "O slug é obrigatório!")]
        public string? Slug { get; set; }
    }
}
