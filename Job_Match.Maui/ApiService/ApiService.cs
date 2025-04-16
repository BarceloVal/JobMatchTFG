using System.Net.Http;
using System.Net.Http.Json;
using Job_Match.Maui.Models;
using Job_Match.Maui.ApiService;
using Job_Match.Data.Models;

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
            var handler = new HttpClientHandler();

#if ANDROID
            // ⚠️ Acepta cualquier certificado (solo para desarrollo)
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://10.0.2.2:7010/")
            };
#else
                    _httpClient = new HttpClient(handler)
                    {
                        BaseAddress = new Uri("https://localhost:7010/")
                    };
#endif
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
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    // Guardar en la sesión global
                    UserSessionService.Instance.Load(loginResponse);

                    return loginResponse;
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

        #region m_user

        public async Task<m_user?> GetUserByIdAsync(Guid id_user)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/m_user/{id_user}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<m_user>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener usuario: {response.StatusCode}");
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

        #region m_doc_type

        public async Task<List<DocType>?> GetDocTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/doc-types");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<DocType>>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener tipos de documento: {response.StatusCode}");
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

        #region m_postal_code

        public async Task<List<string>?> GetPostalCodesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/postal-codes"); // Asegurate que este endpoint existe

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<string>>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener códigos postales: {response.StatusCode}");
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
