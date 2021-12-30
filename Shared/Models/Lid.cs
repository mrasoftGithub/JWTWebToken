using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTWebToken.Shared.Models
{
    public partial class Lid
    {
        public int ID { get; set; }

        public string Achternaam { get; set; }

        public string Voornaam { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Abonnement { get; set; }

        public bool Expired { get; set; }

    }
}
