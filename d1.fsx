open System.IO;

//note the data is sorted
let data = File.ReadAllLines("d1.txt") |> Array.map(fun item-> item |> int32) |> Array.sortBy(fun x -> x)

let findTwoSum() =
    let rec findTwoSumRecursive index1 index2 =
        if (index2>=data.Length) then
            findTwoSumRecursive (index1+1) 0
        else if (index1>=data.Length) then
            printfn "well..."
            -1
        else
            let result = data.[index1] + data.[index2]
            if (result = 2020) then
                (data.[index1] * data.[index2])
            else if (result < 2020 ) then
                findTwoSumRecursive index1 (index2+1)
            else
                findTwoSumRecursive index1 (index2+1)
    findTwoSumRecursive 0 0

printfn "%A" (findTwoSum())

let findThreeSum() =
    let rec findRecursive index1 index2 index3 =
            let l1=index1
            let l2=index2
            let l3=index3

            let sum = data.[l1]+data.[l2]+data.[l3]
            if(sum=2020) then
                data.[l1]*data.[l2]*data.[l3]
            else
            //if-else looks so nasty so ¯\_(ツ)_/¯
                let nextIndex3 = l3+1
                let nextIndex2 = l2 + nextIndex3/data.Length
                let nextIndex1 = l1 + nextIndex2/data.Length
                findRecursive (nextIndex1%data.Length) (nextIndex2%data.Length) (nextIndex3%data.Length)
    findRecursive 0 0 0

printfn "%A" (findThreeSum())