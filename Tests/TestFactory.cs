using Microsoft.Extensions.DependencyInjection;
using MiniHelpDesk.App.ApplicationSetup;
using MiniHelpDesk.Presentation.ViewModels;

namespace MiniHelpDesk.Tests;

public static class TestFactory
{
    public static ServiceProvider CreateProvider()
        => new ServiceCollection().AddMiniHelpDesk().BuildServiceProvider();

    public static MainViewModel CreateViewModel(ServiceProvider provider)
        => provider.GetRequiredService<MainViewModel>();

    public static void FillValid(MainViewModel vm)
    {
        vm.TitleInput = "Awaria komputera";
        vm.DescriptionInput = "Komputer się nie uruchamia.";
        vm.SelectedPriority = "Średni";
    }
}
