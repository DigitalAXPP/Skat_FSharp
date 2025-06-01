module GamePlay

open GameFoundation

type Suite = 
    | Diamonds
    | Hearts
    | Clubs
    | Spades
type Rank =
    | Seven
    | Eight
    | Nine
    | Dame
    | King
    | Ten
    | Ace
    | Bube
type Card = { Suite: Suite; Rank: Rank }
type GameSetup =
    {
        FirstPlayer: Card list
        SecondPlayer: Card list
        ThirdPlayer: Card list
        Skat: Card list
    }

let allRanks = [Seven ; Eight ; Nine ; Dame ; King ; Ten ; Ace ; Bube]
let allSuites = [Spades ; Clubs ; Hearts ; Diamonds]

let Deck =
    [ for suite in allSuites do
        for rank in allRanks do
            yield { Rank = rank ; Suite = suite } ]

let shuffleDeck (deck: Card list) =
    let rnd = System.Random()
    deck |> List.sortBy (fun _ -> rnd.Next())

let dealInitialHand deck =
    let shuffled = shuffleDeck deck
    let (firstPlayerHand, rest) = List.splitAt 10 shuffled
    let (secondPlayerHand, newDeck) = List.splitAt 10 rest
    let (thirdPlayerHand, skat) = List.splitAt 10 newDeck
    { FirstPlayer = firstPlayerHand; SecondPlayer = secondPlayerHand; ThirdPlayer = thirdPlayerHand; Skat = skat }

let compareCards (firstPlayerCard: Card) (secondPlayerCard: Card) (thirdPlayerCard: Card) : Card =
    List.max [firstPlayerCard; secondPlayerCard; thirdPlayerCard]