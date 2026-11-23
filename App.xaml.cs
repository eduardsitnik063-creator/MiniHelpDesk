using Microsoft.Extensions.DependencyInjection;

namespace MiniHelpDesk.App;

public class App : Microsoft.Maui.Controls.Application
{
    private readonly MainPage _mainPage;

    public App(IServiceProvider services)
    {
        _mainPage = services.GetRequiredService<MainPage>();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(_mainPage) { Title = "Mini HelpDesk" };
#if WINDOWS
        window.Width = 1000;
        window.Height = 650;
#endif
        return window;
    }
}
