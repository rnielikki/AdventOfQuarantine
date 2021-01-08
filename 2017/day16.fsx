open System.IO;

let len = 16
let instructions = (File.ReadAllText "day16.txt").Split ','
let alphabets = seq { 97..(97+len-1) } |> Array.ofSeq


let spin (s:array<int>) (amount:int) =
    let cutSize = len - amount
    Array.append (s |> Array.skip(cutSize)) (s |> Array.take(cutSize))
let exchange (s:array<int>) (first:int) (second:int) =
    let copied = Array.copy s
    let temp = copied.[first]
    copied.[first] <- copied.[second]
    copied.[second] <- temp
    copied
let partner (s:array<int>) (first:char) (second:char) =
    let first = Array.findIndex (fun item -> item = (first|>int)) s
    let second = Array.findIndex (fun item -> item = (second|>int)) s
    exchange s first second

let rec startApp (instructions:array<string>) (index:int) (input:array<int>) =
    if (index >= (instructions.Length)) then
        input
    else
        let start = startApp instructions (index+1)
        let instruction = instructions.[index]
        let inst = instruction.Substring 1
        let res = match instruction.[0] with
        |'s'-> spin input (inst |> int)
        |'x'->
            let targets = inst.Split '/' |> Array.map (fun f -> f |> int)
            exchange input targets.[0] targets.[1]
        |'p'->
            let targets = inst.Split '/' |> Array.map (fun f -> f.[0])
            partner input targets.[0] targets.[1]
        |_-> invalidArg "_" (sprintf "should not reach")
        start res

let rec Dance (input:array<int>) (amount:int)=
    let rec tryDance (arr:array<int>) (status:list<array<int>>) =
        if (arr = input && status.Length <> 0) then
            status
        else
            tryDance (startApp instructions 0 arr) (status @ [ Array.copy arr ])
    let danceResults = tryDance input List.empty
    danceResults.[amount%danceResults.Length]

//first
//let res = (startApp instructions 0 alphabets)// |> Array.map(fun x -> x |> char) |> System.String.Concat
//second
let res = (Dance alphabets 1000000000) |> Array.map(fun x -> x |> char ) |> System.String.Concat
printfn "%A" res