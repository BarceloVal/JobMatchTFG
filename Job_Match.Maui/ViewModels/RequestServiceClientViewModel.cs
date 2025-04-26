using Job_Match.Data.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;

namespace Job_Match.Maui.ViewModels
{
    public class RequestServiceClientViewModel : INotifyPropertyChanged
    {
        public RequestServiceClientViewModel()
        {
            PostalCodes = new ObservableCollection<m_postal_code>();
            ServiceTypes = new ObservableCollection<m_service_type>();
            FilteredServices = new ObservableCollection<m_service>();

            LoadPostalCodesAsync();
            LoadServiceTypesAsync();
            LoadServicesAsync();
        }

        // Evento que notifica a la interfaz de usuario cuando una propiedad cambia
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para lanzar el evento PropertyChanged
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ApiServiceClass _apiService = new();

        // -------------------
        // ---- Población ----
        // -------------------
        public ObservableCollection<m_postal_code> PostalCodes { get; set; }

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

        // Método que carga los códigos postales
        private async void LoadPostalCodesAsync()
        {
            var result = await _apiService.GetPostalCodesAsync(); // Este método está en ApiService
            if (result != null)
            {
                PostalCodes.Clear();
                foreach (var code in result)
                {
                    PostalCodes.Add(code);
                }
            }
        }

        // --------------------------
        // ---- Tipo de servicio ----
        // --------------------------

        // Todos los tipos de servicios
        public ObservableCollection<m_service_type> ServiceTypes { get; set; }

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
                    FilterServices();
                }
            }
        }

        // Método que carga los tipos de servicios
        private async void LoadServiceTypesAsync()
        {
            var result = await _apiService.GetServiceTypesAsync();
            if (result != null)
            {
                ServiceTypes.Clear();
                foreach (var code in result)
                {
                    ServiceTypes.Add(code);
                }
            }
        }

        // ------------------
        // ---- Servicio ----
        // ------------------

        // Todos los servicios (sin filtrar)
        private List<m_service> _allServices = new();

        // Servicios filtrados
        public ObservableCollection<m_service> FilteredServices { get; set; } = new();

        public ObservableCollection<m_service> Services { get; set; }

        private m_service _selectedService;
        public m_service SelectedService
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

        // Método que carga los servicios
        private async void LoadServicesAsync()
        {
            var result = await _apiService.GetServicesAsync();
            if (result != null)
            {
                _allServices = result.ToList();
                FilteredServices.Clear();
                foreach (var service in _allServices)
                {
                    FilteredServices.Add(service);
                }
            }
        }

        private void FilterServices()
        {
            if (SelectedServiceType == null)
                return;

            var filtered = _allServices
                .Where(s => s.id_service_type == SelectedServiceType.id)
                .ToList();

            FilteredServices.Clear();
            foreach (var service in filtered)
                FilteredServices.Add(service);
        }

    }
}
