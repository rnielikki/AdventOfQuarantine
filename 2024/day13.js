let raw = document.body.firstChild.textContent.split("\n");
let data = [];
let current = {};
for(let i=0;i<raw.length;i++) {
    switch(i%4) {
        case 0:
           current.A = parse(raw[i].substring(10));
           break;
        case 1:
           current.B = parse(raw[i].substring(10));
           break;
        case 2:
           current.prize = parse(raw[i].substring(7));
           current.prize.x+=10000000000000;
           current.prize.y+=10000000000000;
           data.push(current);
           current = {};
           break;
        default:
           break;
    }
}



function parse(r) {
    let sp = r.split(", ");
    return {x: Number(sp[0].substring(2)), y:Number(sp[1].substring(2))}
}

//----------------

let sum = 0;
for(let d of data) {
    sum+=find(d);
}
console.log(sum);


function find(input) {
     let a = input.A.x; let b = input.A.y; let c = input.B.x; let d = input.B.y;
     let rx = input.prize.x; let ry = input.prize.y;
     if(b*c==a*d) return 0;
     let n = (d*rx-c*ry)/(a*d-b*c);
     let m = (b*rx-a*ry)/(b*c-a*d);
     if(n < 0 || m < 0 || n%1!==0 || m%1!==0) {
          return 0;
     }
     return n*3+m;
}