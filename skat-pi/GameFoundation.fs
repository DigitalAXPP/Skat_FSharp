module GameFoundation

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
type reizen = {
    FirstPlayer: PlayerId
    SecondPlayer: PlayerId
    ThirdPlayer: PlayerId
}
type reizAction = {
    Player: PlayerId
    Activity: Action
    Amount: int option
}
type  reizList = {
    FirstPlayer: reizAction
    SecondPlayer: reizAction
    ThirdPlayer: reizAction
}

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
    printfn "Player %d, enter your action (Bid, Reject):" playerId
    match System.Console.ReadLine().Trim().ToLower().Split() with
    | [| "bid" |] -> Bid
    | [| "reject" |] -> Reject
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

//let resolveBids (bid1: reizAction) (bid2: reizAction) (bid3: reizAction) : reizen =
//    if bid1.Activity = Bid > bid2.Amount then
//        Winner (bid1.Player, bid1.Amount)
//    elif bid2.Amount > bid1.Amount then
//        Winner (bid2.Player, bid2.Amount)
//    else
//        Tie bid1.Amount

let getBidFromConsole playerId =
    printf "Player %d, enter your bid: " playerId
    let input = System.Console.ReadLine()
    match System.Int32.TryParse input with
    | true, amount -> { Player = playerId; Amount = amount }
    | _ -> 
        printfn "Invalid input. Defaulting to 0."
        { Player = playerId; Amount = 0 }

let rec biddingLoop rounds (score: Map<PlayerId, int>) =
    if rounds = 0 then
        printfn "Final scores:"
        score |> Map.iter (fun pid s -> printfn "Player %d: %d" pid s)
    else
        let b1 = getBidFromConsole 1
        let b2 = getBidFromConsole 2
        let result = resolveBids b1 b2
        let updatedScore =
            match result with
            | Winner (pid, _) -> score |> Map.change pid (fun opt -> Some ((Option.defaultValue 0 opt) + 1))
            | Tie _ -> score
        biddingLoop (rounds - 1) updatedScore

let rec gameloop (player: PlayerId) (game: GameState) =
    match game.TurnQueue with
    | [] ->
        printfn "All players have quit. Game over."
    | currentPlayer :: rest ->
        printfn "Turn %d: Player %d's turn" game.TurnCount currentPlayer
        let action = getActionFromConsole currentPlayer
        let playerid, gamestate = takeAction currentPlayer action game
        gameloop playerid gamestate