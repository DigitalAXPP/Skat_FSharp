module PlayerLogic

type PlayerID = {
    ID: int
    Username: string
}

let rec loginPlayer (playerID: int) =
    printf "Please enter the username for player %i" playerID
    let name = System.Console.ReadLine()
    if System.String.IsNullOrWhiteSpace(name) then
        printf "Not a valid name. Try again"
        loginPlayer playerID
    else
        { ID = playerID; Username = name }

let loginAllPlayers () =
    [1..3]
    |> List.map loginPlayer