using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class EditProfileProviderViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Llama al evento PropertyChanged para actualizar la interfaz de usuario cuando una propiedad cambia.
        /// </summary>
        /// <param name="propName">Nombre de la propiedad que cambió. Se asigna automáticamente si no se especifica.</param>
        private void OnPropertyChanged([CallerMemberName] string propName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        private readonly ApiService.ApiService _apiService = new();

        private ProfileProviderDto _profileDto;

        public Guid Id { get; set; }
        public Guid IdUser { get; set; }

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

        // --------------------------------
        // ------- Tipo Documento -------
        // --------------------------------
        public ObservableCollection<m_doc_type> DocTypes { get; set; } = new();

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
            var tipo = SelectedDocType?.id_mask?.ToUpper();

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

        private m_doc_type _selectedDocType;
        public m_doc_type SelectedDocType
        {
            get => _selectedDocType;
            set
            {
                if (_selectedDocType != value)
                {
                    _selectedDocType = value;
                    OnPropertyChanged();
                    ValidateDocument();

                    if (value != null)
                    {
                        DocTypeName = value.name; // o value.id_mask si prefieres
                    }
                }
            }
        }

        private string _docTypeName;
        public string DocTypeName
        {
            get => _docTypeName;
            set
            {
                if (_docTypeName != value)
                {
                    _docTypeName = value;
                    OnPropertyChanged();
                }
            }
        }

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

                    if (value != null)
                    {
                        PostalCodeName = $"{value.cod_auto_name}/{value.cod_pro_name}/{value.cod_loc_name}";
                    }
                }
            }
        }

        private string _postalCodeName;
        public string PostalCodeName
        {
            get => _postalCodeName;
            set
            {
                if (_postalCodeName != value)
                {
                    _postalCodeName = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<m_service_type> ServiceTypes { get; } = new();
        public ObservableCollection<m_service> Services { get; } = new();

        private m_service_type _selectedServiceType;
        public m_service_type SelectedServiceType
        {
            get => _selectedServiceType;
            set
            {
                _selectedServiceType = value;
                OnPropertyChanged();
                _ = LoadServicesByTypeAsync();
            }
        }

        public ObservableCollection<ServiceSelectable> FilteredServices { get; } = new();

        private ServiceSelectable _selectedService;
        public ServiceSelectable SelectedService
        {
            get => _selectedService;
            set
            {
                if (_selectedService != value)
                {
                    _selectedService = value;
                    OnPropertyChanged();
                }
            }
        }

        public EditProfileProviderViewModel(ProfileProviderDto dto)
        {
            _profileDto = dto;
            Id = dto.IdProvider;
            IdUser = dto.IdUser;
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Document = dto.Document;
            Website = dto.Website;
            MapCoordinates = dto.MapCoordinates;
            Address = dto.Address;
            Phone1 = dto.Phone1;
            Phone2 = dto.Phone2;
            TradeName = dto.TradeName;
            Description = dto.Description;
            PostalCodeName = dto.PostalCodeName;
        }

        /// <summary>
        /// Inicializa los datos necesarios para la vista de edición, como los tipos de documento y códigos postales.
        /// Además, selecciona el tipo de documento y código postal correspondientes al DTO original.
        /// </summary>
        public async Task InitAsync()
        {
            var docTypes = await _apiService.GetDocTypesAsync();
            if (docTypes is not null)
            {
                DocTypes.Clear();
                foreach (var dt in docTypes)
                {
                    DocTypes.Add(dt);
                    if (dt.id == _profileDto.IdDocType)
                        SelectedDocType = dt;
                }
            }

            var postalCodes = await _apiService.GetPostalCodesAsync();
            if (postalCodes is not null)
            {
                PostalCodes.Clear();
                foreach (var pc in postalCodes)
                {
                    PostalCodes.Add(pc);
                    var fullName = $"{pc.cod_auto_name}/{pc.cod_pro_name}/{pc.cod_loc_name}";
                    if (pc.id == _profileDto.IdPostalCode)
                    {
                        SelectedPostalCode = pc;
                        PostalCodeName = fullName;
                    }
                }
            }

            // Cargar tipos de servicio
            var serviceTypes = await _apiService.GetServiceTypesAsync();
            if (serviceTypes != null)
            {
                ServiceTypes.Clear();
                foreach (var type in serviceTypes)
                {
                    ServiceTypes.Add(type);
                    if (type.name == _profileDto.Services.FirstOrDefault()?.ServiceTypeName)
                        SelectedServiceType = type;
                }
            }

            // Cargar servicios del tipo seleccionado
            if (SelectedServiceType != null)
            {
                var allServices = await _apiService.GetServicesAsync();
                if (allServices is not null && _profileDto.Services?.Any() == true)
                {
                    var currentServiceName = _profileDto.Services.First().ServiceName;

                    FilteredServices.Clear();
                    foreach (var svc in allServices.Where(s => s.id_service_type == SelectedServiceType.id))
                    {
                        var selectable = new ServiceSelectable
                        {
                            id = svc.id,
                            id_service_type = svc.id_service_type,
                            name = svc.name,
                            description = svc.description,
                            active = svc.active,
                            IsSelected = svc.name == currentServiceName
                        };

                        if (selectable.IsSelected)
                            SelectedService = selectable;

                        FilteredServices.Add(selectable);
                    }
                }
            }
        }

        /// <summary>
        /// Valida todos los datos introducidos.
        /// </summary>
        public async Task<bool> ValidateAllAsync()
        {
            ValidateFirstName();
            ValidateLastName();
            ValidateTradeName();
            ValidateDocument();
            ValidateAddress();
            ValidatePhone1();
            ValidatePhone2();
            ValidateWebsite();
            ValidateMapCoordinates();
            ValidateDescription();

            bool isPostalCodeSelected = SelectedPostalCode != null && SelectedPostalCode.id != Guid.Empty;
            if (!isPostalCodeSelected)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debes seleccionar un código postal.", "OK");
                return false;
            }

            return IsFirstNameValid && IsLastNameValid && IsTradeNameValid &&
                   IsDocumentValid && IsAddressValid &&
                   IsPhone1Valid && IsPhone2Valid &&
                   IsWebsiteValid && IsMapCoordinatesValid && IsDescriptionValid;
        }


        /// <summary>
        /// Construye un objeto DTO (`UpdateProfileCProviderDto`) con los datos actuales del ViewModel,
        /// preparado para ser enviado a la API para actualizar el perfil del proveedor.
        /// </summary>
        public UpdateProfileProviderDto BuildUpdateDto()
        {
            Console.WriteLine($"SelectedPostalCode: {(SelectedPostalCode != null ? SelectedPostalCode.id.ToString() : "NULL")}");

            if (SelectedPostalCode == null)
            {
                return null;
            }

            return new UpdateProfileProviderDto
            {
                Id = Id,
                IdUser = IdUser,
                FirstName = FirstName,
                LastName = LastName,
                TradeName = TradeName,
                Document = Document,
                Address = Address,
                Website = string.IsNullOrWhiteSpace(Website) ? null : Website.Trim(),
                MapCoordinates = string.IsNullOrWhiteSpace(MapCoordinates) ? null : MapCoordinates.Trim(),
                Phone1 = Phone1,
                Phone2 = Phone2,
                IdDocType = SelectedDocType?.id ?? Guid.Empty,
                IdPostalCode = SelectedPostalCode?.id ?? Guid.Empty,
                IdService = SelectedService?.id,
                Description = Description // <--- FALTABA ESTA LÍNEA
            };
        }

        // Este método se llama automáticamente al seleccionar ServiceType
        async Task LoadServicesByTypeAsync()
        {
            var allServices = await _apiService.GetServicesAsync();
            FilteredServices.Clear();

            foreach (var svc in allServices.Where(s => s.id_service_type == SelectedServiceType?.id))
            {
                var selectable = new ServiceSelectable
                {
                    id = svc.id,
                    id_service_type = svc.id_service_type,
                    name = svc.name,
                    description = svc.description,
                    active = svc.active,
                    IsSelected = svc.name == _profileDto.Services.FirstOrDefault()?.ServiceName
                };

                if (selectable.IsSelected)
                    SelectedService = selectable;

                FilteredServices.Add(selectable);
            }
        }

        public class ServiceSelectable : m_service
        {
            public bool IsSelected { get; set; }
        }
    }
}
