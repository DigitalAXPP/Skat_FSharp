module SecondPage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

open GameFoundation
open GameHub

type Intent =
    | DoNothing
    | BackFirstPage
    | ForwardThirdpage

type Model = { 
        Name: string
        Move: string
        Hub: GameHub.Model
    }

type Msg =
        | PasswordChanged of string
        | MoveSet of string
        | ReturnFirstPage
        | GoThirdPage
        | HubMsg of GameHub.Msg

let init () =
     let hubModel, hubCmd = GameHub.init()
     { Name = ""
       Move = ""
       Hub = hubModel}, Cmd.map HubMsg hubCmd

let update msg model =
        match msg with
        | PasswordChanged pwd -> { model with Name = pwd}, Cmd.none, DoNothing
        | MoveSet move -> { model with Move = move}, Cmd.none, DoNothing
        | ReturnFirstPage -> model, Cmd.none, BackFirstPage
        | GoThirdPage -> model, Cmd.none, ForwardThirdpage
        //| HubMsg ConnectHub ->
        //    model, Cmd.none, DoNothing
        | HubMsg hubMsg ->
        // You could delegate updates to GameHub.update if you have one
            let newHub, hubCmd = GameHub.update hubMsg model.Hub
            { model with Hub = newHub }, Cmd.map HubMsg hubCmd, DoNothing

let view model =
            ContentPage(
                ScrollView(
                    (VStack (spacing =25.) {
                        Label($"Hello, xxx")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Label($"Hello, {playerOne.Player}. You are at position {playerOne.Position}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Label($"{model.Hub.Status}.")
                            .semantics(SemanticHeadingLevel.Level1)
                            .font(size = 32.)
                            .centerTextHorizontal()

                        Button("1st page", ReturnFirstPage)

                        Button("3rd page", GoThirdPage)

                        Button("Connect", HubMsg ConnectHub)
                        Button("Disconnect", HubMsg DisconnectHub)
                        Button("Join Game", HubMsg (EnterGame model.Name))
                        Button("Leave Game", HubMsg (LeaveGame model.Name))
                        Button("Receive Move", HubMsg (ReceiveMove model.Move))

                        let cards = [ {Suite = Hearts; Rank = Eight}; {Suite = Clubs; Rank = Dame} ]
                        ListView(cards)
                            (fun card -> 
                                ViewCell(
                                    Image((cardToImageName card))
                                        .height(64.)
                                ))                            

                        Entry(model.Name, PasswordChanged)
                        Entry(model.Move, MoveSet)
                    }).margin(Thickness(10., 0.))
                ).verticalScrollBarVisibility(ScrollBarVisibility.Always)
            )