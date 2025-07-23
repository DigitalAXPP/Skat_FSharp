//namespace Firstpage
module firstpage

open Fabulous
open Fabulous.Maui
open Microsoft.Maui.Accessibility
open Microsoft.Maui
open type Fabulous.Maui.View

//open GameFoundation



//    type Model = { Count: int }
type Model = { Name: string }

//    type Msg = | Clicked
type Msg = | Clicked

//    type CmdMsg = SemanticAnnounce of string

//    let semanticAnnounce text =
//        Cmd.ofSub(fun _ -> SemanticScreenReader.Announce(text))

//    let mapCmd cmdMsg =
//        match cmdMsg with
//        | SemanticAnnounce text -> semanticAnnounce text

//    let init () = { Count = 0 }, []
let init () = { Name = "Alex"}

//    let update msg model =
//        match msg with
//        | Clicked -> { model with Count = model.Count + 1 }, [ SemanticAnnounce $"Clicked {model.Count} times" ]
let update msg model =
        match msg with
        | Clicked -> { model with Name = "Verca"}

//    let view model =
//        Application(
//            ContentPage(
//                ScrollView(
//                    (VStack(spacing = 25.) {
//                        Image("dotnet_bot.png")
//                            .semantics(description = "Cute dotnet bot waving hi to you!")
//                            .height(200.)
//                            .centerHorizontal()

//                        Label("Hello, World!")
//                            .semantics(SemanticHeadingLevel.Level1)
//                            .font(size = 32.)
//                            .centerTextHorizontal()

//                        Label("Welcome to .NET Multi-platform App UI powered by Fabulous")
//                            .semantics(SemanticHeadingLevel.Level2, "Welcome to dot net Multi platform App U I powered by Fabulous")
//                            .font(size = 18.)
//                            .centerTextHorizontal()

//                        Label($"This is a test label.{playerOne.Activity}")
//                            .semantics(SemanticHeadingLevel.Level2, $"test: {playerOne.Activity}")
//                            .font(size = 18.)
//                            .centerTextHorizontal()

//                        let text =
//                            if model.Count = 0 then
//                                "Click me"
//                            else
//                                $"Clicked {model.Count} times"

//                        Button(text, Clicked)
//                            .semantics(hint = "Counts the number of times you click")
//                            .centerHorizontal()
//                    })
//                        .padding(30., 0., 30., 0.)
//                        .centerVertical()
//                )
//            )
//        )

let view model =
        //Application(
            ContentPage(
                (VStack (spacing =25.) {
                    Label($"Hello, {model.Name}")
                        .semantics(SemanticHeadingLevel.Level1)
                        .font(size = 32.)
                        .centerTextHorizontal()

                    Button("Click", Clicked)
                })
            )
        //)