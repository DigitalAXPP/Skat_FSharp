module SkatRules

type GameState =
    {
        Players: string list
        Score: int
    }

let mutable game : GameState option = None

let startGame () =
    game <- Some { Players = ["Alex"; "Lukas"; "Finn"]; Score = 0}

let updateGame (increase : int) =
    match game with
    | Some g -> game <- Some { g with Score = g.Score + increase}
    | None -> printfn "None"