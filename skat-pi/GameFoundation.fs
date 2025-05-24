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
    | Undecided
type ReizAction = {
    Player: PlayerId
    Activity: Action
    Amount: int option
}
type Reizen = {
    FirstPlayer: ReizAction
    SecondPlayer: ReizAction
    ThirdPlayer: ReizAction
}
//let reizList = {
//    FirstPlayer = { Player = 1; Activity = Undecided; Amount = None}
//    SecondPlayer = { Player = 2; Activity = Undecided; Amount = None}
//    ThirdPlayer = { Player = 3; Activity = Undecided; Amount = None}
//}
let firstPlayer = {
    Player = 1
    Activity = Undecided
    Amount = None
}
let secondPlayer = {
    Player = 2
    Activity = Undecided
    Amount = None
}
let thirdPlayer = {
    Player = 3
    Activity = Undecided
    Amount = None
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

let updatePlayerActivity player decision =
    { player with Activity = decision }

let updatePlayerAmount player amount =
    match player.Activity with
    | Bid -> { player with Amount = Some amount }
    | Reject -> player
    | Undecided -> player
    //| first :: rest ->
    //    let updatedFirst = { first with Activity = decision }
    //    updatedFirst :: rest
    //| _ -> reizList
//let resolveBids (bid1: reizAction) (bid2: reizAction) (bid3: reizAction) : reizen =
//    if bid1.Activity = Bid > bid2.Amount then
//        Winner (bid1.Player, bid1.Amount)
//    elif bid2.Amount > bid1.Amount then
//        Winner (bid2.Player, bid2.Amount)
//    else
//        Tie bid1.Amount

let getPlayerAction (player: ReizAction) (action: bool) =
    match action with
    | true -> updatePlayerActivity player Bid
    | false -> updatePlayerActivity player Reject

let rec bidding (player: int) (bid: int) =
    printf "Player %i bid:" player
    match System.Int32.TryParse(System.Console.ReadLine()) with
    | false, _ -> None
    | true, input when input > bid -> Some input
    | true, _ -> bidding player bid

let rec getBiddingPlayer (playerOne: int) (playerTwo: int) (startBid: int) =
    match bidding playerOne startBid with
    | None -> $"Player {playerTwo} wins"
    | Some v -> 
        match bidding playerTwo v with
        | None -> 
            $"Player {playerOne} wins"
        | Some i -> getBiddingPlayer playerOne playerTwo i

let startBidding () =
    printf "Player 1, do you want to bid:"
    let erster = System.Console.ReadLine()
    printf "Player 2, do you want to bid:"
    let zwei = System.Console.ReadLine()
    if erster = "Y" && zwei = "Y" then
        getBiddingPlayer 1 2 18
    else
        printf "Player 3, do you want to bid:"
        let drei = System.Console.ReadLine()
        if erster = "Y" && drei = "Y" then
            getBiddingPlayer 1 3 18
        elif zwei = "Y" && drei = "Y" then
            getBiddingPlayer 2 3 18
        else
            "No player wants to bid"
        

let resolveReizen (playerOne: ReizAction) (playerTwo: ReizAction) playerThree =
    if playerOne.Activity = Undecided then
        printf "Player %d, do you want to bid?" playerOne.Player
        let fst, input = bool.TryParse(System.Console.ReadLine())
        let first = getPlayerAction playerOne input

    else if playerTwo.Activity = Undecided then
        printf "Player %d, do you want to bid?" playerTwo.Player
        let fst, input = bool.TryParse(System.Console.ReadLine())
        let second = getPlayerAction playerTwo input

let resolveBids (reizen: Reizen) : Reizen =
    if reizen.FirstPlayer.Activity = Undecided then
        printf "Player %d, do you want to bid?" reizen.FirstPlayer.Player
        let input = System.Console.ReadLine()
        match input with
        | "Yes" -> updatedFirstPlayerActivity Bid
        | "No" -> updatedFirstPlayerActivity Reject
    else
        reizen
    //else if reizen.[0].Activity = Bid then
    //    printf "Player %d, enter your bid: " reizen.[0].Player
    //    let input = System.Console.ReadLine()
    //    match System.Int32.TryParse input with
    //    | true, amount -> { Player = playerId; Amount = Some amount; Activity = Bid}
    //    | _ -> 
    //        printfn "Invalid input. Defaulting to 0."
    //        { Player = playerId; Amount = None; Activity = Bid }
        
            

let getBidFromConsole playerId =
    printf "Player %d, enter your bid: " playerId
    let input = System.Console.ReadLine()
    match System.Int32.TryParse input with
    | true, amount -> { Player = playerId; Amount = Some amount; Activity = Bid}
    | _ -> 
        printfn "Invalid input. Defaulting to 0."
        { Player = playerId; Amount = None; Activity = Bid }

//let rec biddingLoop rounds (score: Map<PlayerId, int>) =
//    let b1 = getBidFromConsole 1
//    let b2 = getBidFromConsole 2
//    let result = resolveBids b1 b2
//    let updatedScore =
//        match result with
//        | Winner (pid, _) -> score |> Map.change pid (fun opt -> Some ((Option.defaultValue 0 opt) + 1))
//        | Tie _ -> score
//    biddingLoop (rounds - 1) updatedScore

let rec biddingLoop (reizen: Reizen) =
    let player1 = getBidFromConsole 1
    let player2 = getBidFromConsole 2
    let player3 = getBidFromConsole 3
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