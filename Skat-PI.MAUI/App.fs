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

    type Model = {
        Step: Step
        FirstpageModel: firstpage.Model
        SecondpageModel: SecondPage.Model option
    }

    type Msg =
        | FirstPageMsg of firstpage.Msg
        | SecondPageMsg of SecondPage.Msg
        | NextStep of Step
        | BackStep of Step

    let init () = {
        Step = PageFirst
        FirstpageModel = firstpage.init ()
        SecondpageModel = None }, Cmd.none

    let update msg model =
        match msg with
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
    let program = Program.statefulWithCmd init update view
