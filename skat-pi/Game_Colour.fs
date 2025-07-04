module GamePlay

open GameFoundation

//type Suite = 
//    | Diamonds
//    | Hearts
//    | Clubs
//    | Spades
//type Rank =
//    | Seven
//    | Eight
//    | Nine
//    | Dame
//    | King
//    | Ten
//    | Ace
//    | Jack
//type Card = { Suite: Suite; Rank: Rank }
type GameSetup =
    {
        FirstPlayer: Card list
        SecondPlayer: Card list
        ThirdPlayer: Card list
        Skat: Card list
    }
type GameState = 
    | Uninitialized
    | Running
    | Finished
type GameType =
    | SuitGame of Suite  // One suit is trump
    | Grand              // Only Jacks are trump
    | Null               // No trumps at all
type PlayerHand = { PlayerID: PlayerId; HandScore: int; Hand: Card}

let allRanks = [Seven ; Eight ; Nine ; Dame ; King ; Ten ; Ace ; Jack]
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

let getTrumpSuites (gameType: GameType) : Suite list =
    match gameType with
    | SuitGame trumpSuite ->
        // In Suit game, all Jacks + the selected trump suite are trumps
        [Diamonds; Hearts; Clubs; Spades] // All Jacks are implied to be trump
        |> List.filter (fun s -> s = trumpSuite)  // Return all suites for now
    | Grand ->
        // Only Jacks are trumps; return their "suites"
        []  // Since Jacks are not Suite cards, you might model them differently
    | Null ->
        // No trumps at all
        []

let isTrumpSuite (card: Card) (trump: Suite) =
    card.Suite = trump

let normalRankOrder = [Seven; Eight; Nine; Dame; King; Ten; Ace]

let removeCardFirst (cards: GameSetup) (card: Card) =
    let newhand = List.filter ((<>) card) cards.FirstPlayer
    {cards with FirstPlayer = newhand }

let removeCardSecond (cards: GameSetup) (card: Card) =
    let newhand = List.filter ((<>) card) cards.SecondPlayer
    {cards with SecondPlayer = newhand }

let removeCardThird (cards: GameSetup) (card: Card) =
    let newhand = List.filter ((<>) card) cards.ThirdPlayer
    {cards with ThirdPlayer = newhand }

let addHandtoPlayerOne hand =
    playerOne <- { playerOne with Hands = playerOne.Hands @ hand }

let addHandtoPlayerTwo hand =
    playerTwo <- { playerTwo with Hands = playerTwo.Hands @ hand }

let addHandtoPlayerThree hand =
    playerThree <- { playerThree with Hands = playerThree.Hands @ hand }

let cardStrength (game: GameType) (card: Card) : int =
    let jackSuitOrder = [Clubs; Spades; Hearts; Diamonds]

    match game with
    | Grand | SuitGame _ when card.Rank = Jack ->
        100 + List.findIndex ((=) card.Suite) jackSuitOrder
    | SuitGame trump when card.Suite = trump ->
        50 + List.findIndex ((=) card.Rank) normalRankOrder
    | _ ->
        List.findIndex ((=) card.Rank) normalRankOrder

let winHand (hands: PlayerHand list) =
    let handList = [hands.[0].Hand; hands.[1].Hand; hands.[2].Hand]
    let winningHand = hands |> List.maxBy (fun p -> p.HandScore)
    match winningHand.PlayerID with
    | 1 -> addHandtoPlayerOne handList
    | 2 -> addHandtoPlayerTwo handList
    | 3 -> addHandtoPlayerThree handList

let handValueGrand (hand: Card) =
    match hand.Rank with
    | Seven -> 0
    | Eight -> 0
    | Nine -> 0
    | Dame -> 3
    | King -> 4
    | Ten -> 10
    | Ace -> 11
    | Jack -> 2

let calculateAugen (player: PlayerState) =
    player.Hands
    |> List.map (fun h -> handValueGrand h)
    |> List.sum

let rec playRound cards game =
    printf "Player 1, select the card you want to play:"
    let consoleOne = System.Console.ReadLine()
    printf "Player 2, select the card you want to play:"
    let consoleTwo = System.Console.ReadLine()
    printf "Player 3, select the card you want to play:"
    let consoleThree = System.Console.ReadLine()
    let one = match System.Int32.TryParse(consoleOne) with
                | false, _ -> failwith "no integer"
                | true, input -> cards.FirstPlayer[input]
    let two = match System.Int32.TryParse(consoleTwo) with
                | false, _ -> failwith "no integer"
                | true, input -> cards.SecondPlayer[input]
    let three = match System.Int32.TryParse(consoleThree) with
                | false, _ -> failwith "no integer"
                | true, input -> cards.ThirdPlayer[input]
    let first = cardStrength game one
    let cards_first = removeCardFirst cards one
    let second = cardStrength game two
    let cards_second = removeCardSecond cards_first two
    let third = cardStrength game three
    let cards_third = removeCardThird cards_second three
    let handList = [
        {PlayerID = 1; HandScore = first; Hand = one}
        {PlayerID = 2; HandScore = second; Hand = two}
        {PlayerID = 3; HandScore = third; Hand = three}]
    winHand handList
    //let highest = handList |> List.maxBy (fun p -> p.HandScore)
    //let final = List.max [first; second; third]
    //printf "%i" final
    printf "Player 1 has %i Augen." (calculateAugen playerOne)
    printf "Player 2 has %i Augen." (calculateAugen playerTwo)
    printf "Player 3 has %i Augen." (calculateAugen playerThree)
    printf "%A" cards_third
    match cards_third.FirstPlayer with
    | [] -> Finished
    | _ -> playRound cards_third game