open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Microsoft.AspNetCore.SignalR
open System.Collections.Concurrent

//[<EntryPoint>]
//let main args =
//    let builder = WebApplication.CreateBuilder(args)
//    let app = builder.Build()

//    app.MapGet("/", Func<string>(fun () -> "Hello World!")) |> ignore

//    app.Run()

    //0 // Exit code
module GameStore =
    let games = ConcurrentDictionary<string, ResizeArray<string>>()

    let addPlayer gameId playerName =
        let players = games.GetOrAdd(gameId, fun _ -> ResizeArray())
        if not (players.Contains playerName) then
            players.Add playerName
        players

type GameHub() =
    inherit Hub()

    member this.JoinGame (gameId: string, playerName: string) =
        task {
            do! this.Groups.AddToGroupAsync (this.Context.ConnectionId, gameId)
            let players = GameStore.addPlayer gameId playerName
            do! this.Clients.Group(gameId).SendAsync("PlayersUpdate", players)
        }

    member this.QuiteGame (gameId: string, playerName: string) =
        task {
            do! this.Groups.AddToGroupAsync (this.Context.ConnectionId, gameId)
            match GameStore.games.TryGetValue gameId with
            | true, players ->
                players.Remove playerName |> ignore
                do! this.Clients.Group(gameId).SendAsync("PlayersUpdate", playerName)
            | _ -> ()
        }

    member this.SendMove (move: string) =
        this.Clients.All.SendAsync("ReceiveMove", move)

let builder = WebApplication.CreateBuilder()
builder.Services.AddSignalR() |> ignore

let app = builder.Build()
app.MapHub<GameHub>("/gamehub") |> ignore
app.Run()

