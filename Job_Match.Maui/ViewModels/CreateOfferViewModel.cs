using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using Job_Match.Maui.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;

namespace Job_Match.Maui.ViewModels
{
    public class CreateOfferViewModel : INotifyPropertyChanged
    {
        public CreateOfferViewModel()
        {
            LoadPostalCodesAsync();
            LoadServiceTypesAsync();
            LoadServicesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        // Lanza el evento cuando una propiedad cambia
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ApiServiceClass _apiService = new();

        // ----------------------
        // ------- Título -------
        // ----------------------
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                    ValidateTitle();
                }
            }
        }

        private string _titleError;
        public string TitleError
        {
            get => _titleError;
            set
            {
                _titleError = value;
                OnPropertyChanged(nameof(TitleError));
                OnPropertyChanged(nameof(IsTitleValid));
            }
        }

        public bool IsTitleValid => string.IsNullOrEmpty(TitleError);

        private void ValidateTitle()
        {
            if (string.IsNullOrWhiteSpace(Title))
                TitleError = "Campo obligatorio.";
            else if (Title.Length > 100)
                TitleError = "Máximo 100 caracteres.";
            else
                TitleError = string.Empty;
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

        // ------------------------
        // --- Tipo de servicio ---
        // ------------------------
        public class ServiceTypeSelectable : m_service_type
        {
            public bool IsSelected { get; set; } = false;
        }

        // Tipo de servicio (Checkbox)
        public ObservableCollection<ServiceTypeSelectable> ServiceTypes { get; set; } = new();

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

        private void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Description))
                DescriptionError = "Campo obligatorio.";
            else if (Description.Length > 1000)
                DescriptionError = "Máximo 1000 caracteres.";
            else
                DescriptionError = string.Empty;
        }

        // ---------------------
        // --- Fecha Inicio ----
        // ---------------------
        private string _dateStart;
        public string DateStart
        {
            get => _dateStart;
            set
            {
                if (_dateStart != value)
                {
                    _dateStart = value;
                    OnPropertyChanged();
                    ValidateDateStart();
                }
            }
        }

        private string _dateStartError;
        public string DateStartError
        {
            get => _dateStartError;
            set
            {
                _dateStartError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDateStartValid));
            }
        }

        public bool IsDateStartValid => string.IsNullOrEmpty(DateStartError);

        private void ValidateDateStart()
        {
            if (string.IsNullOrWhiteSpace(DateStart))
                DateStartError = "Campo obligatorio.";
            else if (!DateTime.TryParseExact(DateStart, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                DateStartError = "Formato inválido. Usa dd/MM/aaaa.";
            else
                DateStartError = string.Empty;
        }

        // ----------------------------
        // --- Fecha Fin (opcional) ---
        // ----------------------------
        private string _dateEnd;
        public string DateEnd
        {
            get => _dateEnd;
            set
            {
                if (_dateEnd != value)
                {
                    _dateEnd = value;
                    OnPropertyChanged();
                    ValidateDateEnd();
                }
            }
        }

        private string _dateEndError;
        public string DateEndError
        {
            get => _dateEndError;
            set
            {
                _dateEndError = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDateEndValid));
            }
        }

        public bool IsDateEndValid => string.IsNullOrEmpty(DateEndError);

        private void ValidateDateEnd()
        {
            if (string.IsNullOrWhiteSpace(DateEnd))
            {
                DateEndError = string.Empty; // opcional
                return;
            }

            if (!DateTime.TryParseExact(DateEnd, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                DateEndError = "Formato inválido. Usa dd/MM/aaaa.";
            else
                DateEndError = string.Empty;
        }

        private async Task<Guid> GetOpenStatusIdAsync()
        {
            var statuses = await _apiService.GetStatusAsync();
            var openStatus = statuses?.FirstOrDefault(s => s.name.ToLower() == "abierta");
            return openStatus?.id ?? Guid.Empty; // o lanzá una excepción si es crítico
        }
        public async Task<bool> CreateOfferAsync()
        {
            var session = UserSessionService.Instance;

            // Validaciones antes de continuar
            ValidateTitle();
            ValidateDescription();
            ValidateDateStart();
            ValidateDateEnd();

            if (!IsTitleValid || !IsDescriptionValid || !IsDateStartValid || !IsDateEndValid)
                return false;

            var selectedService = FilteredServices.FirstOrDefault(s => s.IsSelected);
            if (selectedService == null)
            {
                await Shell.Current.DisplayAlert("Error", "Debes seleccionar un servicio.", "Aceptar");
                return false;
            }

            if (session == null)
            {
                await Shell.Current.DisplayAlert("Error", "No se encontró el usuario actual.", "Aceptar");
                return false;
            }

            Console.WriteLine($"[DEBUG] session.Id = {session.Id}");
            Console.WriteLine($"[DEBUG] session.Id IsEmpty? {session.Id == Guid.Empty}");

            var dto = new CreateTaskRequestDto
            {
                IdUserClient = session.IdUserClient ?? Guid.Empty,
                IdPostalCode = SelectedPostalCode.id,
                IdService = selectedService.id,
                IdStatus = await GetOpenStatusIdAsync(),
                Title = Title,
                Description = Description,
                DateStart = DateTime.ParseExact(DateStart, "dd/MM/yyyy", null),
                DateEnd = string.IsNullOrWhiteSpace(DateEnd) ? null : DateTime.ParseExact(DateEnd, "dd/MM/yyyy", null)
            };

            Console.WriteLine("=== DATOS ENVIADOS A LA API ===");
            Console.WriteLine($"IdUserClient: {dto.IdUserClient}");
            Console.WriteLine($"IdPostalCode: {dto.IdPostalCode}");
            Console.WriteLine($"IdService: {dto.IdService}");
            Console.WriteLine($"IdStatus: {dto.IdStatus}");
            Console.WriteLine($"Title: {dto.Title}");
            Console.WriteLine($"Description: {dto.Description}");
            Console.WriteLine($"DateStart: {dto.DateStart}");
            Console.WriteLine($"DateEnd: {(dto.DateEnd.HasValue ? dto.DateEnd.Value.ToString() : "null")}");
            Console.WriteLine("===============================");

            var result = await _apiService.CreateTaskRequestAsync(dto);
            if (result)
            {
                await Shell.Current.DisplayAlert("Éxito", "Oferta creada correctamente.", "Aceptar");
                await Shell.Current.GoToAsync("OfferClient");
                return true;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "No se pudo crear la oferta.", "Aceptar");
                return false;
            }
        }
    }
}
