using Job_Match.Data.Models.Dtos;
using Job_Match.Maui.ViewModels;
using Newtonsoft.Json;
using System.Globalization;
using ApiServiceClass = Job_Match.Maui.ApiService.ApiService;

namespace Job_Match.Maui.Views;

[QueryProperty(nameof(OfferJson), "data")]
public partial class EditOffer : ContentPage
{
    public string OfferJson
    {
        set
        {
            var offer = JsonConvert.DeserializeObject<TaskRequestWithProvidersDto>(Uri.UnescapeDataString(value));
            BindingContext = new EditOfferViewModel(offer);
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ajusta la decoración del Login
        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            PathAndroid.IsVisible = true;
            PathWin.IsVisible = false;
        }
        else if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            PathAndroid.IsVisible = false;
            PathWin.IsVisible = true;
        }

    }

    public EditOffer()
    {
        InitializeComponent();
    }

    private async void btnEditOffer_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is not EditOfferViewModel viewModel)
            return;

        // Validaciones básicas
        if (!viewModel.IsTitleValid || !viewModel.IsDescriptionValid || !viewModel.IsDateStartValid)
        {
            await DisplayAlert("Error", "Por favor completa los campos obligatorios.", "Aceptar");
            return;
        }

        // Construir DTO de actualización
        var updateDto = new UpdateTaskRequestDto
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Description = viewModel.Description,
            IdPostalCode = viewModel.SelectedPostalCode?.id ?? Guid.Empty,
            IdService = viewModel.SelectedService?.id ?? Guid.Empty,
            IdStatus = viewModel.SelectedTaskStatus?.id ?? Guid.Empty,
            DateStart = DateTime.ParseExact(viewModel.DateStart, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            DateEnd = string.IsNullOrWhiteSpace(viewModel.DateEnd)
                ? (DateTime?)null
                : DateTime.ParseExact(viewModel.DateEnd, "dd/MM/yyyy", CultureInfo.InvariantCulture)
        };

        // Llamar al ApiService
        var success = await new ApiServiceClass().UpdateOfferAsync(updateDto);

        if (success)
        {
            await DisplayAlert("Éxito", "Oferta actualizada correctamente.", "Aceptar");
            await Shell.Current.GoToAsync("OfferClient");
        }
        else
        {
            await DisplayAlert("Error", "No se pudo actualizar la oferta.", "Aceptar");
        }
    }

    private void radBtnServices_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is RadioButton radioButton && e.Value)
        {
            var selectedService = radioButton.BindingContext as SelectableService;
            if (selectedService != null && BindingContext is EditOfferViewModel viewModel)
            {
                viewModel.SelectedService = selectedService;

                // Desmarca los demás si es necesario
                foreach (var service in viewModel.FilteredServices)
                {
                    service.IsSelected = service == selectedService;
                }
            }
        }
    }

    private void eDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue.Length;
        lblCharCount.Text = $"{count}/1000";
    }
}