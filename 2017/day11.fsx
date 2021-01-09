open System.IO;

let input = File.ReadAllText("day11.txt").Split ','
let calc x y =
    if x*y<0 then
        max (abs x) (abs y)
    else
        abs x+y
let res =
    input |> Array.fold(fun acc value ->
        let (x, y, maxDistance) = acc
        let dist = max maxDistance (calc x y)
        match value with
            |"n"-> (x+1, y-1, dist)
            |"ne"-> (x+1, y, dist)
            |"se"-> (x, y+1, dist)
            |"s"-> (x-1, y+1, dist)
            |"sw"-> (x-1, y, dist)
            |"nw"-> (x , y-1, dist)
            |_ -> invalidArg "err" (sprintf "%s is not valid" value)
    ) (0,0,0)
    //move 0 0 input
let (x, y, maxDist) = res
let dist = calc x y
printfn "%d %d" dist maxDist