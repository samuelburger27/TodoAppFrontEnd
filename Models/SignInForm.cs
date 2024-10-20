using System.ComponentModel.DataAnnotations;

namespace WebAss.Models;

public class SignInForm
{
    [Required, EmailAddress] public string Email { get; set; } = "";

    [Required, MinLength(6, ErrorMessage = "Passwords must be at least 6 characters"),
     RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{8,}$",
         ErrorMessage =
             " Password must contain at least 1 digit, 1 upper case letter and 1 not alphanumeric character ")]
    public string Password { get; set; } = "";

    public bool isLogin { get; set; }
}