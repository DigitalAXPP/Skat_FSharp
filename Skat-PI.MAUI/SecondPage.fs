module SecondPage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

type Model = { 
        Password: string
        ConfirmPassword: string
    }

type Msg =
        | PasswordChanged of string
        | ConfirmPasswordChanged of string

let init () = {
        Password = ""
        ConfirmPassword = ""
    }

let update msg model =
        match msg with
        | PasswordChanged pwd -> { model with Password = pwd}, Cmd.none
        | ConfirmPasswordChanged pwd -> { model with ConfirmPassword = pwd}, Cmd.none

let view model =
        //Application(
            ContentPage(
                (VStack (spacing =25.) {
                    Label($"Hello, xxx")
                        .semantics(SemanticHeadingLevel.Level1)
                        .font(size = 32.)
                        .centerTextHorizontal()

                    //Button("Click", Clicked)
                    Entry(model.Password, PasswordChanged)
                    Entry(model.ConfirmPassword, ConfirmPasswordChanged)
                })
            )
        //)