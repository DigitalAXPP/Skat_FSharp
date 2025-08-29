module ThirdPage

open Microsoft.Maui
open Microsoft.Maui.Controls
open Fabulous
open Fabulous.Maui
open type Fabulous.Maui.View
open GamePlay
open GameFoundation

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
                        let cardSetup = dealInitialHand Deck

                        Label($"Hello, {playerOne.Player}. You are at position {playerOne.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        ListView(cardSetup.FirstPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))   

                        Label($"Hello, {playerTwo.Player}. You are at position {playerTwo.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        ListView(cardSetup.SecondPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))  

                        Label($"Hello, {playerThree.Player}. You are at position {playerThree.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        ListView(cardSetup.ThirdPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))  

                        Button($"1st page - {model.Name}", ReturnFirstPage)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )