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

type WinnerInfo = {
    Winner:int;
    Value:list<int>;
}


let rec play (p1Duplicate:list<list<int>>) (p2Duplicate:list<list<int>>) (player1:list<int>) (player2:list<int>):WinnerInfo =
    if List.isEmpty player1 then
        {Winner = 2; Value = player2}
    else if List.isEmpty player2 then
        {Winner = 1; Value = player1}
    else if (p1Duplicate |> List.exists (fun f->f = player1) || p2Duplicate |> List.exists (fun f->f = player2)) then
        {Winner = 1; Value = player1}
    else
        let playrec = play (p1Duplicate @ [player1]) (p2Duplicate @ [player2])
        let card1 = player1.Head
        let card2 = player2.Head
        if (card1 < player1.Length && card2 < player2.Length) then
            match (play [] [] ( player1.Tail |> Seq.take card1 |> List.ofSeq ) ( player2.Tail |> Seq.take card2 |> List.ofSeq )).Winner with
            | 1 -> playrec (player1.Tail @ [card1;card2]) player2.Tail
            | 2 -> playrec player1.Tail (player2.Tail @ [card2;card1])
            | _ -> invalidOp "panik"
        else
            if card1 > card2 then
                playrec (player1.Tail @ [card1;card2]) player2.Tail
            else
                playrec player1.Tail (player2.Tail @ [card2;card1])

//let rec combat (pl1:list<int>) (pl2:list<int>) =
    //play [] [] pl1 pl2

let res = (play [] [] p1 p2).Value

printfn "%A" (res |> List.fold (fun acc v-> [|acc.[0]+v*acc.[1]; acc.[1]-1 |] ) [|0; res.Length|] ).[0]