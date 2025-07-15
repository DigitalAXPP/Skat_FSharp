namespace Skat_PI.MAUI

open Foundation
open Microsoft.Maui

[<Register("AppDelegate")>]
type AppDelegate() =
    inherit MauiUIApplicationDelegate()

    override _.CreateMauiApp() = MauiProgram.CreateMauiApp()
