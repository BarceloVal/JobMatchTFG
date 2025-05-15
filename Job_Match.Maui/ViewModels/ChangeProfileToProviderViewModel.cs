using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Job_Match.Data.Models;
using Job_Match.Maui.ApiService;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;
using Job_Match.Maui.Models;
using System.Runtime.CompilerServices;
using System.Globalization;
using Job_Match.Data.Models.Dtos;

namespace Job_Match.Maui.ViewModels
{
    public class ChangeProfileToProviderViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor del ViewModel. Inicializa listas de datos necesarias desde la API:
        /// tipos de documento, códigos postales, tipos de servicio y servicios disponibles.
        /// </summary>
        public ChangeProfileToProviderViewModel()
        {
            LoadDocTypesAsync();
            LoadPostalCodesAsync();
            LoadServiceTypesAsync();
            LoadServicesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Lanza el evento que notifica a la interfaz de usuario que una propiedad ha cambiado.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad (asignado automáticamente si se omite).</param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ApiServiceClass _apiService = new();

        // -----------------------
        // ------- Nombre -------
        // -----------------------
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    ValidateFirstName();
                }
            }
        }

        private string _firstNameError;
        public string FirstNameError
        {
            get => _firstNameError;
            set
            {
                _firstNameError = value;
                OnPropertyChanged(nameof(FirstNameError));
                OnPropertyChanged(nameof(IsFirstNameValid));
            }
        }

        public bool IsFirstNameValid => string.IsNullOrEmpty(FirstNameError);

        /// <summary>
        /// Valida el nombre (obligatorio, máximo 30 caracteres).
        /// </summary>
        private void ValidateFirstName()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
                FirstNameError = "Campo obligatorio.";
            else if (FirstName.Length > 30)
                FirstNameError = "Máximo 30 caracteres.";
            else
                FirstNameError = string.Empty;
        }

        // -----------------------
        // ------ Apellido ------
        // -----------------------
        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    ValidateLastName();
                }
            }
        }

        private string _lastNameError;
        public string LastNameError
        {
            get => _lastNameError;
            set
            {
                _lastNameError = value;
                OnPropertyChanged(nameof(LastNameError));
                OnPropertyChanged(nameof(IsLastNameValid));
            }
        }

        public bool IsLastNameValid => string.IsNullOrEmpty(LastNameError);

        /// <summary>
        /// Valida el apellido (obligatorio, máximo 30 caracteres).
        /// </summary>
        private void ValidateLastName()
        {
            if (string.IsNullOrWhiteSpace(LastName))
                LastNameError = "Campo obligatorio.";
            else if (LastName.Length > 30)
                LastNameError = "Máximo 30 caracteres.";
            else
                LastNameError = string.Empty;
        }


        // -----------------------
        // -- Nombre comercial --
        // -----------------------
        private string _tradeName;
        public string TradeName
        {
            get => _tradeName;
            set
            {
                if (_tradeName != value)
                {
                    _tradeName = value;
                    OnPropertyChanged(nameof(TradeName));
                    ValidateTradeName();
                }
            }
        }

        private string _tradeNameError;
        public string TradeNameError
        {
            get => _tradeNameError;
            set
            {
                _tradeNameError = value;
                OnPropertyChanged(nameof(TradeNameError));
                OnPropertyChanged(nameof(IsTradeNameValid));
            }
        }

        public bool IsTradeNameValid => string.IsNullOrEmpty(TradeNameError);

        /// <summary>
        /// Valida el nombre comercial del proveedor (obligatorio, máximo 50 caracteres).
        /// </summary>
        private void ValidateTradeName()
        {
            if (string.IsNullOrWhiteSpace(TradeName))
                TradeNameError = "Campo obligatorio.";
            else if (TradeName.Length > 50)
                TradeNameError = "Máximo 50 caracteres.";
            else
                TradeNameError = string.Empty;
        }

        // --------------------------------
        // ------- Tipo Documento -------
        // --------------------------------
        public ObservableCollection<m_doc_type> DocTypes { get; set; } = new();

        private m_doc_type _selectedDocType;
        public m_doc_type SelectedDocType
        {
            get => _selectedDocType;
            set
            {
                _selectedDocType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Carga la lista de tipos de documento desde la API.
        /// </summary>
        private async void LoadDocTypesAsync()
        {
            var result = await _apiService.GetDocTypesAsync();
            if (result != null)
            {
                DocTypes.Clear();

                foreach (var item in result)
                {
                    Console.WriteLine($"Cargando tipo: {item.id_mask} - {item.name}");
                    DocTypes.Add(item);
                }
            }
            else
            {
                Console.WriteLine("No se recibieron datos del servidor.");
            }
        }

        // -------------------
        // ---- Documento ----
        // -------------------
        private string _document;
        public string Document
        {
            get => _document;
            set
            {
                if (_document != value)
                {
                    _document = value;
                    OnPropertyChanged(nameof(Document));
                    ValidateDocument();
                }
            }
        }

        private string _documentError;
        public string DocumentError
        {
            get => _documentError;
            set
            {
                _documentError = value;
                OnPropertyChanged(nameof(DocumentError));
                OnPropertyChanged(nameof(IsDocumentValid));
            }
        }

        public bool IsDocumentValid => string.IsNullOrEmpty(DocumentError);


        /// <summary>
        /// Valida el documento según el tipo seleccionado (DNI, NIE, PAS).
        /// </summary>
        private void ValidateDocument()
        {
            if (string.IsNullOrWhiteSpace(Document))
            {
                DocumentError = "Campo obligatorio.";
                return;
            }

            if (SelectedDocType == null)
            {
                DocumentError = "Selecciona un tipo de documento.";
                return;
            }

            var doc = Document.ToUpper().Trim();
            var tipo = SelectedDocType.id_mask.ToUpper();

            switch (tipo)
            {
                case "DNI":
                    if (!Regex.IsMatch(Document, @"^\d{8}[A-Za-z]$"))
                    {
                        DocumentError = "DNI no válido. Debe tener 8 números y 1 letra.";
                    }
                    else if (!Regex.IsMatch(Document, @"^\d{8}[A-Z]$"))
                    {
                        DocumentError = "La letra del DNI debe estar en mayúscula.";
                    }
                    else
                    {
                        DocumentError = "";
                    }
                    break;

                case "NIE":
                    if (!Regex.IsMatch(Document, @"^[XYZxyz]\d{7}[A-Za-z]$"))
                    {
                        DocumentError = "NIE no válido. Formato: X1234567T.";
                    }
                    else if (!Regex.IsMatch(Document, @"^[XYZ]\d{7}[A-Z]$"))
                    {
                        DocumentError = "Las letras del NIE deben estar en mayúscula.";
                    }
                    else
                    {
                        DocumentError = "";
                    }
                    break;

                case "NIF":
                    if (!Regex.IsMatch(Document, @"^[A-Za-z0-9]{1,9}$"))
                    {
                        DocumentError = "NIF no válido. Debe tener hasta 9 caracteres alfanuméricos.";
                    }
                    else if (!Regex.IsMatch(Document, @"^[A-Z0-9]{1,9}$"))
                    {
                        DocumentError = "El NIF debe estar en mayúsculas.";
                    }
                    else
                    {
                        DocumentError = "";
                    }
                    break;

                case "PAS":
                    DocumentError = doc.Length >= 5 ? "" : "Pasaporte demasiado corto.";
                    break;

                default:
                    DocumentError = "";
                    break;
            }

        }

        // -----------------------
        // ------- Website -------
        // -----------------------
        private string _website;
        public string Website
        {
            get => _website;
            set
            {
                if (_website != value)
                {
                    _website = value;
                    OnPropertyChanged(nameof(Website));
                    ValidateWebsite();
                }
            }
        }

        private string _websiteError;
        public string WebsiteError
        {
            get => _websiteError;
            set
            {
                _websiteError = value;
                OnPropertyChanged(nameof(WebsiteError));
                OnPropertyChanged(nameof(IsWebsiteValid));
            }
        }

        public bool IsWebsiteValid => string.IsNullOrEmpty(WebsiteError);

        /// <summary>
        /// Valida el sitio web. Campo opcional pero debe ser una URL válida si se proporciona.
        /// </summary>
        private void ValidateWebsite()
        {
            if (string.IsNullOrWhiteSpace(Website))
            {
                WebsiteError = string.Empty; // Campo no obligatorio
            }
            else if (!Uri.TryCreate(Website, UriKind.Absolute, out Uri? uriResult)
                     || string.IsNullOrEmpty(uriResult.Host)
                     || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                WebsiteError = "Debe ser una URL válida (con http o https).";
            }
            else
            {
                WebsiteError = string.Empty;
            }
        }

        // -------------------
        // --- Dirección ---
        // -------------------
        private string _address;
        public string Address
        {
            get => _address;
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                    ValidateAddress();
                }
            }
        }

        private string _addressError;
        public string AddressError
        {
            get => _addressError;
            set
            {
                _addressError = value;
                OnPropertyChanged(nameof(AddressError));
                OnPropertyChanged(nameof(IsAddressValid));
            }
        }

        public bool IsAddressValid => string.IsNullOrEmpty(AddressError);

        /// <summary>
        /// Valida la dirección (obligatoria, máximo 50 caracteres).
        /// </summary>
        private void ValidateAddress()
        {
            if (string.IsNullOrWhiteSpace(Address))
                AddressError = "Campo obligatorio.";
            else if (Address.Length > 50)
                AddressError = "Máximo 50 caracteres.";
            else
                AddressError = string.Empty;
        }

        // ---------------------
        // --- Coordenadas ---
        // ---------------------
        private string _mapCoordinates;
        public string MapCoordinates
        {
            get => _mapCoordinates;
            set
            {
                if (_mapCoordinates != value)
                {
                    _mapCoordinates = value;
                    OnPropertyChanged();
                    ValidateMapCoordinates();
                }
            }
        }

        private string _mapCoordinatesError;
        public string MapCoordinatesError
        {
            get => _mapCoordinatesError;
            set
            {
                if (_mapCoordinatesError != value)
                {
                    _mapCoordinatesError = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasMapCoordinatesError));
                    OnPropertyChanged(nameof(IsMapCoordinatesValid));
                }
            }
        }

        public bool HasMapCoordinatesError => !string.IsNullOrEmpty(MapCoordinatesError);
        public bool IsMapCoordinatesValid => string.IsNullOrEmpty(MapCoordinatesError);

        /// <summary>
        /// Valida las coordenadas del mapa. Opcional, pero deben tener el formato correcto lat,long.
        /// </summary>
        private void ValidateMapCoordinates()
        {
            if (string.IsNullOrWhiteSpace(MapCoordinates))
            {
                MapCoordinatesError = string.Empty; // no es obligatorio
                return;
            }

            var parts = MapCoordinates.Split(',');

            if (parts.Length != 2 ||
                !double.TryParse(parts[0], CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(parts[1], CultureInfo.InvariantCulture, out double lon) ||
                lat < -90 || lat > 90 ||
                lon < -180 || lon > 180)
            {
                MapCoordinatesError = "Formato inválido. Usa 'latitud,longitud'.";
            }
            else
            {
                MapCoordinatesError = string.Empty;
            }
        }

        // ---------------------
        // --- Código postal ---
        // ---------------------
        public ObservableCollection<m_postal_code> PostalCodes { get; set; } = new();

        private m_postal_code _selectedPostalCode;
        public m_postal_code SelectedPostalCode
        {
            get => _selectedPostalCode;
            set
            {
                if (_selectedPostalCode != value)
                {
                    _selectedPostalCode = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Carga la lista de códigos postales desde la API.
        /// </summary>
        private async void LoadPostalCodesAsync()
        {
            List<m_postal_code> result = await _apiService.GetPostalCodesAsync();
            if (result != null)
            {
                PostalCodes.Clear();
                foreach (var code in result)
                {
                    PostalCodes.Add(code);
                }
            }
        }

        // ---------------------
        // --- Descripción ---
        // ---------------------
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                    ValidateDescription();
                }
            }
        }

        private string _descriptionError;
        public string DescriptionError
        {
            get => _descriptionError;
            set
            {
                _descriptionError = value;
                OnPropertyChanged(nameof(DescriptionError));
                OnPropertyChanged(nameof(IsDescriptionValid));
            }
        }

        public bool IsDescriptionValid => string.IsNullOrEmpty(DescriptionError);

        /// <summary>
        /// Valida la descripción del proveedor (obligatoria, máximo 1000 caracteres).
        /// </summary>
        private void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
                DescriptionError = "Campo obligatorio.";
            else if (Description.Length > 1000)
                DescriptionError = "Máximo 1000 caracteres.";
            else
                DescriptionError = string.Empty;
        }

        // --------------------
        // --- Teléfono 1 ---
        // --------------------
        private string _phone1;
        public string Phone1
        {
            get => _phone1;
            set
            {
                if (_phone1 != value)
                {
                    _phone1 = value;
                    OnPropertyChanged(nameof(Phone1));
                    ValidatePhone1();
                }
            }
        }

        private string _phone1Error;
        public string Phone1Error
        {
            get => _phone1Error;
            set
            {
                _phone1Error = value;
                OnPropertyChanged(nameof(Phone1Error));
                OnPropertyChanged(nameof(IsPhone1Valid));
            }
        }

        public bool IsPhone1Valid => string.IsNullOrEmpty(Phone1Error);

        /// <summary>
        /// Valida el teléfono principal (obligatorio, formato español con 9 dígitos).
        /// </summary>
        private void ValidatePhone1()
        {
            if (string.IsNullOrWhiteSpace(Phone1))
                Phone1Error = "Campo obligatorio.";
            else if (!Regex.IsMatch(Phone1, @"^[6-7]{1}[0-9]{8}$"))
                Phone1Error = "El formato del teléfono no es válido.";
            else
                Phone1Error = string.Empty;

            OnPropertyChanged(nameof(IsPhone1Valid));
        }


        // --------------------
        // --- Teléfono 2 ---
        // --------------------
        private string _phone2;
        public string Phone2
        {
            get => _phone2;
            set
            {
                if (_phone2 != value)
                {
                    _phone2 = value;
                    OnPropertyChanged(nameof(Phone2));
                    ValidatePhone2();
                }
            }
        }

        private string _phone2Error;
        public string Phone2Error
        {
            get => _phone2Error;
            set
            {
                _phone2Error = value;
                OnPropertyChanged(nameof(Phone2Error));
                OnPropertyChanged(nameof(IsPhone2Valid));
            }
        }

        public bool IsPhone2Valid => string.IsNullOrEmpty(Phone2Error);

        /// <summary>
        /// Valida el segundo teléfono. Es opcional, pero si se incluye debe ser válido.
        /// </summary>
        private void ValidatePhone2()
        {
            if (string.IsNullOrWhiteSpace(Phone2))
                Phone2Error = string.Empty; // Es válido si está vacío (opcional)
            else if (!Regex.IsMatch(Phone2, @"^[6-7]{1}[0-9]{8}$"))
                Phone2Error = "El formato del teléfono no es válido.";
            else
                Phone2Error = string.Empty;

            OnPropertyChanged(nameof(IsPhone2Valid));
        }

        // ------------------------
        // --- Tipo de servicio ---
        // ------------------------
        public class ServiceTypeSelectable : m_service_type
        {
            public bool IsSelected { get; set; } = false;
        }

        // Tipo de servicio (Checkbox)
        public ObservableCollection<ServiceTypeSelectable> ServiceTypes { get; set; } = new();

        /// <summary>
        /// Carga la lista de tipos de servicio desde la API.
        /// </summary>
        private async void LoadServiceTypesAsync()
        {
            var result = await _apiService.GetServiceTypesAsync();
            if (result != null)
            {
                ServiceTypes.Clear();
                foreach (var item in result)
                {
                    ServiceTypes.Add(new ServiceTypeSelectable
                    {
                        id = item.id,
                        id_mask = item.id_mask,
                        name = item.name,
                        active = item.active,
                        created_on = item.created_on,
                        last_update = item.last_update
                    });
                }
            }
        }


        // ----------------
        // --- Servicio ---
        // ----------------
        public class ServiceSelectable : m_service
        {
            public bool IsSelected { get; set; } = false;
        }

        // Servicios filtrados (Checkbox)
        public ObservableCollection<ServiceSelectable> FilteredServices { get; set; } = new();

        // Tipo de servicio seleccionado
        private m_service_type _selectedServiceType;
        public m_service_type SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                if (_selectedServiceType != value)
                {
                    _selectedServiceType = value;
                    OnPropertyChanged();
                    FilterServicesByType();
                }
            }
        }

        /// <summary>
        /// Filtra los servicios según el tipo de servicio seleccionado
        /// y actualiza la lista mostrada al usuario.
        /// </summary>
        private void FilterServicesByType()
        {
            if (SelectedServiceType == null)
                return;

            // Limpiar la lista actual
            FilteredServices.Clear();

            // Agregar solo los servicios que coincidan con el tipo seleccionado
            var filtered = _allServices
                .Where(service => service.id_service_type == SelectedServiceType.id)
                .ToList();

            foreach (var service in filtered)
            {
                FilteredServices.Add(new ServiceSelectable
                {
                    id = service.id,
                    id_service_type = service.id_service_type,
                    name = service.name,
                    IsSelected = false // inicializamos en false
                });
            }
        }


        // Todos los servicios (sin filtrar)
        private List<ServiceSelectable> _allServices = new();

        /// <summary>
        /// Carga la lista de servicios disponibles desde la API y los guarda en memoria.
        /// </summary>
        private async void LoadServicesAsync()
        {
            var result = await _apiService.GetServicesAsync();
            if (result != null)
            {
                _allServices = result.Select(service => new ServiceSelectable
                {
                    id = service.id,
                    id_service_type = service.id_service_type,
                    name = service.name,
                    description = service.description,
                    IsSelected = false
                }).ToList();

                FilteredServices.Clear();
                foreach (var service in _allServices)
                {
                    FilteredServices.Add(service);
                }
            }
        }

        /// <summary>
        /// Ejecuta el proceso de cambio de perfil de cliente a proveedor.
        /// Valida los campos, construye el DTO y llama al endpoint correspondiente de la API.
        /// </summary>
        /// <returns>True si la operación fue exitosa; False en caso de error o validación fallida.</returns>
        public async Task<bool> ChangeToProviderAsync()
        {
            var session = UserSessionService.Instance;
            var password = UserSessionService.Instance.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo recuperar la contraseña.", "Aceptar");
                return false;
            }

            // Validar campos
            ValidateTradeName();
            ValidateWebsite();
            ValidateMapCoordinates();
            ValidateDescription();

            if (!IsTradeNameValid || !IsWebsiteValid || !IsMapCoordinatesValid || !IsDescriptionValid)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Completa todos los campos obligatorios.", "Aceptar");
                return false;
            }

            // Validar tipo de servicio y servicio
            if (SelectedServiceType == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un tipo de servicio.", "Aceptar");
                return false;
            }

            var selectedService = FilteredServices.FirstOrDefault(s => s.IsSelected);
            if (selectedService == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un servicio.", "Aceptar");
                return false;
            }

            // Construir el DTO
            var dto = new ChangeClientToProviderDto
            {
                IdUser = session.Id,
                FirstName = FirstName,
                LastName = LastName,
                Document = Document,
                IdDocType = SelectedDocType?.id,
                Address = Address,
                IdPostalCode = SelectedPostalCode?.id,
                Phone1 = Phone1,
                Phone2 = Phone2,
                TradeName = TradeName,
                Description = Description,
                Website = Website,
                MapCoordinates = MapCoordinates,
                IdServiceType = SelectedServiceType.id,
                IdService = selectedService.id,
                Password = password
            };

            // Llamar a la API
            var success = await _apiService.ChangeClientToProviderAsync(dto);

            if (!success)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo cambiar el perfil.", "Aceptar");
                return false;
            }

            return true;
        }
    }
}
