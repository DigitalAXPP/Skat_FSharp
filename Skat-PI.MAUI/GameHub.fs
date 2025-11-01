module GameHub

open System.Threading.Tasks
open SignalRClient
open Microsoft.AspNetCore.SignalR.Client
open Fabulous

type Model = { 
    Status: string
    Moves: string list
    Connection: ConnectionState
}

type Msg =
    | TestHub
    | ConnectHub
    | ConnectedHub of HubConnection
    | DisconnectedHub
    | EnterGame of string
    | GameJoined
    | LeaveGame
    | ReceiveMove of string
    | SendMove of string
    | ConnectionHubFailed of string

let init() = {
    Status = "Not connected"
    Moves = []
    Connection = HubDisconnected}, Cmd.none

let update msg model =
    match msg, model.Connection with
    | GameJoined, _ -> {model with Status = "Game Joined"}, Cmd.none
    | ConnectHub, _ ->
        let cmd = 
            Cmd.ofAsyncMsg (async {
                try
                    let! hub = connect "http://localhost:5000/gamehub" ignore |> Async.AwaitTask
                    return ConnectedHub hub
                with exn ->
                    return ConnectionHubFailed exn.Message
            })
                
        { model with Status = "Connecting..." }, cmd

    | ConnectedHub hub, _ ->
        { model with
            Status = "Connected"
            Connection = HubConnected hub },
        Cmd.none

    | EnterGame name, _ ->
        match model.Connection with
        | HubConnected hub -> 
            let cmd =
                Cmd.ofAsyncMsg (async {
                    try
                        do! hub.InvokeAsync("JoinGame", "game1", name) |> Async.AwaitTask
                        return GameJoined
                    with exn ->
                        return ConnectionHubFailed exn.Message
                })
            model, cmd

    | LeaveGame, _ ->
        model, Cmd.none

    | ReceiveMove move, _ ->
        { model with Moves = move :: model.Moves },
        Cmd.none

    | SendMove move, HubConnected hub ->
        let cmd =
            Cmd.ofAsyncMsg (async {
                do! hub.SendAsync("SendMove", move) |> Async.AwaitTask
                return ConnectedHub hub
            })
        { model with Status = $"Sent: {move}" }, cmd

    | ConnectionHubFailed err, _ ->
        { model with Status = $"Connection failed: {err}" }, Cmd.none

    | _ -> model, Cmd.none
