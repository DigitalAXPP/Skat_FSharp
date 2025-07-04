module GameFoundation

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
    | Jack
type Card = { Suite: Suite; Rank: Rank }

type PlayerId = int
type SkatPosition =
    | Geben
    | Hoeren
    | Sagen
type GameState = {
    TurnQueue: PlayerId list
    TurnCount: int
}
type GameStyle =
    | ColourGame
    | Grand
    | NullGame
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
type PlayerState = {
    Player: PlayerId
    Activity: Action
    Amount: int option
    Position: SkatPosition
    Hands: Card list
}
type Reizen = {
    FirstPlayer: ReizAction
    SecondPlayer: ReizAction
    ThirdPlayer: ReizAction
}
let mutable playerOne = {
    Player = 1
    Activity = Undecided
    Amount = None
    Position = Geben
    Hands = []
}
let mutable playerTwo = {
    Player = 2
    Activity = Undecided
    Amount = None
    Position = Geben
    Hands = []
}
let mutable playerThree = {
    Player = 3
    Activity = Undecided
    Amount = None
    Position = Geben
    Hands = []
}