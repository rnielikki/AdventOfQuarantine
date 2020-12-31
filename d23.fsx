let input = "963275481" |> Seq.map (fun i-> i.ToString() |> int)// |> Array.ofSeq


let loadDictionary (numberSeq:seq<int>) =
    let inputHead = numberSeq |> Seq.head
    let rec loadDictionaryRecursive (dictionary:Map<int,int>) (inputSeq:seq<int>) =
        let head = inputSeq |> Seq.head
        let skipped = inputSeq |> Seq.skip 1
        if (skipped |> Seq.isEmpty) then
            dictionary |> Map.add head inputHead
        else
            let next = skipped |> Seq.head
            loadDictionaryRecursive (dictionary |> Map.add head next) skipped
    loadDictionaryRecursive Map.empty numberSeq

let chainPrint (start:int) (dictionary:Map<int,int>) =
    let rec chainPaintRecursive (current:int) =
        let next = dictionary.[current]
        if next = start then
            printfn ""
            ()
        else
            printf "%d" next
            chainPaintRecursive next
    chainPaintRecursive start

let changeDic (key:int) (value:int) (dictionary:Map<int,int>) =
    (dictionary |> Map.remove key).Add(key, value)


let startGame (count:int) (startTarget:int) (dictionary:Map<int,int>) =
    let cupLength = dictionary |> Map.count
    let rec takeCup (dictionary:Map<int,int>) (target:int) (count:int) =
        if (count < 1) then
            dictionary
        else
            let rec getNonDuplicated (collection:array<int>) (target:int)=
                let value = (cupLength+target-1)%cupLength+1;
                if (collection |> Array.exists (fun f -> f = value)) then
                    getNonDuplicated collection (value-1)
                else
                    value

            let first = dictionary.[target]
            let second = dictionary.[first]
            let third = dictionary.[second]
            let nextTarget = dictionary.[third]
            let elements = [| first; second; third |]
            let dest = getNonDuplicated elements (target-1)

            let beforeElement = dictionary |> Map.find

            let resDictionary = dictionary |> changeDic target nextTarget |> changeDic dest first |> changeDic third dictionary.[dest]
            

            takeCup resDictionary nextTarget (count-1)
    takeCup dictionary startTarget count

//----------1
loadDictionary input |> startGame 100 (input |> Seq.head) |> chainPrint 1

//----------2
//loadDictionary input |> changeDic (input |> Seq.last) 10

let tails = [| 10..1000000 |] |> Array.fold (fun (acc:Map<int,int>) data -> acc.Add(data,(data+1)) ) Map.empty |> changeDic 1000000 (input |> Seq.head)
let heads = loadDictionary input |> changeDic (input |> Seq.last) 10
//printfn "%A" heads

//let bigResult = Map.fold (fun acc key value -> Map.add key value acc) heads tails |> startGame 10000000 (input |> Seq.head)


let bigResult = Map.fold (fun acc key value -> Map.add key value acc) heads tails |> startGame 10000000 (input |> Seq.head)
let res1 = bigResult.[1]
let res2 = bigResult.[res1]

printfn "%A" ((res1 |> int64)*(res2 |> int64))

(*
try
    let test = Map(Seq.concat [heads;tails]) |> startGame 10000000 (input |> Seq.head)
    ()
with
    | :? System.Collections.Generic.KeyNotFoundException as ex -> printfn "%A" ex.Data
*)

//let res = Map(Seq.concat [heads;tails]) |> startGame 10000000 (input |> Seq.head)
//let d1 = res.[1]
//let d2 = res.[d1]

//printfn "%A" ((d1 |> int64)*(d2 |> int64))

//bigDictionary |> changeDic (bigDictionary |> Seq.last) (bigDictionary |> Seq.first)

//let bigResult = [| 10..(1000000-9) |] |> Seq.append input |> loadDictionary
// |> startGame 10000000 (input |> Seq.head)

//let res1 = bigResult.[1]
//let res2 = bigResult.[res1]
//printfn "%A" ((res1 |> int64)*(res2 |> int64))