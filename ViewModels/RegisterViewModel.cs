using System.ComponentModel.DataAnnotations;

namespace EventFlow.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Ad Soyad zorunludur.")]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
    [Display(Name = "Kullanıcı Adı")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-posta zorunludur.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Telefon")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
    [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}