open System;
open System.IO;
open System.Linq;

let parseInt (charSequence:seq<char>) =
    let result = charSequence.TakeWhile (fun c-> Char.IsDigit c)
    result |> String.Concat |> int64

let raw = File.ReadAllLines("d18.txt");
let validOperators = [|'+';'*'|];

let calc (input:string):int64 =
    let charSeq = input.ToCharArray() |> List.ofSeq
    let rec calcWhile (input:seq<char>) (currentData:int64) (operator:char) =
        if input.Any() then
            let matchTarget = input.First();
            if (Char.IsDigit matchTarget) then
                let number = parseInt input
                if operator <> '_' then
                    match operator with
                    |'+'->
                        calcWhile (input.Skip 1) (currentData + number) '_'
                    |'*'->
                        calcWhile (input.Skip 1) (currentData * number) '_'
                    |_-> raise (new System.FormatException("unexpected format "+matchTarget.ToString()+" with "+operator.ToString()))
                else
                    calcWhile (input.Skip 1) currentData '_'
            else
                if (Array.contains matchTarget validOperators) then
                    calcWhile (input.Skip 1) currentData matchTarget
                else
                    calcWhile (input.Skip 1) currentData operator
        else
            currentData
    calcWhile input (parseInt input) '_'

let rec calcRecursion (input:string) =
    if (input.IndexOf '(' <> -1) then
        let starts = [| input.Substring (0, (input.LastIndexOf '(')); input.Substring (input.LastIndexOf '('+1) |]
        let ends = starts.[1].Split (')', 2)
        calcRecursion (starts.[0] + (calc ends.[0]).ToString() + ends.[1])
    else
        calc input

let rec calcRecursionV2 (input:string) =
    let addFirst (str:string) =
        let splitted = str.Split '*'
        splitted |> Array.fold (fun acc item -> acc*(item.Trim() |> calc)) 1L
    if (input.IndexOf '(' <> -1) then
        let starts = [| input.Substring (0, (input.LastIndexOf '(')); input.Substring (input.LastIndexOf '('+1) |]
        let ends = starts.[1].Split (')', 2)
        calcRecursionV2 (starts.[0] + (addFirst ends.[0]).ToString() + ends.[1])
    else
        addFirst input

let result1 = raw |> Array.fold(fun acc line -> acc+(calcRecursion line)) 0L
printfn "%A" result1
let result2 = raw |> Array.fold(fun acc line -> acc+(calcRecursionV2 line)) 0L
printfn "%A" result2