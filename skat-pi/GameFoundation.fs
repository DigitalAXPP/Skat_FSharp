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

let cardToImageName =
    function
    | { Suite = Hearts; Rank = Ace } -> "hearts_ace.png"
    | { Suite = Hearts; Rank = King } -> "hearts_king.png"
    | { Suite = Hearts; Rank = Seven }  -> "hearts_seven.png"
    | { Suite = Hearts; Rank = Eight }  -> "hearts_eight.png"
    | { Suite = Hearts; Rank = Nine }  -> "hearts_nine.png"
    | { Suite = Hearts; Rank = Ten }  -> "hearts_ten.png"
    | { Suite = Hearts; Rank = Dame }  -> "hearts_dame.png"
    | { Suite = Hearts; Rank = Jack }  -> "hearts_jack.png"
    | { Suite = Clubs; Rank = Ace } -> "hearts_ace.png"
    | { Suite = Clubs; Rank = King } -> "clubs_king.png"
    | { Suite = Clubs; Rank = Seven }  -> "clubs_seven.png"
    | { Suite = Clubs; Rank = Eight }  -> "clubs_eight.png"
    | { Suite = Clubs; Rank = Nine }  -> "clubs_nine.png"
    | { Suite = Clubs; Rank = Ten }  -> "clubs_ten.png"
    | { Suite = Clubs; Rank = Dame }  -> "clubs_dame.png"
    | { Suite = Clubs; Rank = Jack }  -> "clubs_jack.png"
    | { Suite = Spades; Rank = Ace } -> "hearts_ace.png"
    | { Suite = Spades; Rank = King } -> "spades_king.png"
    | { Suite = Spades; Rank = Seven }  -> "hearts_seven.png"
    | { Suite = Spades; Rank = Eight }  -> "hearts_eight.png"
    | { Suite = Spades; Rank = Nine }  -> "hearts_nine.png"
    | { Suite = Spades; Rank = Ten }  -> "hearts_ten.png"
    | { Suite = Spades; Rank = Dame }  -> "spades_dame.png"
    | { Suite = Spades; Rank = Jack }  -> "spades_jack.png"
    | { Suite = Diamonds; Rank = Ace } -> "hearts_ace.png"
    | { Suite = Diamonds; Rank = King } -> "diamonds_king.png"
    | { Suite = Diamonds; Rank = Seven }  -> "hearts_seven.png"
    | { Suite = Diamonds; Rank = Eight }  -> "hearts_eight.png"
    | { Suite = Diamonds; Rank = Nine }  -> "hearts_nine.png"
    | { Suite = Diamonds; Rank = Ten }  -> "hearts_ten.png"
    | { Suite = Diamonds; Rank = Dame }  -> "hearts_dame.png"
    | { Suite = Diamonds; Rank = Jack }  -> "diamonds_jack.png"

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
type GameType =
    | SuitGame of Suite  // One suit is trump
    | Grand              // Only Jacks are trump
    | NullGame               // No trumps at all
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