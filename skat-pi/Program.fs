// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"
open GameFoundation
open GamePlay
open Reizen

[<EntryPoint>]
let main args =
    let startingHands = dealInitialHand Deck
    let reizResult = startReizAction ()
    printf "%A" startingHands
    printf "%A" reizResult
    printf "%i %A" playerOne.Player playerOne.Activity
    0
