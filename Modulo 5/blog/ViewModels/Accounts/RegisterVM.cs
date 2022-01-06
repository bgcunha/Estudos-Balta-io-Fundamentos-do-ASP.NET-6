using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels.Accounts
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 03 e 40 caracteres!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }
    }
}
