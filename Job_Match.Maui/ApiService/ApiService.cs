using System.Net.Http;
using System.Net.Http.Json;
using Job_Match.Maui.Models;
using Job_Match.Maui.ApiService;
using Job_Match.Data.Models;
using Job_Match.API.Models.Dtos;
using System.Net;
using Job_Match.Data.Models.Dtos;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Job_Match.Maui.ApiService
{
    public class ApiService
    {
        #region parameters

        private readonly HttpClient _httpClient;

        private string _authToken;

        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        #endregion

        #region constructor
        public ApiService()
        {
            var handler = new HttpClientHandler();

#if ANDROID
            // Acepta cualquier certificado (solo para desarrollo)
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
                    UserSessionService.Instance.Load(loginResponse, password);
                    // ASIGNAR TOKEN PARA USO EN TODAS LAS LLAMADAS PROTEGIDAS
                    SetAuthToken(loginResponse.token);

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

        public async Task<bool> DisableAccountAsync(Guid idUser)
        {
            var dto = new DisableAccountDto { IdUser = idUser };
            var response = await _httpClient.PostAsJsonAsync("api/m_user/disable", dto);
            Console.WriteLine($"DISABLE RESPONSE: {response.StatusCode} - {response.Content}");
            return response.IsSuccessStatusCode;
        }

        #endregion

        #region m_doc_type

        public async Task<List<m_doc_type>?> GetDocTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/doc-types");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_doc_type>>();
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

        public async Task<List<m_postal_code>?> GetPostalCodesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/postal-codes");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_postal_code>>();
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

        #region m_user_client

        public async Task<HttpResponseMessage> CreateClientAsync(CreateClientDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/m_user_client", dto);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}, Body: {content}");

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR API: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error interno al conectar con la API.")
                };
            }
        }

        public async Task<ProfileClientDto?> GetClientByMailAsync(string mail)
        {
            var response = await _httpClient.GetAsync($"api/m_user_client/by-mail/{mail}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileClientDto>(json);
        }

        #endregion

        #region m_user_provider
        public async Task<HttpResponseMessage> CreateProviderAsync(CreateProviderDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/m_user_provider", dto);
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status: {response.StatusCode}, Body: {content}");

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR API: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error interno al conectar con la API.")
                };
            }
        }
        public async Task<ProfileProviderDto?> GetProviderByMailAsync(string mail)
        {
            var response = await _httpClient.GetAsync($"api/m_user_provider/by-mail/{mail}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProfileProviderDto>(json);
        }

        #endregion

        #region m_service_type

        public async Task<List<m_service_type>?> GetServiceTypesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/service-type");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_service_type>>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener tipos de servicio: {response.StatusCode}");
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

        #region m_service

        public async Task<List<m_service>?> GetServicesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/services");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_service>>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener servicios: {response.StatusCode}");
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

        #region m_service_provider

        public async Task<List<m_service_provider>?> GetServiceProvidersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/m_service_provider");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_service_provider>>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo relaciones proveedor-servicio: {ex.Message}");
            }

            return null;
        }

        // Agregar servicios a un proveedor
        public async Task<bool> AddServicesToProviderAsync(Guid providerId, List<Guid> serviceIds)
        {
            var request = new
            {
                ProviderId = providerId,
                ServiceIds = serviceIds
            };

            var response = await _httpClient.PostAsJsonAsync("api/m_service_provider", request);

            return response.IsSuccessStatusCode;
        }
        public async Task<List<ProviderWithServicesDto>?> GetAllProvidersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/m_user_provider/all-with-services");

                if (!response.IsSuccessStatusCode)
                    return new List<ProviderWithServicesDto>(); // no null

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProviderWithServicesDto>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in GetAllProvidersAsync: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region m_task_status

        public async Task<List<m_task_status>?> GetStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/task-status");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<m_task_status>>();
                }
                else
                {
                    Console.WriteLine($"Error al obtener estados de tarea: {response.StatusCode}");
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

        #region t_task_request

        public async Task<bool> CreateTaskRequestAsync(CreateTaskRequestDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/task-request", dto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear oferta: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region changeClientToProvider

        public async Task<bool> ChangeClientToProviderAsync(ChangeClientToProviderDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/m_user_provider/change-client", dto);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ChangeClientToProviderAsync] Status: {response.StatusCode} - {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar cliente a proveedor: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ProviderExistsAsync(Guid idUser)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/m_user_provider/exists/{idUser}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar existencia de proveedor: {ex.Message}");
                return false;
            }
        }

        #endregion

        #region changeProviderToClient

        public async Task<bool> ChangeProviderToClientAsync(ChangeProviderToClientDto dto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/m_user_client/change-provider", dto);

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[ChangeClientToProviderAsync] Status: {response.StatusCode} - {content}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cambiar proveedor a cliente: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ClientExistsAsync(Guid idUser)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/m_user_client/exists/{idUser}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar existencia de cliente: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
