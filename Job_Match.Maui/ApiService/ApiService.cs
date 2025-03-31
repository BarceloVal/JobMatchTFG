using System.Net.Http;
using System.Net.Http.Json;
using Job_Match.Maui.Models;

namespace Job_Match.Maui.ApiService
{
    public class ApiService
    {
        #region parameters

        private readonly HttpClient _httpClient;

        #endregion

        #region constructor
        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7010/") // Cambiar por 10.0.2.2 si estás en Android
            };
        }

        #endregion

        #region login

        public async Task<LoginResponse?> LoginAsync(string mail, string password)
        {
            try
            {
                var request = new LoginRequest
                {
                    mail = mail,
                    password = password
                };

                var response = await _httpClient.PostAsJsonAsync("api/m_user/login", request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                else
                {
                    // Podés loguear el status si querés
                    Console.WriteLine($"Error de login: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Error de red: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }

            return null;
        }

        #endregion

    }
}
