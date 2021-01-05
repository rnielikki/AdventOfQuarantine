let input = 265149;

let rec loadValue (item:int) (offset:int) =
    if (item-offset*8<0) then
        let dist = offset*2;
        offset + (abs ((item % dist)-offset)) - 1
    else
        loadValue (item-offset*8) (offset+1)


printfn "%d" (loadValue input 1)
