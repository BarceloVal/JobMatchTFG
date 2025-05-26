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
    public class CreateProviderAccountViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Constructor del ViewModel. Inicializa la carga de tipos de documento, códigos postales, tipos de servicio y servicios desde la API.
        /// </summary>
        public CreateProviderAccountViewModel()
        {
            LoadDocTypesAsync();
            LoadPostalCodesAsync();
            LoadServiceTypesAsync();
            LoadServicesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Lanza el evento que notifica a la interfaz de usuario cuando una propiedad cambia.
        /// </summary>
        /// <param name="propertyName">Nombre de la propiedad que cambió (se determina automáticamente).</param>
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

        /// <summary>Valida el campo Nombre. Máximo 30 caracteres, obligatorio.</summary>
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

        /// <summary>Valida el campo Apellido. Máximo 30 caracteres, obligatorio.</summary>
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

        /// <summary>Valida el campo Nombre Comercial. Máximo 50 caracteres, obligatorio.</summary>
        private void ValidateTradeName()
        {
            if (string.IsNullOrWhiteSpace(TradeName))
                TradeNameError = "Campo obligatorio.";
            else if (TradeName.Length > 50)
                TradeNameError = "Máximo 50 caracteres.";
            else
                TradeNameError = string.Empty;
        }

        // ---------------
        // --- Mail ---
        // ---------------
        private string _mail;
        public string Mail
        {
            get => _mail;
            set
            {
                if (_mail != value)
                {
                    _mail = value;
                    OnPropertyChanged(nameof(Mail));
                    ValidateMail();
                }
            }
        }

        private string _mailError;
        public string MailError
        {
            get => _mailError;
            set
            {
                _mailError = value;
                OnPropertyChanged(nameof(MailError));
                OnPropertyChanged(nameof(IsMailValid));
            }
        }

        public bool IsMailValid => string.IsNullOrEmpty(MailError);

        /// <summary>Valida el campo Email. Debe tener formato válido.</summary>
        private void ValidateMail()
        {
            if (string.IsNullOrWhiteSpace(Mail))
                MailError = "Campo obligatorio.";
            else if (!Regex.IsMatch(Mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                MailError = "El formato del correo no es válido.";
            else
                MailError = string.Empty;

            OnPropertyChanged(nameof(IsMailValid));
        }


        // ---------------------
        // ---- Contraseña ----
        // ---------------------
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    ValidatePassword();
                    ValidateRepeatPassword(); // También se verifica coincidencia
                }
            }
        }

        private string _passwordError;
        public string PasswordError
        {
            get => _passwordError;
            set
            {
                _passwordError = value;
                OnPropertyChanged(nameof(PasswordError));
                OnPropertyChanged(nameof(IsPasswordValid));
            }
        }

        public bool IsPasswordValid => string.IsNullOrEmpty(PasswordError);

        /// <summary>Valida el campo Contraseña. Obligatorio.</summary>
        private void ValidatePassword()
        {
            PasswordError = string.IsNullOrWhiteSpace(Password) ? "Campo obligatorio." : string.Empty;
            OnPropertyChanged(nameof(IsPasswordValid));
        }


        // --------------------------------
        // ----- Repetir contraseña -----
        // --------------------------------
        private string _repeatPassword;
        public string RepeatPassword
        {
            get => _repeatPassword;
            set
            {
                if (_repeatPassword != value)
                {
                    _repeatPassword = value;
                    OnPropertyChanged(nameof(RepeatPassword));
                    ValidateRepeatPassword();
                }
            }
        }

        private string _repeatPasswordError;
        public string RepeatPasswordError
        {
            get => _repeatPasswordError;
            set
            {
                _repeatPasswordError = value;
                OnPropertyChanged(nameof(RepeatPasswordError));
                OnPropertyChanged(nameof(IsRepeatPasswordValid));
            }
        }

        public bool IsRepeatPasswordValid => string.IsNullOrEmpty(RepeatPasswordError);

        /// <summary>Valida el campo Repetir Contraseña. Debe coincidir con la contraseña.</summary>
        private void ValidateRepeatPassword()
        {
            RepeatPasswordError =
                string.IsNullOrWhiteSpace(RepeatPassword) ? "Campo obligatorio." :
                RepeatPassword != Password ? "Las contraseñas no coinciden." :
                string.Empty;

            OnPropertyChanged(nameof(IsRepeatPasswordValid));
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

        /// <summary>Carga los tipos de documento desde la API y los almacena en una colección observable.</summary>
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

        /// <summary>Valida el Documento según el tipo (DNI, NIE, PAS).</summary>
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

        /// <summary>Valida el campo Sitio Web. Opcional, pero debe tener formato válido con http/https.</summary>
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

        /// <summary>Valida el campo Dirección. Máximo 50 caracteres, obligatorio.</summary>
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
        /// Valida el campo Coordenadas. Opcional, pero debe estar en formato "latitud,longitud" válido.
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

        /// <summary>Carga los códigos postales desde la API y los guarda en una colección observable.</summary>
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

        /// <summary>Valida la Descripción. Obligatoria, máximo 1000 caracteres.</summary>
        private void ValidateDescription()
        {
            if (Description.Length > 1000)
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

        /// <summary>Valida el campo Teléfono 1. Obligatorio, formato español.</summary>
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

        /// <summary>Valida el campo Teléfono 2. Opcional, pero debe tener formato si se especifica.</summary>
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

        /// <summary>Carga los tipos de servicio desde la API y los almacena con estado seleccionable.</summary>
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
        /// Filtra la lista de servicios mostrados según el tipo de servicio seleccionado.
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

        /// <summary>Carga los servicios desde la API y los guarda en una lista filtrable por tipo de servicio.</summary>
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

                Console.WriteLine($"Se cargaron {result.Count} servicios.");
            }
            else
            {
                Console.WriteLine("No se recibieron servicios del servidor.");
            }
        }

        /// <summary>
        /// Realiza la creación de un nuevo proveedor. Valida los campos, crea el DTO, lo envía a la API y asocia el servicio seleccionado.
        /// </summary>
        /// <returns>True si la creación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> CreateProviderAsync()
        {
            // Validar campos antes de enviar
            ValidateFirstName();
            ValidateLastName();
            ValidateTradeName();
            ValidateMail();
            ValidatePassword();
            ValidateRepeatPassword();
            ValidateDocument();
            ValidateAddress();
            ValidatePhone1();
            ValidatePhone2();
            ValidateWebsite();
            ValidateMapCoordinates();
            ValidateDescription();

            if (!IsFirstNameValid || !IsLastNameValid || !IsTradeNameValid || !IsMailValid ||
                !IsPasswordValid || !IsRepeatPasswordValid || !IsDocumentValid || !IsAddressValid ||
                !IsPhone1Valid || !IsPhone2Valid || !IsWebsiteValid || !IsMapCoordinatesValid || !IsDescriptionValid)
            {
                return false;
            }

            var providerDto = new CreateProviderDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Mail = Mail,
                Password = Password,
                TradeName = TradeName,
                IdDocType = SelectedDocType.id,
                Document = Document,
                Address = Address,
                IdPostalCode = SelectedPostalCode.id,
                Phone1 = Phone1,
                Phone2 = Phone2,
                Website = Website,
                MapCoordinates = MapCoordinates,
                Description = Description
            };

            var response = await _apiService.CreateProviderAsync(providerDto);

            var provider = await _apiService.GetProviderByMailAsync(providerDto.Mail);

            if (provider == null)
            {
                return false;
            }

            var selectedService = FilteredServices.FirstOrDefault(s => s.IsSelected);
            if (selectedService == null)
            {
                Console.WriteLine("No se ha seleccionado ningún servicio.");
                return false;
            }

            await _apiService.AddServicesToProviderAsync(provider.IdProvider, new List<Guid> { selectedService.id });

            return response.IsSuccessStatusCode;
        }
    }
}
