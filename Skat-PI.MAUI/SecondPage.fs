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
    | ForwardThirdpage

type Model = { 
        Password: string
        ConfirmPassword: string
    }

type Msg =
        | PasswordChanged of string
        | ConfirmPasswordChanged of string
        | ReturnFirstPage
        | GoThirdPage

let init () = {
        Password = ""
        ConfirmPassword = ""
    }

let update msg model =
        match msg with
        | PasswordChanged pwd -> { model with Password = pwd}, Cmd.none, DoNothing
        | ConfirmPasswordChanged pwd -> { model with ConfirmPassword = pwd}, Cmd.none, DoNothing
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage
        | GoThirdPage -> model, Cmd.none, ForwardThirdpage

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

                        Button("1st page", ReturnFirstPage)

                        Button("3rd page", GoThirdPage)

                        let cards = [ {Suite = Hearts; Rank = Eight}; {Suite = Clubs; Rank = Dame} ]
                        ListView(cards)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))                            

                        Entry(model.Password, PasswordChanged)
                        Entry(model.ConfirmPassword, ConfirmPasswordChanged)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )