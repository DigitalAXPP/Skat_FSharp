namespace Skat_PI.MAUI

open Fabulous
open Fabulous.Maui
open Microsoft.Maui
open Microsoft.Maui.Graphics
open Microsoft.Maui.Accessibility
open Microsoft.Maui.Primitives
open GameFoundation

open type Fabulous.Maui.View

module App =
    
    type Step =
        | PageFirst
        | PageSecond

    //type Model = { Count: int }
    //type Model = { Name: string }
    type Model = {
        Step: Step
        FirstpageModel: firstpage.Model
        SecondpageModel: SecondPage.Model option
    }

    //type Msg = | Clicked
    //type Msg = | Clicked
    type Msg =
        | FirstPageMsg of firstpage.Msg
        | SecondPageMsg of SecondPage.Msg
        | NextStep of Step
        | BackStep of Step

    //type CmdMsg = SemanticAnnounce of string

    //let semanticAnnounce text =
    //    Cmd.ofSub(fun _ -> SemanticScreenReader.Announce(text))

    //let mapCmd cmdMsg =
    //    match cmdMsg with
    //    | SemanticAnnounce text -> semanticAnnounce text

    //let init () = { Count = 0 }, []
    //let init () = { Name = "Alex"}
    let init () = {
        Step = PageFirst
        FirstpageModel = firstpage.init ()
        SecondpageModel = None }, Cmd.none

    //let update msg model =
    //    match msg with
    //    | Clicked -> { model with Count = model.Count + 1 }, [ SemanticAnnounce $"Clicked {model.Count} times" ]
    //let update msg model =
    //    match msg with
    //    | Clicked -> { model with Name = "Verca"}
    let update msg model =
        match msg with
        //| FirstPageMsg f1 -> { model with FirstpageModel = firstpage.update f1 model.FirstpageModel }
        //| SecondPageMsg f2 -> { model with SecondpageModel = SecondPage.update f2 model.SecondpageModel}
        | NextStep step ->
            let newStep =
                match step with
                | PageFirst -> { model with FirstpageModel = firstpage.init () }
                | PageSecond -> { model with SecondpageModel = Some (SecondPage.init ()) }

            { newStep with Step = step }, Cmd.none
        | BackStep step ->
            let oldStep =
                match step with
                | PageFirst -> { model with SecondpageModel = None}
                | PageSecond -> { model with SecondpageModel = Some (SecondPage.init ())}

            { oldStep with Step = step }, Cmd.none
        | FirstPageMsg f1 ->
            let updatedModel, cmd, intent = firstpage.update f1 model.FirstpageModel
            match intent with
            | firstpage.Intent.DoNothing -> { model with FirstpageModel = updatedModel }, Cmd.map FirstPageMsg cmd
            | firstpage.Intent.SecondStep -> 
                let newModel = { model with FirstpageModel = updatedModel }
                newModel, Cmd.batch [
                    Cmd.map FirstPageMsg cmd
                    Cmd.ofMsg (NextStep PageSecond)
                ]
        | SecondPageMsg f2 -> 
            match model.SecondpageModel with
            | Some m ->
                let updatedModel, cmd, intent = SecondPage.update f2 m
                match intent with
                | SecondPage.Intent.DoNothing -> { model with SecondpageModel = Some updatedModel }, Cmd.map SecondPageMsg cmd
                | SecondPage.Intent.BackFirstPage -> 
                    let newModel = { model with SecondpageModel = Some updatedModel }
                    newModel, Cmd.batch [
                        Cmd.map SecondPageMsg cmd
                        Cmd.ofMsg (BackStep PageFirst)
                    ]
            | None -> model, Cmd.none


    //let view model =
    //    Application(
    //        ContentPage(
    //            ScrollView(
    //                (VStack(spacing = 25.) {
    //                    Image("dotnet_bot.png")
    //                        .semantics(description = "Cute dotnet bot waving hi to you!")
    //                        .height(200.)
    //                        .centerHorizontal()

    //                    Label("Hello, World!")
    //                        .semantics(SemanticHeadingLevel.Level1)
    //                        .font(size = 32.)
    //                        .centerTextHorizontal()

    //                    Label("Welcome to .NET Multi-platform App UI powered by Fabulous")
    //                        .semantics(SemanticHeadingLevel.Level2, "Welcome to dot net Multi platform App U I powered by Fabulous")
    //                        .font(size = 18.)
    //                        .centerTextHorizontal()

    //                    Label($"This is a test label.{playerOne.Activity}")
    //                        .semantics(SemanticHeadingLevel.Level2, $"test: {playerOne.Activity}")
    //                        .font(size = 18.)
    //                        .centerTextHorizontal()

    //                    let text =
    //                        if model.Count = 0 then
    //                            "Click me"
    //                        else
    //                            $"Clicked {model.Count} times"

    //                    Button(text, Clicked)
    //                        .semantics(hint = "Counts the number of times you click")
    //                        .centerHorizontal()
    //                })
    //                    .padding(30., 0., 30., 0.)
    //                    .centerVertical()
    //            )
    //        )
    //    )
    //let view model =
    //    Application(
    //        ContentPage(
    //            (VStack (spacing =25.) {
    //                Label($"Hello, {model.Name}")
    //                    .semantics(SemanticHeadingLevel.Level1)
    //                    .font(size = 32.)
    //                    .centerTextHorizontal()

    //                Button("Click", Clicked)
    //            })
    //        )
    //    )
    let view model =
        Application (
            NavigationPage () {
                View.map FirstPageMsg (firstpage.view model.FirstpageModel)
                if model.Step = PageSecond then
                    match model.SecondpageModel with
                    | None -> View.map FirstPageMsg (firstpage.view model.FirstpageModel)
                    | Some v -> View.map SecondPageMsg (SecondPage.view v)
            }
        )
    
    //let program = Program.statefulWithCmdMsg init update view mapCmd
    //let program = Program.stateful init update view
    let program = Program.statefulWithCmd init update view
