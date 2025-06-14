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
let ``UpdatePlayerAmountTests`` () =
    //-- Arrange
    let firstPlayer = {
        Player = 1
        Activity = Bid
        Amount = None
    }
    //-- Act
    let firstPlayer = updatePlayerAmount firstPlayer 100

    //-- Assert
    Assert.Equal (firstPlayer.Amount, Some 100)

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
    let firstPlayer = {
        Player = 1
        Activity = Undecided
        Amount = None
    }
    let input = new StringReader "Yes"
    Console.SetIn input
    
    //-- Act
    let result = getPlayerAction firstPlayer

    //-- Assert
    Assert.Equal (result.Activity, Bid)

[<Fact>]
let ``GetPlayerAction_No_Tests`` () =
    //-- Arrange
    let firstPlayer = {
        Player = 1
        Activity = Undecided
        Amount = None
    }
    let input = new StringReader "No"
    Console.SetIn input
    
    //-- Act
    let result = getPlayerAction firstPlayer

    //-- Assert
    Assert.Equal (result.Activity, Reject)