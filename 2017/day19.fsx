open System.IO;

let Move =
    let map = File.ReadAllLines("day19.txt") |> Array.map(fun item -> item.ToCharArray());
    let startX = Array.findIndex (fun arr->arr = '|') map.[0]

    let maxX = map.[0].Length;
    let maxY = map.Length;

    let rec moveRec(x:int) (y:int) (directionOffset:(int*int)) (text:string) (movedSum:int) =
        if (x<0 || y<0 || x>=maxX || y>=maxY ) then
            (text, movedSum)
        else
            let (moveX, moveY) = directionOffset;
            let c = map.[y].[x];
            //printfn "%d %d [%c]" x y c
            match c with
            | '|' | '-' -> moveRec (x+moveX) (y+moveY) directionOffset text (movedSum+1)
            | '+' ->
                if moveX <> 0 then
                    if ((y+1) >= maxY || map.[y+1].[x] = ' ') then
                        moveRec x (y-1) (0, -1) text (movedSum+1)
                    else
                        moveRec x (y+1) (0, 1) text (movedSum+1)
                else
                    if ((x+1) >= maxX || map.[y].[x+1] = ' ') then
                        moveRec (x-1) y (-1, 0) text (movedSum+1)
                    else
                        moveRec (x+1) y (1, 0) text (movedSum+1)
            | ' ' -> (text, movedSum)
            | _ when System.Char.IsLetter c ->
                moveRec (x+moveX) (y+moveY) directionOffset (text+c.ToString()) (movedSum+1)
            | _ -> invalidArg "map" (sprintf "%c is not valid" map.[y].[x])
        
    moveRec startX 0 (0,1) "" 0

let (str, count) = Move
printfn "%s %d" str count