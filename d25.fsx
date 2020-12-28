open System.IO;
open System.Linq;

let raw = File.ReadAllLines("d25.txt") |> Array.map(fun aaa -> aaa |> int64)
let card = raw.First()
let door = raw.Last();
//let value = 1L;
//let loop = 0;

let rec calc1 (value:int64) (loop:int) =
    if value = card then
        loop
    else
        calc1 ((value*7L) % 20201227L) (loop+1)
 
let rec calc2 (value:int64) (loop:int) (targetLoop:int) =
    if loop = targetLoop then
        value
    else
        calc2 ((value*door) % 20201227L) (loop+1) targetLoop

printfn "%A" (calc1 1L 0 |> calc2 1L 0)