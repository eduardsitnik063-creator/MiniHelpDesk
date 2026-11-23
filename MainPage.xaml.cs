using MiniHelpDesk.Domain;
using MiniHelpDesk.Presentation.ViewModels;
using MiniHelpDesk.Tests;

namespace MiniHelpDesk.App;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    private readonly bool _isMobile;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        _isMobile = DeviceInfo.Platform == DevicePlatform.Android;

        if (_isMobile)
        {
            MobileNavigation.IsVisible = true;
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            Grid.SetColumn(TicketsPanel, 0);
            ShowForm();
        }
    }

    private void ShowForm()
    {
        FormPanel.IsVisible = true;
        TicketsPanel.IsVisible = !_isMobile;
    }

    private void ShowList()
    {
        if (!_isMobile) return;
        FormPanel.IsVisible = false;
        TicketsPanel.IsVisible = true;
    }

    private void OnShowFormClicked(object sender, EventArgs e) => ShowForm();
    private void OnShowListClicked(object sender, EventArgs e) => ShowList();
    private void OnSaveClicked(object sender, EventArgs e)
    {
        if (_viewModel.TrySave()) ShowList();
    }

    private void OnCancelClicked(object sender, EventArgs e) => _viewModel.ResetForm();
    private void OnEditClicked(object sender, EventArgs e)
    {
        _viewModel.StartEditing((sender as Button)?.BindingContext as Ticket);
        ShowForm();
    }
    private void OnDeleteClicked(object sender, EventArgs e)
        => _viewModel.Delete((sender as Button)?.BindingContext as Ticket);
    private void OnStatusClicked(object sender, EventArgs e)
        => _viewModel.AdvanceStatus((sender as Button)?.BindingContext as Ticket);

    private async void OnRunTestsClicked(object sender, EventArgs e)
    {
        string report = InAppTestRunner.RunAll();
        await DisplayAlertAsync("Testy MSTest (wewnątrz MAUI)", report, "OK");
    }
}
