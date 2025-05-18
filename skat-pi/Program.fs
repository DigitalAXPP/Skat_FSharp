// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"
open GameFoundation

[<EntryPoint>]
let main args =
    let startingHands = dealInitialHand Deck
    printf "%A" startingHands
    gameloop 1 initialState
    0
