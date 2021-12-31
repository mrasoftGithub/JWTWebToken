using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using JWTWebToken.Shared.Models;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using System.Net.Http.Headers;

using JWTWebToken.Client.Services;

namespace JWTWebToken.Client
{
    public class AuthenticatieStatus : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticatieStatus(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Het object kan gevuld worden als succesvol is ingelogd
                Lid lid = await HuidigLid();

                if (lid != null && lid.ID != 0)
                {
                    //creëer claims uit het geretourneerde object
                    var claimID = new Claim(ClaimTypes.NameIdentifier, Convert.ToString(lid.ID));
                    var claimNaam = new Claim(ClaimTypes.Name, lid.Voornaam + " " + lid.Achternaam);
                    var claimExpired = new Claim(ClaimTypes.Expired, Convert.ToString(lid.Expired));

                    //create claimsIdentity
                    var claimsIdentity = new ClaimsIdentity(new[] { claimID, claimNaam, claimExpired }, "lidSportschool");

                    //retourneer de claimsPrincipal
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    return new AuthenticationState(claimsPrincipal);
                }
                else
                    // het lid kan niet gevonden 
                    // retourneer een lege ClaimsPrincipal
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            catch
            {
                // throw;
                // er is wat fout gegaan, retourneer een lege ClaimsPrincipal
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public async Task<Lid> HuidigLid()
        {
            //pulling the token from localStorage
            var token = await _localStorageService.GetItemAsStringAsync("jwt_token");
            if (token == null) return null;

            // Eerste karakter verwijderen
            token = token.Remove(0, 1);
            // Laatste karakter verwijderen
            token = token.Remove(token.Length - 1, 1);

            //-- Zet de token in de header
            //-- _httpClient.DefaultRequestHeaders.Add("Authorization","Bearer " + token);
            //-- var response = await _httpClient.GetAsync("/api/Profiel/huidiglid");
            //-- response.EnsureSuccessStatusCode();
            //-- var huidigLid = await response.Content.ReadFromJsonAsync<Lid>();
            //-- returning the user if found
            // if (huidigLid != null) return await Task.FromResult(huidigLid); else return null;

            using (var request = new HttpRequestMessage(HttpMethod.Get, "/api/Profiel/huidiglid"))
            {
                //-- Zet de token in de header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // use a single HttpClient instance for multiple requests.
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var huidigLid = await response.Content.ReadFromJsonAsync<Lid>();
                //-- returning the user if found
                if (huidigLid != null) return await Task.FromResult(huidigLid); else return null;
            }

        }
    }
}
