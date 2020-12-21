open System.IO;
open System.Linq;

let file = File.ReadAllLines("d10.txt").Select(fun f -> f |> int).ToList();
file.AddRange([0;file.Max()+3])
let data = file |> Seq.sortBy(fun x -> x);
let length = file.Count;

//---------------------------- one :::::::::::::::: 2482

printfn "--------------- ONE -----------------"
//the WTF way?
//One of the WTF is approaching with The System Collection Generic List.
let one = new System.Collections.Generic.List<int>();
let two = new System.Collections.Generic.List<int>();
let three = new System.Collections.Generic.List<int>();

//lol. use For loop dude.
let rec putData (seq:int[]) (itemIndex:int) =
    if itemIndex < length then
        match seq.[itemIndex] - seq.[itemIndex-1] with
        | 1 -> one.Add(seq.[itemIndex])
        | 2 -> two.Add(seq.[itemIndex])
        | 3 -> three.Add(seq.[itemIndex])
        | _ -> raise(new System.ArgumentException("lol... what?"))
        putData seq (itemIndex+1)

putData (data.ToArray()) 1

let printArray (items:seq<_>) = 
    items |> Seq.iter(fun a -> printf "%A; " a)
    printfn ""

printArray one
printArray two
printArray three
printfn "result is %d" (one.Count * three.Count)

//--------------- two :::::::::::::::::::::::::::::: 96717311574016

printfn "--------------- TWO -----------------"
let removableTargets = new System.Collections.Generic.List<int[]>();
let rec group (seq:int[]) (itemIndex:int) (current:int[]) =
    if (itemIndex < seq.Length) then
        if (seq.[itemIndex] - seq.[itemIndex-1] < 3) then
            group seq (itemIndex+1) (current.Append(seq.[itemIndex-1]).ToArray());
        else
            removableTargets.Add(current);
            group seq (itemIndex+1) [||]
    else
        removableTargets.Add(current)

group (one.ToArray()) 1 [||];

let exceptionCalculator (len:int):int64 =
    if len < 3 then pown 2 len |> int64
    else pown 2 len - ((len-1)*(len-2)/2) |> int64

let res = removableTargets.Aggregate(1 |> int64, fun acc src -> acc * exceptionCalculator src.Length)

printfn "%A" res
