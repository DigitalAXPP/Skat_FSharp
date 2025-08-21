module ThirdPage

open Microsoft.Maui
open Microsoft.Maui.Controls
open Fabulous
open Fabulous.Maui
open type Fabulous.Maui.View

type Intent =
    | DoNothing
    | BackFirstPage

type Model = { Name: string }

type Msg =
        | ReturnFirstPage

let init () = { Name = "Skat" }

let update msg model =
        match msg with
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage

let view model =
            ContentPage(
                ScrollView(
                    (VStack (spacing =25.) {
                        Button($"1st page - {model.Name}", ReturnFirstPage)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )