module SignalRClient

open Microsoft.AspNetCore.SignalR.Client

type ConnectionState =
    | HubDisconnected
    | HubConnecting
    | HubConnected of HubConnection

type ServerMsg =
    | JoinGame of string
    | QuitGame of string
    | ReceiveMove of string

let connect (hubUrl : string) (dispatch : ServerMsg -> unit) =
    task {
        let hub =
            HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                .Build()

        hub.On<string>("JoinGame", fun name -> 
            dispatch (JoinGame name)) |> ignore

        hub.On<string>("ReceiveMove", fun move ->
            dispatch (ReceiveMove move)) |> ignore

        do! hub.StartAsync()
        printf "Connected to %s" hubUrl
        return hub
    }

let disconnect (hub: HubConnection) =
    task {
        if not (isNull hub) then
            do! hub.StopAsync()
            do! hub.DisposeAsync().AsTask()
            printfn "Hub disconnected."
    }