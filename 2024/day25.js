const raw = document.body.firstElementChild.textContent;
const lineBreak = (raw.indexOf("\r\n")>-1)?"\r\n":"\n";
const data = raw.split(lineBreak+lineBreak).reduce((a,b)=>{
   let crop = b.split(lineBreak);
   if(crop[0]==="#####"){
      a.lock.push(makePin(crop));
   }
   else {
      a.key.push(makePin(crop));
   }
   return a;
   function makePin(arr){
      let sum = [-1,-1,-1,-1,-1];
      for(let y=0;y<arr.length;y++) {
          for(let x=0;x<arr[0].length;x++) {
              if(arr[y][x]=="#") sum[x]++;
          }
      }
      return sum;
   }
},{key:[], lock:[]});

let result = 0;
for(let lock of data.lock) {
   for(let key of data.key) {
      if(isFit(lock, key)){
          result++;
      }
   }
}
console.log(result);

function isFit(lock, key){
    for(let i=0;i<5;i++){
         if(lock[i]+key[i]>5) return false;
    }
    return true;
}