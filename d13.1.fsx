open System.IO;

let data = File.ReadAllLines("d13.txt");
let time = data.[0]  |> int32
let __data1 = data.[1].Split ','
let buses = __data1 |> Array.filter(fun x -> x <> "x") |> Array.map (fun x -> x |> int32)

let xCount = __data1 |> Array.filter(fun x -> x = "x") |> Array.length
let headBus = buses.[0]
let tailBus = buses.[buses.Length-1]

let rec nextBus (position:int64) (waitedTime:int32) =
    let validBus = buses |> Array.filter(fun x -> (position%(x |> int64) |> int32) = 0)
    match validBus.Length with
    | 0 -> nextBus (position+1L) (waitedTime+1)
    | _ -> (waitedTime, validBus.[0])
//    | _ -> raise (new System.ArgumentException("well..."))

let (waited:int32, busNum:int32) = nextBus (time |> int64) 0
printfn "%A" (waited*busNum)