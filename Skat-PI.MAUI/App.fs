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
        | PageThird
        | PageFour of GameType

    type Model = {
        Step: Step
        FirstpageModel: firstpage.Model
        SecondpageModel: SecondPage.Model option
        ThirdpageModel: ThirdPage.Model
        FourthpageModel: FourPage.Model
    }

    type Msg =
        | FirstPageMsg of firstpage.Msg
        | SecondPageMsg of SecondPage.Msg
        | ThirdPageMsg of ThirdPage.Msg
        | FourthPageMsg of FourPage.Msg
        | NextStep of Step
        | BackStep of Step

    let init () = {
        Step = PageFirst
        FirstpageModel = firstpage.init ()
        SecondpageModel = None
        ThirdpageModel = ThirdPage.init ()
        FourthpageModel = FourPage.init (NullGame)}, Cmd.none

    let update msg model =
        match msg with
        | NextStep step ->
            //let newStep =
                match step with
                | PageFirst -> 
                    { model with 
                        Step = PageFirst
                        FirstpageModel = firstpage.init () }, Cmd.none
                | PageSecond -> 
                    { model with 
                        Step = PageSecond
                        SecondpageModel = Some (SecondPage.init ()) }, Cmd.none
                | PageThird -> 
                    { model with 
                        Step = PageThird
                        ThirdpageModel = ThirdPage.init ()}, Cmd.none
                | PageFour value ->
                    { model with 
                        Step = PageFour value
                        FourthpageModel = FourPage.init value}, Cmd.none

            //{ newStep with Step = step }
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
                | SecondPage.Intent.ForwardThirdpage ->
                    let newModel = { model with SecondpageModel = Some updatedModel }
                    newModel, Cmd.batch [
                        Cmd.map SecondPageMsg cmd
                        Cmd.ofMsg (NextStep PageThird)
                    ]
            | None -> model, Cmd.none
        | ThirdPageMsg f3 ->
            let updatedModel, cmd, intent = ThirdPage.update f3 model.ThirdpageModel
            match intent with
            | ThirdPage.Intent.DoNothing -> { model with ThirdpageModel = updatedModel }, Cmd.map ThirdPageMsg cmd
            | ThirdPage.Intent.BackFirstPage -> 
                let newModel = { model with ThirdpageModel = updatedModel }
                newModel, Cmd.batch [
                    Cmd.map ThirdPageMsg cmd
                    Cmd.ofMsg (BackStep PageFirst)
                ]
            | ThirdPage.Intent.ForwardFourthPage s ->
                let newModel = { model with ThirdpageModel = updatedModel }
                newModel, Cmd.batch [
                    Cmd.map ThirdPageMsg cmd
                    Cmd.ofMsg (NextStep (PageFour s))
                ]
        | FourthPageMsg f4 ->
            let updatedModel, cmd, intent = FourPage.update f4 model.FourthpageModel
            match intent with
            | FourPage.Intent.DoNothing -> { model with FourthpageModel = updatedModel }, Cmd.map FourthPageMsg cmd

    let view model =
        Application (
            NavigationPage () {
                //View.map FirstPageMsg (firstpage.view model.FirstpageModel)
                match model.Step with
                | PageFirst -> View.map FirstPageMsg (firstpage.view model.FirstpageModel)
                | PageSecond -> View.map SecondPageMsg (SecondPage.view model.SecondpageModel.Value)
                | PageThird -> View.map ThirdPageMsg (ThirdPage.view model.ThirdpageModel)
                | PageFour _ -> View.map FourthPageMsg (FourPage.view model.FourthpageModel)
                //if model.Step = PageSecond then
                    //match model.SecondpageModel with
                    //| None -> View.map FirstPageMsg (firstpage.view model.FirstpageModel)
                    //| Some v -> View.map SecondPageMsg (SecondPage.view v)
            }
        )
    let program = Program.statefulWithCmd init update view
