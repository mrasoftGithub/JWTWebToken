using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Blazored.LocalStorage;


namespace JWTWebToken.Client.Handlers
{
    public class DelegatieHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;

        public DelegatieHandler(ILocalStorageService localStorageService)
        {
            //injecting local storage service
            _localStorageService = localStorageService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("DelegatieHandler.cs --> .SendAsync() om een token toe te voegen aan de header van de HTTP-request.");

                //de token uit de localStorage halen
                string token = await _localStorageService.GetItemAsStringAsync("jwt_token");

                //token toevoegen aan de authorization header
                if (token != null)
                {
                    // Eerste karakter verwijderen
                    token = token.Remove(0, 1);
                    // Laatste karakter verwijderen
                    token = token.Remove(token.Length - 1, 1);

                    // Toevoegen token
                    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    Console.WriteLine("Deze token wordt toegevoegd aan de header:");
                    Console.WriteLine(token);
                }

                //sending the request
                return await base.SendAsync(httpRequestMessage, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine("DelegatieHandler.cs --> .SendAsync(): Error!");
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}