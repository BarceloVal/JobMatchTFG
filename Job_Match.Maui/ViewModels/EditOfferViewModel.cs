using Job_Match.Data.Models;
using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ApiService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class EditOfferViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica a la UI que una propiedad ha cambiado su valor.
        /// </summary>
        /// <param name="name">Nombre de la propiedad modificada (autodetectado).</param>
        private void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private readonly Job_Match.Maui.ApiService.ApiService _apiService = new();

        // ID original de la oferta (necesario para actualizar)
        public Guid Id { get; set; }

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

        /// <summary>
        /// Valida el título de la oferta. Obligatorio, máximo 100 caracteres.
        /// </summary>
        private void ValidateTitle()
        {
            if (string.IsNullOrWhiteSpace(Title))
                TitleError = "Campo obligatorio.";
            else if (Title.Length > 100)
                TitleError = "Máximo 100 caracteres.";
            else
                TitleError = string.Empty;
        }

        // ----------------
        // ---- Estado ----
        // ----------------
        public ObservableCollection<m_task_status> TaskStatuses { get; set; } = new();

        private m_task_status _selectedTaskStatus;
        public m_task_status SelectedTaskStatus
        {
            get => _selectedTaskStatus;
            set { _selectedTaskStatus = value; OnPropertyChanged(); }
        }

        private string _taskStatusError;
        public string TaskStatusError
        {
            get => _taskStatusError;
            set
            {
                _taskStatusError = value;
                OnPropertyChanged(nameof(TaskStatusError));
                OnPropertyChanged(nameof(IsTaskStatusValid));
            }
        }

        public bool IsTaskStatusValid => string.IsNullOrEmpty(TaskStatusError);

        private void ValidateTaskStatus()
        {
            TaskStatusError = SelectedTaskStatus == null ? "Debes seleccionar un estado." : string.Empty;
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
        /// Valida la descripción. Obligatoria, máximo 1000 caracteres.
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

        /// <summary>
        /// Valida la fecha de inicio. Obligatoria y debe tener el formato "dd/MM/yyyy".
        /// </summary>
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

        /// <summary>
        /// Valida la fecha de fin. Es opcional pero si se especifica debe tener formato válido.
        /// </summary>
        private void ValidateDateEnd()
        {
            if (string.IsNullOrWhiteSpace(DateEnd))
            {
                DateEndError = string.Empty;
                return;
            }

            if (!DateTime.TryParseExact(DateEnd, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                DateEndError = "Formato inválido. Usa dd/MM/aaaa.";
            else
                DateEndError = string.Empty;
        }

        // -------------------
        // ---- Población ----
        // -------------------

        public ObservableCollection<m_postal_code> PostalCodes { get; set; } = new();
        private m_postal_code _selectedPostalCode;
        public m_postal_code SelectedPostalCode
        {
            get => _selectedPostalCode;
            set { _selectedPostalCode = value; OnPropertyChanged(); }
        }

        // --------------------------
        // ---- Tipo de servicio ----
        // --------------------------

        public ObservableCollection<m_service_type> ServiceTypes { get; set; } = new();
        private m_service_type _selectedServiceType;
        public m_service_type SelectedServiceType
        {
            get => _selectedServiceType;
            set { _selectedServiceType = value; OnPropertyChanged(); FilterServices(); }
        }

        // ------------------
        // ---- Servicio ----
        // ------------------

        private List<SelectableService> _allServices = new();
        public ObservableCollection<SelectableService> FilteredServices { get; set; } = new();

        public m_service SelectedService { get; set; }

        /// <summary>
        /// Constructor que inicializa el ViewModel con los datos de la oferta existente para edición.
        /// Carga códigos postales, tipos de servicio, servicios y estados desde la API.
        /// </summary>
        /// <param name="offer">DTO con los datos de la oferta a editar.</param>
        public EditOfferViewModel(TaskRequestWithProvidersDto offer)
        {
            Id = offer.Id;
            Title = offer.Title;
            Description = offer.Description;
            DateStart = offer.DateStart.ToString("dd/MM/yyyy");
            DateEnd = offer.DateEnd.HasValue ? offer.DateEnd.Value.ToString("dd/MM/yyyy") : "";

            LoadPostalCodesAsync(offer.PostalCodeName);
            LoadServiceTypesAsync(offer.ServiceType, offer.Service);
            LoadServicesAsync(offer.Service);
            LoadTaskStatusesAsync(offer.Status);
        }

        /// <summary>
        /// Carga todos los códigos postales disponibles desde la API y selecciona el correspondiente.
        /// </summary>
        /// <param name="postalCodeName">Nombre del código postal actual para seleccionarlo por defecto.</param>
        private async void LoadPostalCodesAsync(string postalCodeName)
        {
            var result = await _apiService.GetPostalCodesAsync();
            if (result != null)
            {
                PostalCodes.Clear();
                foreach (var code in result)
                {
                    PostalCodes.Add(code);
                    if (code.DisplayText == postalCodeName)
                        SelectedPostalCode = code;
                }
            }
        }

        /// <summary>
        /// Carga los tipos de servicio desde la API y selecciona el que corresponda a la oferta.
        /// </summary>
        /// <param name="serviceTypeName">Nombre del tipo de servicio actual.</param>
        /// <param name="serviceName">Nombre del servicio actual (para posterior uso).</param>
        private async void LoadServiceTypesAsync(string serviceTypeName, string serviceName)
        {
            var result = await _apiService.GetServiceTypesAsync();
            if (result != null)
            {
                ServiceTypes.Clear();
                foreach (var type in result)
                {
                    ServiceTypes.Add(type);
                    if (type.name == serviceTypeName)
                        SelectedServiceType = type;
                }
            }
        }

        /// <summary>
        /// Carga los servicios desde la API, los convierte a `SelectableService` y marca el servicio actual.
        /// </summary>
        /// <param name="selectedServiceName">Nombre del servicio seleccionado actualmente.</param>
        private async void LoadServicesAsync(string selectedServiceName)
        {
            var result = await _apiService.GetServicesAsync();
            if (result != null)
            {
                // Convertir cada m_service a SelectableService
                _allServices = result
                    .Select(s => new SelectableService
                    {
                        id = s.id,
                        id_service_type = s.id_service_type,
                        name = s.name,
                        // Copia otros campos si los tienes
                        IsSelected = false
                    }).ToList();

                FilterServices();

                SelectedService = _allServices.FirstOrDefault(s => s.name == selectedServiceName);
                if (SelectedService != null)
                {
                    var serviceInList = FilteredServices.FirstOrDefault(s => s.id == SelectedService.id);
                    if (serviceInList != null)
                    {
                        // Marcar como seleccionado (si tienes IsSelected en el modelo)
                        serviceInList.IsSelected = true;
                    }
                }
            }
        }

        /// <summary>
        /// Carga los posibles estados de tarea desde la API y selecciona el correspondiente.
        /// </summary>
        /// <param name="statusName">Nombre del estado actual de la oferta.</param>
        private async void LoadTaskStatusesAsync(string statusName)
        {
            var result = await _apiService.GetStatusAsync();
            if (result != null)
            {
                TaskStatuses.Clear();
                foreach (var status in result)
                {
                    TaskStatuses.Add(status);
                    if (status.name == statusName)
                        SelectedTaskStatus = status;
                }
            }
        }

        /// <summary>
        /// Filtra los servicios en base al tipo de servicio seleccionado.
        /// </summary>
        private void FilterServices()
        {
            if (SelectedServiceType == null) return;

            var filtered = _allServices
                .Where(s => s.id_service_type == SelectedServiceType.id)
                .ToList();

            FilteredServices.Clear();
            foreach (var s in filtered)
                FilteredServices.Add(s);
        }
    }

    public class SelectableService : m_service, INotifyPropertyChanged
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifica a la interfaz cuando cambia el estado de selección del servicio.
        /// </summary>
        /// <param name="name">Nombre de la propiedad que cambió.</param>
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}
