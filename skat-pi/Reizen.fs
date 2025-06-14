module Reizen

open GameFoundation

let updatePlayerActivity player decision =
    { player with Activity = decision }

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

let rec getPlayerAction (player: ReizAction) =
    printf "Player %i, do you want to bid (Yes/No):" player.Player
    let console = System.Console.ReadLine()
    match console with
    | "Yes" -> updatePlayerActivity player Bid
    | "No" -> updatePlayerActivity player Reject
    | _ -> getPlayerAction player

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
    let firstPlayer = getPlayerAction firstPlayer
    let secondPlayer = getPlayerAction secondPlayer
    if firstPlayer.Activity = Bid && secondPlayer.Activity = Bid then
        getBiddingPlayer firstPlayer secondPlayer 18
    else
        let thirdPlayer = getPlayerAction thirdPlayer
        if firstPlayer.Activity = Bid && thirdPlayer.Activity = Bid then
            getBiddingPlayer firstPlayer thirdPlayer 18
        elif secondPlayer.Activity = Bid && thirdPlayer.Activity = Bid then
            getBiddingPlayer secondPlayer thirdPlayer 18
        else
            failwith "There is no bidder."

let startReizAction () =
    let result = startBidding()
    setGameStyle result