using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Account
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "E-mail")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Senha Inválida")]
        public string Password { get; set; } = string.Empty;
    }
}
