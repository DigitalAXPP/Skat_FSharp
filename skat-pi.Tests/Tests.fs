module Tests

open System
open System.IO
open Xunit
open Reizen
open GameFoundation

[<Fact>]
let ``My test`` () =
    Assert.True(true)

[<Fact>]
let ``UpdatePlayerOneAmountTests`` () =
    //-- Arrange
    let input = new StringReader "10"
    Console.SetIn input
    //-- Act
    biddingM 1 9 |> ignore

    //-- Assert
    Assert.Equal (Some 10, playerOne.Amount)

[<Fact>]
let ``UpdatePlayerActivityTests`` () =
    //-- Arrange
    let firstPlayer = {
        Player = 1
        Activity = Undecided
        Amount = None
    }
    //-- Act
    let firstPlayer = updatePlayerActivity firstPlayer Reject
    
    //-- Assert
    Assert.Equal (firstPlayer.Activity, Reject)

[<Fact>]
let ``GetPlayerAction_Yes_Tests`` () =
    //-- Arrange
    let input = new StringReader "Yes"
    Console.SetIn input
    
    //-- Act
    getPlayerAction 1

    //-- Assert
    Assert.Equal (Bid, playerOne.Activity)

[<Fact>]
let ``GetPlayerAction_No_Tests`` () =
    //-- Arrange
    let input = new StringReader "No"
    Console.SetIn input
    
    //-- Act
    getPlayerAction 1

    //-- Assert
    Assert.Equal (Reject, playerOne.Activity)