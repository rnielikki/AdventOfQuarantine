const raw = document.querySelector("pre").textContent.split("\n").filter(v=>v);
const data = raw.reduce((acc, item)=>{
    var l = item.split("(");
    var ing = l[0].split(" ").slice(0,-1);
    var alle = l[1].split(" ").slice(1).map(s=>s.slice(0,-1));
           
    for(let al of alle){
        if(!acc.hasOwnProperty(al)) acc[al] = ing;
        else{
            acc[al] = acc[al].filter(a=>ing.includes(a));
        }
    }
    return acc;
}, {});

let allIngs = raw.reduce((acc, item)=>{
    var arr = item.split("(")[0].split(" ").slice(0,-1);
    return acc.concat(arr);
},[]);

let allergianAll= Object.values(data).reduce((acc,d)=>acc.concat(d),[]);

//first.
allIngs.filter(ing=>!allergianAll.includes(ing)).length;
let currentStatus = Object.keys(data).reduce((acc,d)=>{ acc.push([d,data[d]]); return acc;}, []);

let result = []; //for future sorting

while(currentStatus.length != 0)
{
    let targets = currentStatus.filter(targ=>targ[1].length==1);
    for(let target of targets){
        result.push([target[0],target[1][0]]);
        currentStatus = currentStatus.map(f=>[f[0], f[1].filter(a=>a!=target[1][0])] ).filter(a=>a[1].length!=0);
    }
}

//second.
result.sort((comp1,comp2)=>(comp1[0]<comp2[0])?-1:1).map(v=>v[1]).join(",")