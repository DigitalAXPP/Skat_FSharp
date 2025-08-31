module ThirdPage

open Microsoft.Maui
open Microsoft.Maui.Controls
open Fabulous
open Fabulous.Maui
open type Fabulous.Maui.View
open GamePlay
open GameFoundation
open System

type Intent =
    | DoNothing
    | BackFirstPage

type Model = { 
    Name: string 
    Bid: string}

type Msg =
        | ReturnFirstPage
        | UpdateBid of string
        | ExitBidding of int
        | UpdateUser of int

//let makeGridCollectionView (items: string list) =
//    let cv : CollectionView = CollectionView()
//    let layout : ItemsLayout = GridItemsLayout(3, ItemsLayoutOrientation.Vertical)
//    cv.ItemsLayout <- layout
//    cv.ItemTemplate <- DataTemplate(fun () ->
//        let img : Microsoft.Maui.Controls.Image = Microsoft.Maui.Controls.Image(Aspect = Aspect.AspectFit)
//        img.SetBinding(Microsoft.Maui.Controls.Image.SourceProperty, ".")
//        img :> View
//    )
//    cv.ItemsSource <- items
//    cv
let cardSetup = dealInitialHand Deck

let init () = { 
    Name = "Skat" 
    Bid = "18"}

let update msg model =
        match msg with
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage
        | UpdateUser u ->
            let t, input = System.Int32.TryParse(model.Bid)
            match u with
            | 1 -> (playerOne <- {playerOne with Amount = Some input})
            | 2 -> (playerTwo <- {playerTwo with Amount = Some input})
            | 3 -> (playerThree <- {playerThree with Amount = Some input})
            | _ -> failwith "Wrong player ID."
            model, Cmd.none, DoNothing
        | UpdateBid b -> { model with Bid = b }, Cmd.none, DoNothing
        | ExitBidding id ->
            match id with
            | 1 -> (playerOne <- {playerOne with Activity = Reject })
            | 2 -> (playerTwo <- {playerTwo with Activity = Reject })
            | 3 -> (playerThree <- {playerThree with Activity = Reject })
            | _ -> failwith "Wrong player ID."
            model, Cmd.none, DoNothing

let view model =
            ContentPage(
                ScrollView(
                    (VStack (spacing = 25.) {

                        Label($"Hello, {playerOne.Player}. Activity: {playerOne.Activity} Your bid is {model.Bid}. {playerOne.Amount}")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Entry(model.Bid.ToString(), UpdateBid)
                            .placeholder("Please enter a bid.")
                            .onCompleted(UpdateUser 1)

                        Button("Exit bidding.", ExitBidding 1)

                        ListView(cardSetup.FirstPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))   

                        Label($"Hello, {playerTwo.Player}. You are at position {playerTwo.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        ListView(cardSetup.SecondPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))  

                        Label($"Hello, {playerThree.Player}. You are at position {playerThree.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        ListView(cardSetup.ThirdPlayer)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))  

                        Button($"1st page - {model.Name}", ReturnFirstPage)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )