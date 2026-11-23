using MiniHelpDesk.App.ApplicationSetup;

namespace MiniHelpDesk.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddMiniHelpDesk();
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}
