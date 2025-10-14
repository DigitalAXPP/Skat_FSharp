module FourPage

open Fabulous
open Fabulous.Maui
open type Fabulous.Maui.View
open Microsoft.Maui
open Microsoft.Maui.Controls
open System
open GameFoundation
open GamePlay
open Fabulous.Dispatcher

type Intent = 
    | DoNothing

type Model = {
    PlayerOne: PlayerConfig
    PlayerTwo: PlayerConfig
    PlayerThree: PlayerConfig
    SelectedCards: (PlayerId * Card) list option
    Game: GameType
    CardValue: int
}

type Msg =
    | ReturnFirstPage
    | ToggleCard of (PlayerId * Card) list option
    | SetCards
    | CompareCards
    | RemoveCard of (PlayerId * Card)
    | WinningHand

let cardSetup = dealInitialHand Deck

let init (payload: GameType) = {
    PlayerOne = playerOneFixed
    PlayerTwo = playerTwoFixed
    PlayerThree = playerThreeFixed
    SelectedCards = None
    Game = payload
    CardValue = 0
}

let assignHandToPlayers model (hands: GameSetup) =
    let firstP = { model with PlayerOne.StartingHand = hands.FirstPlayer }
    let secondP = { firstP with PlayerTwo.StartingHand = hands.SecondPlayer }
    { secondP with PlayerThree.StartingHand = hands.ThirdPlayer }

let removeCardFromPlayer model (id: int) (card: Card) =
    let newHand = 
        match id with
        | 1 when model.PlayerOne.Player = 1
            -> List.filter ((<>) card) model.PlayerOne.StartingHand
        | 2 when model.PlayerTwo.Player = 2
            -> List.filter ((<>) card) model.PlayerTwo.StartingHand
        | 3 when model.PlayerThree.Player = 3
            -> List.filter ((<>) card) model.PlayerThree.StartingHand
    match id with
    | 1 when model.PlayerOne.Player = 1
        -> { model with PlayerOne.StartingHand = newHand}
    | 2 when model.PlayerTwo.Player = 2
        -> { model with PlayerTwo.StartingHand = newHand}
    | 3 when model.PlayerThree.Player = 3
        -> { model with PlayerThree.StartingHand = newHand}

let winningHand (game: GameType) (hands: (PlayerId * Card) list) =
    hands
    |> List.map (fun (a,b) -> (a,b, (cardStrength game b)))
    |> List.maxBy (fun (_, _, v) -> v)

let calculateAugen (player: PlayerConfig) =
    match player.HandsWon with
    | Some v ->
        v
        |> List.map snd
        |> List.map (fun h -> handValueGrand h)
        |> List.sum
    | None -> 0

let update msg model =
    match msg with
    | SetCards -> (assignHandToPlayers model cardSetup), Cmd.none, DoNothing
    | ReturnFirstPage -> model, Cmd.none, DoNothing
    | ToggleCard (Some [(id, c)]) ->
        let newList =
            match model.SelectedCards with
            | None -> Some [(id, c)]
            | Some _ -> Some (model.SelectedCards.Value @ [(id, c)])
        let newModel = { model with SelectedCards = newList }
        newModel, Cmd.batch [
            Cmd.ofMsg CompareCards
            Cmd.ofMsg (RemoveCard (id, c))
        ], DoNothing
    | CompareCards ->
        let selectedCards = model.SelectedCards
        let one = 
            match selectedCards.IsSome with
            | true -> 
                let (id, c) = selectedCards.Value.Head
                cardStrength model.Game c
            | false -> failwith "No cards selected."
        let newModel = { model with CardValue = one }
        newModel, Cmd.none, DoNothing
    | RemoveCard (id, c) ->
        let newModel = removeCardFromPlayer model id c
        newModel, Cmd.none, DoNothing
    | WinningHand -> 
        let winner = 
            match model.SelectedCards with
            | Some v -> winningHand model.Game v
            | None -> failwith "No cards selected yet."
        let modelHandsWon =
            match winner with
            | (1, _, _) -> 
                let newList = 
                    match model.PlayerOne.HandsWon with
                    | Some v -> v @ model.SelectedCards.Value
                    | None -> model.SelectedCards.Value
                { model with PlayerOne.HandsWon = Some newList }
            | (2, _, _) -> 
                let newList = 
                    match model.PlayerTwo.HandsWon with
                    | Some v -> v @ model.SelectedCards.Value
                    | None -> model.SelectedCards.Value
                { model with PlayerTwo.HandsWon = Some newList }
            | (3, _, _) -> 
                let newList = 
                    match model.PlayerThree.HandsWon with
                    | Some v -> v @ model.SelectedCards.Value
                    | None -> model.SelectedCards.Value
                { model with PlayerThree.HandsWon = Some newList }
        let newModel = { modelHandsWon with SelectedCards = None  }
        newModel, Cmd.none, DoNothing

let view model =
    View.ContentPage(
        View.VStack (spacing = 25.) {
            View.Label($"{model.Game}")
            View.Label($"{model.CardValue}")
            View.Label($"{model.SelectedCards}")
            View.Label($"{model.PlayerOne.HandsWon}")
            View.Button("Set Cards", SetCards)
            View.Button("Evaluate", WinningHand)
            View.HStack (spacing = 25.) {
                View.Label($"{(calculateAugen model.PlayerOne)}")
                View.Label($"{(calculateAugen model.PlayerTwo)}")
                View.Label($"{(calculateAugen model.PlayerThree)}")
            }
            View.HStack (spacing = 25.) {
                View.ListView(model.PlayerOne.StartingHand)
                    (fun card -> 
                        View.ViewCell(
                            View.Image((cardToImageName card))
                                .height(64.)
                                .gestureRecognizer(
                                    View.TapGestureRecognizer(ToggleCard (Some [(model.PlayerOne.Player, card)]))
                                )
                        ))
                View.ListView(model.PlayerTwo.StartingHand)
                    (fun card -> 
                        View.ViewCell(
                            View.Image((cardToImageName card))
                                .height(64.)
                                .gestureRecognizer(
                                    View.TapGestureRecognizer(ToggleCard (Some [(model.PlayerTwo.Player, card)]))
                                )
                        ))
                View.ListView(model.PlayerThree.StartingHand)
                    (fun card -> 
                        View.ViewCell(
                            View.Image((cardToImageName card))
                                .height(64.)
                                .gestureRecognizer(
                                    View.TapGestureRecognizer(ToggleCard (Some [(model.PlayerThree.Player, card)]))
                                )
                        ))
                View.ListView(cardSetup.Skat)
                    (fun card -> 
                        View.ViewCell(
                            View.Image((cardToImageName card))
                                .height(64.)
                        ))
            }
        }
    )