open System.IO;

let startRead xgap ygap =
    let reader = new StreamReader("d3.txt");
    let rec read currentX currentY count =
        if reader.EndOfStream then
            count
        else if currentY%ygap <> 0 then
            let line = reader.ReadLine();
            read currentX (currentY+1) count;
        else
            let line = reader.ReadLine().ToCharArray()
            match line.[currentX] with
            | '#' -> read ((currentX+xgap)%line.Length) (currentY+1) (count+1);
            | '.' -> read ((currentX+xgap)%line.Length) (currentY+1) count;
            | _ -> raise (System.FormatException("not # or ."))
    read 0 0 0 |> int64

printfn "%A" (startRead 3 1)
printfn "%A" ((startRead 3 1)*(startRead 1 1)*(startRead 5 1)*(startRead 7 1)*(startRead 1 2))