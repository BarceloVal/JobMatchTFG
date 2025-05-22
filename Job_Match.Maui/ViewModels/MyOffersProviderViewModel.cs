using Job_Match.Data.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Job_Match.Maui.ViewModels
{
    public class MyOffersProviderViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SubscribedOfferDto> MyOffers { get; set; } = new();

        private readonly Job_Match.Maui.ApiService.ApiService _api = new();

        public ObservableCollection<TaskStatusDto> TaskStatuses { get; set; } = new();
        private TaskStatusDto? _selectedTaskStatus;
        public TaskStatusDto? SelectedTaskStatus
        {
            get => _selectedTaskStatus;
            set
            {
                if (_selectedTaskStatus != value)
                {
                    _selectedTaskStatus = value;
                    OnPropertyChanged(nameof(SelectedTaskStatus));
                }
            }
        }

        public async Task LoadMyOffersAsync(Guid idUserProvider)
        {
            var result = await _api.GetOffersByProviderAsync(idUserProvider);

            MyOffers.Clear();

            // Si hay un estado seleccionado, filtrar por nombre del estado
            var filtered = SelectedTaskStatus != null
                ? result.Where(o => o.Status == SelectedTaskStatus.Name)
                : result;

            foreach (var offer in filtered)
                MyOffers.Add(offer);
        }

        public async Task QuitOfferAsync(Guid idProvider, Guid idTaskRequest)
        {
            var dto = new QuitOfferDto
            {
                IdUserProvider = idProvider,
                IdTaskRequest = idTaskRequest
            };

            await _api.PostAsync("api/offer-subscriptions/quit", dto);
            await LoadMyOffersAsync(idProvider); // Recargar después de quitar
        }

        public async Task LoadTaskStatusesAsync()
        {
            var result = await _api.GetAsync<List<TaskStatusDto>>("api/task-status");
            TaskStatuses.Clear();
            foreach (var status in result)
                TaskStatuses.Add(status);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }

}
