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

            LoadPostalCodesAsync();
            LoadServiceTypesAsync();
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
    }
}
