
using Job_Match.Maui.ViewModels;

namespace Job_Match.Maui.Views;

public partial class ChangeProfileToProvider : ContentPage
{
    private readonly ChangeProfileToProviderViewModel _viewModel;

    public ChangeProfileToProvider()
    {
        InitializeComponent();
        _viewModel = new ChangeProfileToProviderViewModel();
        BindingContext = _viewModel;
    }

    private void radBtnServices_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (!e.Value)
            return;

        var selectedService = (sender as RadioButton)?.BindingContext as CreateProviderAccountViewModel.ServiceSelectable;
        if (selectedService == null)
            return;

        var vm = BindingContext as CreateProviderAccountViewModel;
        if (vm == null)
            return;

        foreach (var service in vm.FilteredServices)
        {
            service.IsSelected = service == selectedService;
        }
    }

    private async void btnChangeProvider_Clicked(object sender, EventArgs e)
    {
        var result = await _viewModel.ChangeToProviderAsync();

        if (result)
        {
            await Shell.Current.DisplayAlert("Éxito", "Tu perfil se ha actualizado como proveedor.", "Aceptar");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Corrige los errores e intenta de nuevo.", "Aceptar");
        }

    }

    private void eDescription_TextChanged(object sender, TextChangedEventArgs e)
    {
        int count = e.NewTextValue.Length;
        lblCharCount.Text = $"{count}/1000";
    }
}