namespace Skat_PI.MAUI.WinUI

open System

module Program =
    [<EntryPoint; STAThread>]
    let main args =
        do FSharp.Maui.WinUICompat.Program.Main(args, typeof<Skat_PI.MAUI.WinUI.App>)
        0
