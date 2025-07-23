module SecondPage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

type Model = { Name: string }

type Msg = | Clicked

let init () = { Name = "Alex"}

let update msg model =
        match msg with
        | Clicked -> { model with Name = "Verca"}

let view model =
        Application(
            ContentPage(
                (VStack (spacing =25.) {
                    Label($"Hello, {model.Name}")
                        .semantics(SemanticHeadingLevel.Level1)
                        .font(size = 32.)
                        .centerTextHorizontal()

                    Button("Click", Clicked)
                })
            )
        )