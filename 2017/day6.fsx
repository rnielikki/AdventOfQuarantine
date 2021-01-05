open System.IO;

let banks = File.ReadAllText("day6.txt").Split(' ') |> Array.map (fun f->f |> int);

let Deposit (banks:int[]) =
    let distSize = (banks |> Array.length)-1
    let rec depositRec (banks:int[]) (duplication:List<int[]>) (step:int) =
        if (duplication |> List.contains(banks)) then
            (duplication, banks, step)
        else
            let max = banks |> Array.max
            let index = banks |> Array.findIndex (fun x -> x = max)
            let distribute = max/(List.min [distSize;max])
            let newBanks = banks |> Array.indexed |> Array.map(fun f ->
                let getReal (arrayPos, item) =
                    if arrayPos = index then
                        (arrayPos, 0)
                    else
                        (arrayPos,item)
                let (arrayPos, item) = getReal f;
                if ((index-arrayPos+distSize+1)%(distSize+1)) > ((distSize-max)%(distSize+1)) then
                    item+distribute
                else
                    item+distribute-1
                    
            )
            depositRec newBanks (duplication @ [banks]) (step+1)
    depositRec banks List.empty 0

let (dup, currentBanks, step) = Deposit banks
printfn "%A" step
printfn "%A" ((dup |> List.length) - (dup |> List.findIndex(fun f -> f = currentBanks)))