module GameFoundation

type Suite = Spades | Clubs | Hearts | Diamonds
type Rank = Seven | Eight | Nine | Dame | King | Ten | Ace | Bube
type Card = { Rank: Rank ; Suite: Suite }
type PlayerId = int
type GameState = { TurnQueue: PlayerId list}
type GameSetup =
    {
        FirstPlayer: Card list
        SecondPlayer: Card list
        ThirdPlayer: Card list
        Skat: Card list
    }

let allRanks = [Seven ; Eight ; Nine ; Dame ; King ; Ten ; Ace ; Bube]
let allSuites = [Spades ; Clubs ; Hearts ; Diamonds]
let initialState = { TurnQueue = [1; 2; 3] }
let Deck =
    [ for suite in allSuites do
        for rank in allRanks do
            yield { Rank = rank ; Suite = suite } ]

let shuffleDeck deck =
    let rnd = System.Random()
    deck |> List.sortBy (fun _ -> rnd.Next())

let pickCard deck =
    deck |> List.head

let nextTurn (state: GameState) : PlayerId * GameState =
    match state.TurnQueue with
    | [] -> failwith "Empty Queue"
    | current :: rest -> 
        let newQueue = rest @ [current]
        current, { state with TurnQueue = newQueue }

let dealInitialHand deck =
    let shuffled = shuffleDeck deck
    let (firstPlayerHand, rest) = List.splitAt 10 shuffled
    let (secondPlayerHand, newDeck) = List.splitAt 10 rest
    let (thirdPlayerHand, skat) = List.splitAt 10 newDeck
    { FirstPlayer = firstPlayerHand; SecondPlayer = secondPlayerHand; ThirdPlayer = thirdPlayerHand; Skat = skat }