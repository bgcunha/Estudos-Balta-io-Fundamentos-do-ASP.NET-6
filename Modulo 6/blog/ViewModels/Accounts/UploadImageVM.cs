using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels.Accounts;

public class UploadImageVM
{
    [Required(ErrorMessage = "Imagem inválida")]
    public string Base64Image { get; set; }
}