var raw = document.querySelector("pre").textContent.split("\n");
var player2 = raw.splice(raw.indexOf("Player 2:")).filter(x=>x!=="");
var player1 = raw.filter(x=>x!=="");
player2.shift();
player1.shift();

function recursiveCombat(rp1, rp2, copy=true){
    let p1;
    let p2;
    let exists = new Set();
    if(copy){
         p1=[...rp1];
         p2=[...rp2];
    }
    else{
         p1=rp1;
         p2=rp2;
    }
   while (p1.length && p2.length)
    {
        var currentStr = JSON.stringify(p1)+"/"+JSON.stringify(p2);
        if
        (exists.has(currentStr))
        {
            //gameEnds for p1.
            return 1;
        }
        else
        {
             exists.add(currentStr);
        }
        let s1 = p1.shift();
        let s2 = p2.shift();
        let n1 = Number(s1);
        let n2 = Number(s2);
        if (n1 <= p1.length && n2 <= p2.length)
        {
            switch(recursiveCombat(p1.slice(0,n1), p2.slice(0,n2))){
                case 1:
                    setCard(p1, s1, s2);
                    break;
                case 2:
                    setCard(p2, s2, s1);
                    break;
            }
        }
        else
        {
            //if recursive combat not valid
            if(n1 > n2)
            {
                setCard(p1, s1, s2);
            }
            else if(n1 < n2)
            {
                setCard(p2, s2, s1);
            }
        }
    }
    if(p1.length) return 1;
    else return 2;
}
function setCard(winner, first, second){
    winner.push(first);
    winner.push(second);
}

console.log(recursiveCombat(player1, player2, false));

let winningQueue = (player1.length)?player1:player2;
let calcIndex = winningQueue.length;
console.log(winningQueue.reduce((acc,item)=>acc+item*(calcIndex--), 0));

console.log("--------------**** res *** ---------------------");

for(var a of player1){
    console.log(a);
}
console.log("--------------");
for(var a of player2){
    console.log(a);
}