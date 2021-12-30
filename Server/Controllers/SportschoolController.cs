using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTWebToken.Shared.Models;
using JWTWebToken.Server.Utilities;
using JWTWebToken.Server.Model;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Utility = JWTWebToken.Server.Utilities.Utility;
using System.Diagnostics;

namespace JWTWebToken.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SportschoolController : ControllerBase
    {
        private readonly IModel _model;
        private readonly IConfiguration _configuration;

        public SportschoolController(IModel model, IConfiguration configuration)
        {
            _model = model;
            _configuration = configuration;
        }

        [HttpPost("aanmelden")]
        public async Task<ActionResult<AuthenticatieAntwoord>> aanmelden(AuthenticatieVraag authenticatieVraag)
        {
            string token = string.Empty;

            // MaakHash password
            authenticatieVraag.Password = Utility.MaakHash(authenticatieVraag.Password);

            // Lid aangemeldLid = new Lid();
            Lid aangemeldLid = await _model.HaalopLid(authenticatieVraag.Email, authenticatieVraag.Password);

            // Evalueren dat correct is ingelogd
            if (aangemeldLid != null)
            {
                // Token genereren
                token = GenereerJwtToken_hs256(aangemeldLid);
                Debug.WriteLine("GenereerJwtToken_hs256:");
                Debug.WriteLine(token);
                Debug.WriteLine("");
            }
            return await Task.FromResult(new AuthenticatieAntwoord() { Token = token });
        }

        private string GenereerJwtToken_hs256(Lid lid)
        {
            //getting the secret key
            string geheimeSleutel = _configuration["JWTSettings:GeheimeSleutel"];
            var sleutel = Encoding.ASCII.GetBytes(geheimeSleutel);

            //------------------------------
            // using System.Security.Claims;
            //------------------------------
            //create claims
            var claimID = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(lid.ID));
            var claimNaam = new Claim(ClaimTypes.Name, lid.Voornaam + " " + lid.Achternaam);

            //create claimsIdentity
            var claimsIdentity = new ClaimsIdentity(new[] { claimID, claimNaam }, "lidSportschool");

            //create claimsPrincipal
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // AuthenticationProperties 
            // Expires = DateTime.UtcNow.AddDays(1),
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(sleutel), SecurityAlgorithms.HmacSha256Signature)
            };

            //creating a token handler
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            //returning the token back
            return jwtSecurityTokenHandler.WriteToken(token);
        }
    }
}
