module SkatRules

type GameState =
    {
        Players: string list
        Score: int
    }

let mutable game : GameState option = None

let startGame () =
    game <- Some { Players = ["Alex"; "Lukas"; "Finn"]; Score = 0}