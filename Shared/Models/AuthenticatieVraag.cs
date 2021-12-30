using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JWTWebToken.Shared.Models
{
    public class AuthenticatieVraag
    {
        [Required(ErrorMessage = "Geef je e-mailadres op.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Geef je wachtwoord op.")]
        public string Password { get; set; }

    }
}
