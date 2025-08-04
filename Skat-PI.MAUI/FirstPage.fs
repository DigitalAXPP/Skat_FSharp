module firstpage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View


type Intent =
    | DoNothing
    | SecondStep

type Model = { Name: string }

type Msg = 
    | Clicked
    | NextPage

let init () = { Name = "Alex"}

let update msg model =
        match msg with
        | Clicked -> { model with Name = "Verca"}, Cmd.none, DoNothing
        | NextPage -> { model with Name = "xx" }, Cmd.none, SecondStep

let view model =
            ContentPage(
                (VStack (spacing =25.) {
                    Label($"Hello, {model.Name}")
                        .semantics(SemanticHeadingLevel.Level1)
                        .font(size = 32.)
                        .centerTextHorizontal()

                    Button("Click", Clicked)

                    Button("Go 2nd page", NextPage)
                })
            )