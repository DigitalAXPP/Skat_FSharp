module ThirdPage

open Microsoft.Maui
open Microsoft.Maui.Controls
open Fabulous
open Fabulous.Maui
open type Fabulous.Maui.View
open GamePlay
open GameFoundation
open System

type Player = North | East | South

type ReizState = {
    CurrentPlayer: Player
    HighestBid: int option
    Passed: Set<Player>
    Finished: bool
}

type Intent =
    | DoNothing
    | BackFirstPage
    | ForwardFourthPage of GameType

type Model = {
    PlayerOne: PlayerConfig
    PlayerTwo: PlayerConfig
    PlayerThree: PlayerConfig
    CurrentPlayer: int
    HighestBid: int option
    Game: GameType
    Passed: Set<PlayerConfig>
    Finished: bool
    UserInput: string
}

//type Model = { 
//    Name: string 
//    Bid: string
//    CurrentPlayer: PlayerState
//    HighestBid: int option
//    Passed: Set<PlayerState>
//    Finished: bool}

type Msg =
        | ReturnFirstPage
        | UpdateBid of string
        | ExitBidding of int
        | UpdateUserAmount of int
        | UpdateUserActivity of int
        | PassBid
        | MakeBid
        | MoveNextPlayer
        | AcceptBid of int
        | TextUpdated of string
        | MoveFourthPage
        | SetGame of int

//let makeGridCollectionView (items: string list) =
    //let cv : CollectionView = CollectionView()
    //let layout : ItemsLayout = GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
    //cv.ItemsLayout <- layout
    //cv.ItemTemplate <- DataTemplate(fun () ->
    //    let img : Microsoft.Maui.Controls.Image = Microsoft.Maui.Controls.Image(Aspect = Aspect.AspectFit)
    //    img.SetBinding(Microsoft.Maui.Controls.Image.SourceProperty, ".")
    //    img :> obj
    //)
    //cv.ItemsSource <- items
    //cv
    //let cv : CollectionView = CollectionView()
    //let layout : ItemsLayout = GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
    //cv.ItemsLayout <- layout
    //cv.ItemsSource <- items
    //cv.ItemTemplate <- DataTemplate()
    //cv

let cardSetup = dealInitialHand Deck

let init () = { 
    PlayerOne = playerOneFixed
    PlayerTwo = playerTwoFixed
    PlayerThree = playerThreeFixed
    CurrentPlayer = 1
    HighestBid = None
    Game = SuitGame (Hearts)
    Passed = Set.empty
    Finished = false
    UserInput = String.Empty
 }

let getPlayer model id =
    match id with
    | 1 -> model.PlayerOne
    | 2 -> model.PlayerTwo
    | 3 -> model.PlayerThree
    | _ -> failwith "Invalid player ID"

let setPlayer model id player =
    match id with
    | 1 -> { model with PlayerOne = player }
    | 2 -> { model with PlayerTwo = player }
    | 3 -> { model with PlayerThree = player }
    | _ -> failwith "Invalid player ID"

let gameToIndex game = 
    match game with
    | SuitGame Hearts -> 0
    | SuitGame Diamonds -> 1
    | SuitGame Spades -> 2
    | SuitGame Clubs -> 3
    | Grand -> 4
    | NullGame -> 5

let nextPlayer model id =
    match id with
    | 1 when (model.PlayerTwo.Activity = Bid || model.PlayerTwo.Activity = Undecided) && (model.PlayerThree.Activity = Reject || model.PlayerThree.Activity = Undecided)
        -> { model with CurrentPlayer = 2 }
    | 1 when model.PlayerThree.Activity = Bid || model.PlayerThree.Activity = Undecided && (model.PlayerTwo.Activity = Reject || model.PlayerTwo.Activity = Undecided)
        -> { model with CurrentPlayer = 3 }
    | 2 when model.PlayerThree.Activity = Bid || model.PlayerThree.Activity = Undecided && (model.PlayerOne.Activity = Reject || model.PlayerOne.Activity = Undecided)
        -> { model with CurrentPlayer = 3 }
    | 2 when model.PlayerOne.Activity = Bid || model.PlayerOne.Activity = Undecided && (model.PlayerThree.Activity = Reject || model.PlayerThree.Activity = Undecided)
        -> { model with CurrentPlayer = 1 }
    | 3 when model.PlayerOne.Activity = Bid || model.PlayerOne.Activity = Undecided && (model.PlayerTwo.Activity = Reject || model.PlayerTwo.Activity = Undecided)
        -> { model with CurrentPlayer = 1 }
    | 3 when model.PlayerTwo.Activity = Bid || model.PlayerTwo.Activity = Undecided && (model.PlayerOne.Activity = Reject || model.PlayerOne.Activity = Undecided)
        -> { model with CurrentPlayer = 2 }
    | _ -> failwith "Invalid player ID"

let update msg model =
        match msg with
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage
        | MoveFourthPage -> model, Cmd.none, ForwardFourthPage model.Game
        | TextUpdated input -> { model with UserInput = input }, Cmd.none, DoNothing
        | SetGame selection ->
            let newmodel = 
                match selection with
                | 0 -> { model with Game = SuitGame (Hearts) }
                | 1 -> { model with Game = SuitGame (Diamonds) }
                | 2 -> { model with Game = SuitGame (Spades) }
                | 3 -> { model with Game = SuitGame (Clubs) }
                | 4 -> { model with Game = Grand }
                | 5 -> { model with Game = NullGame }
            newmodel, Cmd.none, DoNothing
        //| UpdateUserAmount u ->
        //    match u with
        //    | 1 -> (playerOne <- {playerOne with Amount = model.HighestBid})
        //    | 2 -> (playerTwo <- {playerTwo with Amount = model.HighestBid})
        //    | 3 -> (playerThree <- {playerThree with Amount = model.HighestBid})
        //    | _ -> failwith "Wrong player ID."
        //    model, Cmd.none, DoNothing
        //| UpdateUserActivity u ->
        //    match u with
        //    | 1 -> (playerOne <- {playerOne with Activity = Bid})
        //    | 2 -> (playerTwo <- {playerTwo with Activity = Bid})
        //    | 3 -> (playerThree <- {playerThree with Activity = Bid})
        //    | _ -> failwith "Wrong player ID."
        //    model, Cmd.none, DoNothing
        //| UpdateBid b -> { model with HighestBid = b }, Cmd.none, DoNothing
        //| ExitBidding id ->
        //    match id with
        //    | 1 -> (playerOne <- {playerOne with Activity = Reject })
        //    | 2 -> (playerTwo <- {playerTwo with Activity = Reject })
        //    | 3 -> (playerThree <- {playerThree with Activity = Reject })
        //    | _ -> failwith "Wrong player ID."
        //    model, Cmd.none, DoNothing
        | PassBid ->
            let currentPlayer = getPlayer model model.CurrentPlayer
            let updatedPlayer = setPlayer model model.CurrentPlayer { currentPlayer with Activity = Reject }
            let newPassed = model.Passed.Add currentPlayer
            let newPassedModel = { updatedPlayer with Passed = newPassed }
            //let next = nextSkatPlayer model.CurrentPlayer
            let finished =
                newPassed.Count >= 2 // two players passed → bidding done
            let newModel = { newPassedModel with Finished = finished }
            //{ model with
            //    Passed = newPassed
            //    CurrentPlayer = if model.Finished then model.CurrentPlayer else next
            //    Finished = finished }
            newModel, Cmd.ofMsg MoveNextPlayer, DoNothing
            //Cmd.ofMsg (ExitBidding id), DoNothing
        | MakeBid ->
            let parsed, input = System.Int32.TryParse(model.UserInput)
            let highest =
                match parsed with
                | false -> failwith "Input must be integers."
                | true -> 
                    match model.HighestBid with
                    | None -> true, input
                    | Some x when input > x -> true, input
                    | Some x -> false, x
            let selectedPlayer = getPlayer model (model.CurrentPlayer)
            let updatedPlayer =
                match highest with
                | (true, i) -> { selectedPlayer with Amount = Some i }
                | (false, _) -> selectedPlayer
            let newModelWithPlayer = setPlayer model (model.CurrentPlayer) updatedPlayer
            let newModel = 
                match highest with
                | (_, i) -> { newModelWithPlayer with HighestBid = Some i }
            newModel, Cmd.ofMsg MoveNextPlayer, DoNothing
        | MoveNextPlayer -> 
            let newModel = nextPlayer model model.CurrentPlayer
            newModel, Cmd.none, DoNothing
        | AcceptBid id -> 
            let selectedPlayer = getPlayer model id
            let updatedPlayer = { selectedPlayer with Activity = Bid }
            let newModel = setPlayer model id updatedPlayer
            newModel, Cmd.none, DoNothing

let view model =
            ContentPage(
                ScrollView(
                    (VStack (spacing = 25.) {
                        
                        Label(
                            if model.Finished then
                                $"Bidding finished! Highest bid: {model.HighestBid |> Option.defaultValue 0}"
                                //Button("4th page", MoveFourthPage)
                            else
                                $"Current: {model.CurrentPlayer}, Highest: {model.HighestBid |> Option.defaultValue 0}"
                        )

                        if not model.Finished then
                            Button($"{model.CurrentPlayer}, do you want to bid", AcceptBid (model.CurrentPlayer))

                            Entry(model.UserInput, TextUpdated)
                                .keyboard(Keyboard.Numeric)
                                .onCompleted(MakeBid)
                                .isEnabled((getPlayer model model.CurrentPlayer).Activity = Bid)

                            Label($"{model.HighestBid}")

                            Label($"{model.PlayerOne}")
                            Label($"{model.PlayerTwo}")
                            Label($"{model.PlayerThree}")

                            Button($"{model.CurrentPlayer} Pass", PassBid)
                        else
                            Picker(["Hearts"; "Diamonds"; "Spades"; "Clubs"; "Grand"; "NullGame"], gameToIndex model.Game, SetGame)
                            Label($"Game: {model.Game}")
                            Button("4th page", MoveFourthPage)
                            //Button($"{model.CurrentPlayer}, do you want to bid", AcceptBid 2)

                            //Entry("", MakeBid)
                            //    .placeholder($"The current bid is {model.HighestBid}.")
                            //    .keyboard(Keyboard.Numeric)
                            //    .onCompleted(MoveNextPlayer)

                            //Button($"{model.CurrentPlayer} Pass", PassBid 2)

                            //Button($"{model.CurrentPlayer}, do you want to bid", AcceptBid 3)

                            //Entry("", MakeBid)
                            //    .placeholder($"The current bid is {model.HighestBid}.")
                            //    .keyboard(Keyboard.Numeric)
                            //    .onCompleted(MoveNextPlayer)

                            //Button($"{model.CurrentPlayer} Pass", PassBid 3)
                            

                        //Label($"Hello, {playerOne.Player}. Activity: {playerOne.Activity} Your bid is {model.Bid}. {playerOne.Amount}")
                        //    .semantics(SemanticHeadingLevel.Level1)
                        //    .font(size = 32.)
                        //    .centerTextHorizontal()

                        //Entry(model.Bid.ToString(), UpdateBid)
                        //    .placeholder("Please enter a bid.")
                        //    .onCompleted(UpdateUser 1)

                        //Button("Exit bidding.", ExitBidding 1)

                        //ListView(cardSetup.FirstPlayer)
                        //    (fun card -> 
                        //        ViewCell(
                        //            Image((cardToImageName card))
                        //                .height(64.)
                        //        ))

                        Button($"1st page - {model.Game}", ReturnFirstPage)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )