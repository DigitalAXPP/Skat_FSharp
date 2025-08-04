module SecondPage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

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
                (VStack (spacing =25.) {
                    Label($"Hello, xxx")
                        .semantics(SemanticHeadingLevel.Level1)
                        .font(size = 32.)
                        .centerTextHorizontal()

                    Button("Click", ReturnFirstPage)

                    Entry(model.Password, PasswordChanged)
                    Entry(model.ConfirmPassword, ConfirmPasswordChanged)
                })
            )