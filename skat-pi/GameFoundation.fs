﻿module GameFoundation

type Suite = Spades | Clubs | Hearts | Diamonds
type Rank = Seven | Eight | Nine | Dame | King | Ten | Ace | Bube
type Card = { Rank: Rank ; Suite: Suite }
type PlayerId = int
type GameState = {
    TurnQueue: PlayerId list
    TurnCount: int
}
type GameSetup =
    {
        FirstPlayer: Card list
        SecondPlayer: Card list
        ThirdPlayer: Card list
        Skat: Card list
    }
type Action =
    | Bid
    | Reject
    | Accept

let allRanks = [Seven ; Eight ; Nine ; Dame ; King ; Ten ; Ace ; Bube]
let allSuites = [Spades ; Clubs ; Hearts ; Diamonds]
let gamestate = true
let initialState = { TurnQueue = [1; 2; 3]; TurnCount = 0 }
let Deck =
    [ for suite in allSuites do
        for rank in allRanks do
            yield { Rank = rank ; Suite = suite } ]

let shuffleDeck deck =
    let rnd = System.Random()
    deck |> List.sortBy (fun _ -> rnd.Next())

let pickCard deck =
    deck |> List.head

let dealInitialHand deck =
    let shuffled = shuffleDeck deck
    let (firstPlayerHand, rest) = List.splitAt 10 shuffled
    let (secondPlayerHand, newDeck) = List.splitAt 10 rest
    let (thirdPlayerHand, skat) = List.splitAt 10 newDeck
    { FirstPlayer = firstPlayerHand; SecondPlayer = secondPlayerHand; ThirdPlayer = thirdPlayerHand; Skat = skat }

let nextTurn (state: GameState) : PlayerId * GameState =
    match state.TurnQueue with
    | [] -> failwith "Empty Queue"
    | current :: rest -> 
        let newQueue = rest @ [current]
        current, { state with TurnQueue = newQueue; TurnCount = state.TurnCount + 1 }

let getActionFromConsole (playerId: PlayerId) : Action =
    printfn "Player %d, enter your action (Bid, Reject, Accept):" playerId
    match System.Console.ReadLine().Trim().ToLower().Split() with
    | [| "bid" |] -> Bid
    | [| "reject" |] -> Reject
    | [| "accept" |] -> Accept
    | _ ->
        printfn "Invalid input, defaulting to wait."
        Reject

let takeAction (player: PlayerId) (action: Action) (game: GameState) : PlayerId * GameState =
    match action with
    | Bid -> 
        printf "Player %i bids." player
        nextTurn game
    | Reject ->
        printf "Player %i rejects." player
        nextTurn game
    | Accept -> 
        printf "Player %i accepts." player
        nextTurn game

let rec gameloop (player: PlayerId) (game: GameState) =
    match game.TurnQueue with
    | [] ->
        printfn "All players have quit. Game over."
    | currentPlayer :: rest ->
        printfn "Turn %d: Player %d's turn" game.TurnCount currentPlayer
        let action = getActionFromConsole currentPlayer
        let playerid, gamestate = takeAction currentPlayer action game
        gameloop playerid gamestate