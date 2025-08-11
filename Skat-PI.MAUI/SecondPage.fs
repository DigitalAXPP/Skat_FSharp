module SecondPage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

open GameFoundation


type Intent =
    | DoNothing
    | BackFirstPage

type Model = { 
        Password: string
        ConfirmPassword: string
    }

type Msg =
        | PasswordChanged of string
        | ConfirmPasswordChanged of string
        | ReturnFirstPage

let init () = {
        Password = ""
        ConfirmPassword = ""
    }

let update msg model =
        match msg with
        | PasswordChanged pwd -> { model with Password = pwd}, Cmd.none, DoNothing
        | ConfirmPasswordChanged pwd -> { model with ConfirmPassword = pwd}, Cmd.none, DoNothing
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage

let view model =
            ContentPage(
                ScrollView(
                    (VStack (spacing =25.) {
                        Label($"Hello, xxx")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Label($"Hello, {playerOne.Player}. You are at position {playerOne.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Button("Click", ReturnFirstPage)

                        let items = [ 1..100 ]
                        ListView(items)
                            (fun item -> TextCell($"{item}"))

                    

                        Entry(model.Password, PasswordChanged)
                        Entry(model.ConfirmPassword, ConfirmPasswordChanged)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )