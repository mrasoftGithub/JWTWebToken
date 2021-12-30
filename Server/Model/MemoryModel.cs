using JWTWebToken.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTWebToken.Server.Model
{
    public class MemoryModel : IModel
    {

        private readonly List<Lid> _Leden = new List<Lid>
        {
            // ƒMaakHashHash(Geheim1) = ede000b74d6dc6a69bfe76f05c8bc72c
            // ƒMaakHashHash(Geheim2) = 77b71b03bfb703ddd7425ca46ce03e56
            // ƒMaakHashHash(Geheim3) = 5098d86f323ccfcef2b17dfffa505ba7
            new Lid()
            {
                ID =1,
                Achternaam="Jansen",
                Voornaam="Sandra",
                Email="sjansen@mrasoft.nl",
                Password="ede000b74d6dc6a69bfe76f05c8bc72c",
                Abonnement="Aerobics",
                Expired= false
            },
            new Lid()
            {
                ID =2,
                Achternaam="Veermans",
                Voornaam="Peter",
                Email="pveermans@mrasoft.nl",
                Password="77b71b03bfb703ddd7425ca46ce03e56",
                Abonnement="Spinning",
                Expired= true
            },
            new Lid() {
                ID =3,
                Achternaam="Mulder",
                Voornaam="Olga",
                Email="omulder@mrasoft.nl",
                Password="5098d86f323ccfcef2b17dfffa505ba7",
                Abonnement="Yoga",
                Expired= false},
        };

        public async Task<Lid> HaalopLid(int ID)
        {
            // await Task.Delay(0);
            // throw new NotImplementedException();
            return await Task.Run(() => _Leden.FirstOrDefault(x => x.ID == ID));
        }

        public async Task<Lid> HaalopLid(string Email, string Password)
        {
            // await Task.Delay(0);
            // throw new NotImplementedException();
            return await Task.Run(() => _Leden.FirstOrDefault(x => x.Email.Trim().ToUpper() == Email.Trim().ToUpper() && x.Password == Password));
        }
    }
}
