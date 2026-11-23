using Foundation;

namespace MiniHelpDesk;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MiniHelpDesk.App.MauiProgram.CreateMauiApp();
}
