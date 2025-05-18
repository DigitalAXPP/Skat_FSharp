// For more information see https://aka.ms/fsharp-console-apps
//printfn "Hello from F#"
open GameFoundation

[<EntryPoint>]
let main args =
    let startingHands = dealInitialHand Deck
    printf "%A" startingHands
    //while gamestate do
    //    let fst, snd = nextTurn initialState
    //    printf "It's player %i turn" fst
    //    printf "Write something"
    //    let x = System.Console.ReadLine()
    //    printf "%s" x
    gameloop 1 initialState
    0
