const raw = document.querySelector("pre").textContent.split("\n").filter(x=>x!="");
function calcRecursion(input)
{
    let text = input;
    
    while(text.lastIndexOf("(")!==-1){
        let starts = text.lastIndexOf("(");
        let startCut = text.substring(starts+1);
        let ends = startCut.indexOf(")");
        text = text.substring(0,starts) + calcRecursion(startCut.substring(0, ends)) + startCut.substring(ends+1);
    }
    return universalCalc(text);
}

function calcRecursionPlus(input)
{
    let text = input;
    
    while(text.lastIndexOf("(")!==-1){
        let starts = text.lastIndexOf("(");
        let startCut = text.substring(starts+1);
        let ends = startCut.indexOf(")");
        text = text.substring(0,starts) + calcRecursionPlus(startCut.substring(0, ends)) + startCut.substring(ends+1);
    }
    return text.split("*").map(item=>universalCalc(item)).reduce((acc, val)=>acc*val, 1);
}

function universalCalc(text){
    let result = 0;
    let parsed = parseInt(text);
    result+= parsed;
    text = text.substring(parsed.toString().length);
    while(text){
        let parsed = parseInt(text.substring(1));
        switch(text[0]){
            case "+":
                result+=parsed;
                break;
            case "*":
                result*=parsed;
                break;
            default:
                throw "operation error. remember remove whitespaces first";
        }
        text = text.substring(parsed.toString().length+1);
    }
    return result;
}
//one
console.log("one: "+raw.map(d=>calcRecursion(d.replaceAll(" ", ""))).reduce((acc,val)=>acc+val,0));

//two
console.log("two:"+raw.map(d=>calcRecursionPlus(d.replaceAll(" ", ""))).reduce((acc,val)=>acc+val,0));