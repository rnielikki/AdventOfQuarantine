const data = document.getElementsByTagName("pre")[0].textContent.split("\n").map(a => {
    let index = a.indexOf(":");
    return {
       result: Number(a.substring(0, index)),
       inputs: a.substring(index+2).split(" ")
    };
 });
 
 let res = 0;
 
 for(const oneData of data) {
    res += calc(oneData, 0, oneData.inputs[0]) * oneData.result;
 }
 
 console.log(res);
 
 function calc(line, currentIndex, currentValue) {
    let res = 0;
    if(currentValue > Number(line.result)) return 0;
    if(currentIndex == line.inputs.length-1) {
       return line.result == currentValue;
    }
    let nextIndex = currentIndex+1;
    let val2 = line.inputs[nextIndex];
    res |= calc(line, nextIndex, Number(currentValue)+Number(val2));
    res |= calc(line, nextIndex, currentValue * val2);
    res |= calc(line, nextIndex, currentValue + val2);
    return res;
 }