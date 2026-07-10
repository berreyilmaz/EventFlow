using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}