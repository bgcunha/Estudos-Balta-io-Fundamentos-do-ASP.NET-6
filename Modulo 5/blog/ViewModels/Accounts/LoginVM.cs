using System.ComponentModel.DataAnnotations;

namespace blog.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "O e-mail é obrigatório!")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório!")]
        public string Password { get; set; }
    }
}
