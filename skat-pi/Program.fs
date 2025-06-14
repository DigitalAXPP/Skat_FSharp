// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"
open GameFoundation
open GamePlay
open Reizen

[<EntryPoint>]
let main args =
    let startingHands = dealInitialHand Deck
    let reizResult = startReizAction ()
    let firstPlayer = getUsers ()
    printf "%A" startingHands
    printf "%A" reizResult
    printf "%i %A" firstPlayer.Player firstPlayer.Activity
    0
