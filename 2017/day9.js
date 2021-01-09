/* regex ¯\_(ツ)_/¯ */

const preReady = document.querySelector("pre").textContent
.replace(/!./g,"");

const ready = preReady
.replace(/<[^>]*>/g,"");

let groupLevel = 0;
let score = 0;

for(let i=0;i<ready.length;i++){
    switch(ready[i]){
        case '{':
            groupLevel++;
            break;
        case '}':
            score+=groupLevel--;
            break;
    }
}

console.log(score);

const part2 = preReady
.match(/<[^>]*>/g);

console.log(part2.reduce((acc,str)=>acc+str.length, 0) - part2.length*2);