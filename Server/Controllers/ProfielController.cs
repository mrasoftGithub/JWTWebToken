using JWTWebToken.Server.Model;
using JWTWebToken.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace JWTWebToken.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfielController : ControllerBase
    {
        private readonly IModel _model;
        private readonly IConfiguration _configuration;

        public ProfielController(IModel model, IConfiguration configuration)
        {
            _model = model;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("gegevenslid/{ID}")]
        public async Task<ActionResult<Lid>> gegevenslid(int ID)
        {
            try
            {
                Debug.WriteLine("ProfielController Controller actionmethod  gegevenslid(" + ID + ")");
                Lid lid = await _model.HaalopLid(ID);
                return Ok(lid);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error ProfielController Controller actionmethod  gegevenslid(" + ID + ")");
                Debug.WriteLine(ex.Message);
                // throw;
                return Ok(new Lid());
            }
        }

        [Route("huidiglid")]
        [HttpGet]
        public async Task<ActionResult<Lid>> huidiglid()
        {
            try
            {
                await Task.Delay(0);
                Debug.WriteLine("LidController Controller actionmethod huidiglid()");

                // Leeg object retourneren indien niet Authenticated
                // Anders object vullen aan de hand van de claim
                Lid huidigLid = new Lid();

                //getting the secret key
                string geheimeSleutel = _configuration["JWTSettings:GeheimeSleutel"];
                var sleutel = Encoding.ASCII.GetBytes(geheimeSleutel);

                //preparing the validation parameters
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(sleutel),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                // de jwt_token die meegegeven is in de header
                var jwt_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                Debug.WriteLine("jwt_token:");
                Debug.WriteLine(jwt_token);

                // We simuleren een situatie dat met de token is geprutst
                // door de laatste karakter te verwijderen
                // jwt_token = jwt_token.Remove(jwt_token.Length - 1, 1);
                // Debug.WriteLine(jwt_token);

                //validating the token
                var principle = tokenHandler.ValidateToken(jwt_token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = (JwtSecurityToken)securityToken;
                if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    //returning the user if found
                    var claimID = principle.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    Debug.WriteLine("claimID: " + claimID);
                    huidigLid = await _model.HaalopLid(Convert.ToInt32(claimID));
                }
                Debug.WriteLine("");
                return Ok(huidigLid);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("");
                Debug.WriteLine("Error LidController Controller huidiglid()");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("");
                // throw;
                return Ok(new Lid());
            }
        }
    }
}
