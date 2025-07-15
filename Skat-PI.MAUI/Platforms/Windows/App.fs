namespace Skat_PI.MAUI.WinUI

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
type App() =
    inherit FSharp.Maui.WinUICompat.App()

    override this.CreateMauiApp() = Skat_PI.MAUI.MauiProgram.CreateMauiApp()
