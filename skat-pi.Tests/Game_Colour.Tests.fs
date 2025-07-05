module Game_Colour

open Xunit
open GameFoundation
open GamePlay

[<Fact>]
let ``CardStrengthTests`` () =
    //-- Arrange
    let jackHand = {Suite = Diamonds; Rank = Jack}
    let trumpSuiteHand = {Suite = Hearts; Rank = Ace}
    let nullHand = {Suite = Spades; Rank = Eight}

    //-- Act
    let resultJack = cardStrength (SuitGame Spades) jackHand
    let resultTumpSuite = cardStrength (SuitGame Hearts) trumpSuiteHand
    let resultnullHand = cardStrength (SuitGame Clubs) nullHand

    //-- Assert
    Assert.Equal (103, resultJack)
    Assert.Equal (56, resultTumpSuite)
    Assert.Equal (1, resultnullHand)