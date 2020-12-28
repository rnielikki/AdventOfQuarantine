open System.IO;

let reader = new StreamReader("d22.txt");

let rec getLine (lst:list<int>) =
    if reader.EndOfStream then
        lst
    else
        let line = reader.ReadLine();
        if line.Length < 1 then
            lst
        else if line.StartsWith 'P' then
            getLine lst
        else
            getLine (lst @ [line |> int])


let p1 = getLine []
let p2 = getLine []

printfn "%A P1" p1
printfn "%A P2" p2

let rec play (player1:list<int>) (player2:list<int>) =
    if (List.isEmpty player1 || List.isEmpty player2) then
        (player1, player2)
    else
        if player1.Head > player2.Head then
            play (player1.Tail @ [player1.Head;player2.Head]) player2.Tail
        else
            play player1.Tail (player2.Tail @ [player2.Head;player1.Head])

let (p1res, p2res) = play p1 p2
if List.isEmpty p1res then
    printfn "player2 wins, %A" (p2res |> List.fold (fun acc v-> [|acc.[0]+v*acc.[1]; acc.[1]-1 |] ) [|0; p2res.Length|] ).[0]
else
    printfn "player1 wins, %A" (p1res |> List.fold (fun acc v-> [|acc.[0]+v*acc.[1]; acc.[1]-1 |] ) [|0; p1res.Length|] ).[0]