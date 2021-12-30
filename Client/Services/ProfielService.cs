using Blazored.LocalStorage;
using JWTWebToken.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTWebToken.Client.Services
{
    public interface IProfielInterface
    {
        Task<Lid> gegevenslid(int ID);
    }

    public class ProfielService : IProfielInterface
    {
        private readonly HttpClient _httpClient;
        public ProfielService(HttpClient httpClient)
        {
            // Constructor
            _httpClient = httpClient;
        }

        public async Task<Lid> gegevenslid(int ID)
        {
            Lid lid = new Lid();
            try
            {
                Console.WriteLine("ProfielService.gegevenslid(" + ID + ")  --> aanroep Web API /api/Profiel/gegevenslid/" + ID);
                await Task.Delay(0);
                var opgehaald = await _httpClient.GetAsync("/api/Profiel/gegevenslid" + "/" + ID);
                Console.WriteLine("HTTP StatusCode:" + opgehaald.StatusCode);
                lid = await opgehaald.Content.ReadFromJsonAsync<Lid>();
                return lid;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ProfielService.gegevenslid(" + ID + ") aanroep Web API /api/Profiel/gegevenslid/" + ID + " --> Error!");
                Console.WriteLine(ex.Message);
                // throw;
                return lid;
            }
        }
    }

}
