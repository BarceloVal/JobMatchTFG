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

        // Título
        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); ValidateTitle(); }
        }

        private bool _isTitleValid = true;
        public bool IsTitleValid
        {
            get => _isTitleValid;
            set { _isTitleValid = value; OnPropertyChanged(); }
        }

        public string TitleError => "El título es obligatorio.";

        /// <summary>
        /// Valida que el campo Título no esté vacío.
        /// </summary>
        private void ValidateTitle()
        {
            IsTitleValid = !string.IsNullOrWhiteSpace(Title);
        }

        // Estado
        public ObservableCollection<m_task_status> TaskStatuses { get; set; } = new();

        private m_task_status _selectedTaskStatus;
        public m_task_status SelectedTaskStatus
        {
            get => _selectedTaskStatus;
            set { _selectedTaskStatus = value; OnPropertyChanged(); }
        }

        // Descripción
        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); ValidateDescription(); }
        }

        private bool _isDescriptionValid = true;
        public bool IsDescriptionValid
        {
            get => _isDescriptionValid;
            set { _isDescriptionValid = value; OnPropertyChanged(); }
        }

        public string DescriptionError => "La descripción es obligatoria.";

        /// <summary>
        /// Valida que el campo Descripción no esté vacío.
        /// </summary>
        private void ValidateDescription()
        {
            IsDescriptionValid = !string.IsNullOrWhiteSpace(Description);
        }

        // Fechas
        public string DateStart { get; set; }
        public string DateEnd { get; set; }

        public string DateStartError => "Fecha inválida.";
        public string DateEndError => "Fecha inválida.";
        public bool IsDateStartValid => DateTime.TryParse(DateStart, out _);
        public bool IsDateEndValid => string.IsNullOrEmpty(DateEnd) || DateTime.TryParse(DateEnd, out _);

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
