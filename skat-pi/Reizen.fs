module Reizen

open GameFoundation

let updatePlayerActivity (player : ReizAction) decision =
    { player with Activity = decision }

let updatePlayerOneActivity decision =
    match playerOne with
    | p -> playerOne <- {playerOne with Activity = decision}

let updatePlayerTwoActivity decision =
    match playerTwo with
    | p -> playerTwo <- {playerTwo with Activity = decision}

let updatePlayerThreeActivity decision =
    match playerThree with
    | p -> playerThree <- {playerThree with Activity = decision}

//let updatePlayerAmountM player amount =
//    match player.Activity with
//    | Bid -> { player with Amount = Some amount }
//    | Reject -> player
//    | Undecided -> player

let updatePlayerAmount player amount =
    match player.Activity with
    | Bid -> { player with Amount = Some amount }
    | Reject -> player
    | Undecided -> player

let rec setGameStyle (player: ReizAction) : GameStyle =
    printf "Player %i, which game style do you want to play (Colour/Grand/Null)?" player.Player
    let console = System.Console.ReadLine()
    match console with
    | "Colour" -> ColourGame
    | "Grand" -> Grand
    | "Null" -> NullGame
    | _ -> 
        printf "Player %i, wrong selection, try again." player.Player
        setGameStyle player

let rec getPlayerAction (player: PlayerId) =
    printf "Player %i, do you want to bid (Yes/No):" player
    let console = System.Console.ReadLine()
    match player with
    | 1 -> match console with
            | "Yes" -> updatePlayerOneActivity Bid
            | "No" -> updatePlayerOneActivity Reject
            | _ -> getPlayerAction player
    | 2 -> match console with
            | "Yes" -> updatePlayerTwoActivity Bid
            | "No" -> updatePlayerTwoActivity Reject
            | _ -> getPlayerAction player
    | 3 -> match console with
            | "Yes" -> updatePlayerThreeActivity Bid
            | "No" -> updatePlayerThreeActivity Reject
            | _ -> getPlayerAction player
    | _ -> failwith "Wrong player ID."

let rec biddingM (player: PlayerId) (bid: int) =
    printf "Player %i bid:" player
    match System.Int32.TryParse(System.Console.ReadLine()) with
    | false, _ -> failwith "Input could not be parsed as integer."
    | true, input when input > bid -> 
        match player with
        | 1 -> playerOne <- {playerOne with Amount = Some input}
        | 2 -> playerTwo <- {playerTwo with Amount = Some input}
        | 3 -> playerThree <- {playerThree with Amount = Some input}
        | _ -> failwith "Wrong player ID."
    | true, _ -> biddingM player bid

let rec bidding (player: ReizAction) (bid: int) =
    printf "Player %i bid:" player.Player
    match System.Int32.TryParse(System.Console.ReadLine()) with
    | false, _ -> None
    | true, input when input > bid -> Some input
    | true, _ -> bidding player bid

let rec getBiddingPlayer (playerOne: ReizAction) (playerTwo: ReizAction) (startBid: int) =
    match bidding playerOne startBid with
    | None -> playerTwo
    | Some v -> 
        let firstPlayerUpdated = { playerOne with Amount = Some v }
        match bidding playerTwo v with
        | None -> firstPlayerUpdated
        | Some i -> 
            let secondPlayerUpdated = { playerTwo with Amount = Some i }
            getBiddingPlayer firstPlayerUpdated secondPlayerUpdated i

let startBidding () =
    getPlayerAction 1
    getPlayerAction 2
    if firstPlayer.Activity = Bid && secondPlayer.Activity = Bid then
        getBiddingPlayer firstPlayer secondPlayer 18
    else
        getPlayerAction 3
        if firstPlayer.Activity = Bid && thirdPlayer.Activity = Bid then
            getBiddingPlayer firstPlayer thirdPlayer 18
        elif secondPlayer.Activity = Bid && thirdPlayer.Activity = Bid then
            getBiddingPlayer secondPlayer thirdPlayer 18
        else
            failwith "There is no bidder."

let startReizAction () =
    let result = startBidding()
    setGameStyle result