const data = document.querySelector("pre").textContent.split("\n").filter(x=>x!="").reduce((acc,item)=>{
    let [minmax, rawcrit, password] = item.split(" ");
    let [min, max] = minmax.split("-");
    acc.push({
        min:min, max:max,
        crit:rawcrit[0],
        password:password
    })
    return acc;
},[]);

var c1_valid = 0;
for (var d of data){
    let count = 0;
    for(var c of d.password){
        if(c==d.crit) count++;
    }
    if(count>=d.min && count <= d.max) c1_valid++;
}
console.log(c1_valid)

var c2_valid = 0;
for(var d of data){
    if(
        d.password[d.min-1] == d.crit
         ^   
        d.password[d.max-1] == d.crit
    ) c2_valid++;
}
console.log(c2_valid);